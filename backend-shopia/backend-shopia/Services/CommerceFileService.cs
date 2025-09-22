using backend_shopia.DTO;
using backend_shopia.Entities;
using backend_shopia.Exceptions;
using backend_shopia.IServices;
using RFAuth.Exceptions;
using RFOperators;
using RFService.IRepo;
using RFService.Repo;
using RFService.Services;

namespace backend_shopia.Services
{
    public class CommerceFileService(
        IRepo<CommerceFile> repo,
        IServiceProvider serviceProvider
    )
        : ServiceCreatedAtIdUuidName<IRepo<CommerceFile>, CommerceFile>(repo),
            ICommerceFileService
    {
        public override async Task<CommerceFile> ValidateForCreationAsync(CommerceFile data)
        {
            data = await base.ValidateForCreationAsync(data);

            if (string.IsNullOrWhiteSpace(data.Name))
                throw new NoNameException();

            if (data.Commerce == null)
            {
                if (data.CommerceId <= 0)
                    throw new NoCommerceException();

                var commerceService = serviceProvider.GetRequiredService<ICommerceService>();
                data.Commerce = await commerceService.GetSingleOrDefaultForIdAsync(data.CommerceId);
                if (data.Commerce == null)
                    throw new NoCommerceException();
            }

            var userPlanService = serviceProvider.GetRequiredService<IUserPlanService>();
            var limits = await userPlanService.GetLimitsForCurrentUserAsync();

            if (data.Content.Length > limits[PlanLimitName.MaxCommerceImageSize])
                throw new ImageIsTooLargeException();

            var itemImagesCount = await GetCountAsync(new QueryOptions
            {
                Switches = { { "IncludeDisabled", true } },
                Filters = { { "CommerceId", data.CommerceId } }
            });
            if (itemImagesCount >= limits[PlanLimitName.MaxTotalImagesPerSingleCommerce])
                throw new TotalImagesPerCommerceLimitReachedException();

            var totalCount = await GetCountForCurrentUserAsync(new QueryOptions { Switches = { { "IncludeDisabled", true} } });
            if (totalCount >= limits[PlanLimitName.MaxTotalCommercesImages])
                throw new TotalCommercesImagesLimitReachedException();

            var enabledCount = await GetCountForCurrentUserAsync();
            if (enabledCount > limits[PlanLimitName.MaxEnabledCommercesImages])
                throw new MaxEnabledCommercesImagesLimitReachedException();

            var aggregatedSize = await GetAggregatedSizeForCurrentUserAsync(new QueryOptions { Switches = { { "IncludeDisabled", true } } });
            aggregatedSize += data.Content.Length;
            if (aggregatedSize >= limits[PlanLimitName.MaxCommercesImagesAggregatedSize])
                throw new TotalCommercesImagesAggregatedSizeLimitReachedException();

            var enabledAggregatedSize = await GetAggregatedSizeForCurrentUserAsync();
            enabledAggregatedSize += data.Content.Length;
            if (enabledAggregatedSize >= limits[PlanLimitName.MaxEnabledCommercesImagesAggregatedSize])
                throw new MaxEnabledCommercesImagesAggregatedSizeLimitReachedException();

            return data;
        }

        public async Task<QueryOptions> GetFilterForOwnerIdAsync(Int64 ownerId, QueryOptions? options = null)
        {
            var commerceService = serviceProvider.GetRequiredService<ICommerceService>();
            var commercesId = await commerceService.GetListIdForOwnerIdAsync(ownerId, options);

            options = new QueryOptions();
            options.AddFilter("CommerceId", commercesId);

            return options;
        }

        public async Task<int> GetCountForOwnerIdAsync(Int64 ownerId, QueryOptions? options = null)
            => await GetCountAsync(await GetFilterForOwnerIdAsync(ownerId, options));

        public Int64 GetCurrentUserId()
        {
            var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
            var httpContext = httpContextAccessor.HttpContext
                ?? throw new NoAuthorizationHeaderException();

            var userId = (httpContext.Items["UserId"] as Int64?)
                ?? throw new NoSessionUserDataException();

            if (userId <= 0)
                throw new NoSessionUserDataException();

            return userId!;
        }

        public async Task<int> GetCountForCurrentUserAsync(QueryOptions? options = null)
            => await GetCountForOwnerIdAsync(GetCurrentUserId(), options);

        public async Task<Int64> GetAggregatedSizeForOwnerIdAsync(Int64 ownerId, QueryOptions? options = null)
        {
            options = await GetFilterForOwnerIdAsync(ownerId, options);
            options.Select ??= [Op.Sum(Op.DataLength("Content"))];

            return await GetInt64Async(options) ?? 0;
        }

        public async Task<Int64> GetAggregatedSizeForCurrentUserAsync(QueryOptions? options = null)
            => await GetAggregatedSizeForOwnerIdAsync(GetCurrentUserId(), options);

        public async Task<IEnumerable<CommerceFile>> AddForCommerceUuidAsync(Guid commerceUuid, FilesCollectionDTO files)
        {
            var commerceService = serviceProvider.GetRequiredService<ICommerceService>();
            return await AddForCommerceIdAsync(await commerceService.GetSingleIdForUuidAsync(commerceUuid), files);
        }

        public async Task<IEnumerable<CommerceFile>> AddForCommerceIdAsync(Int64 commerceId, FilesCollectionDTO files)
        {
            var result = new List<CommerceFile>();
            foreach (var file in files)
            {
                if (file.Content.Length == 0)
                    continue;

                var commerceImage = new CommerceFile
                {
                    CommerceId = commerceId,
                    Name = file.Name,
                    ContentType = file.ContentType,
                    Content = file.Content,
                };

                result.Add(await CreateAsync(commerceImage));
            }

            return result;
        }

        public async Task<IEnumerable<CommerceFile>> GetListForCommerceIdAsync(Int64 commerceId)
            => await GetListAsync(new QueryOptions
            {
                Filters = { { "CommerceId", commerceId } }
            });
    }
}