namespace backend_shopia.DTO
{
    public class FilesCollectionDTO : List<FileDTO>
    {
        public FilesCollectionDTO(IFormFileCollection formFiles)
        {
            foreach (var formFile in formFiles)
            {
                if (formFile.Length <= 0)
                    continue;

                var fileDTO = new FileDTO
                {
                    Name = formFile.FileName,
                    ContentType = formFile.ContentType,
                    Content = new byte[formFile.Length]
                };
                using (var stream = formFile.OpenReadStream())
                {
                    stream.Read(fileDTO.Content, 0, (int)formFile.Length);
                }

                Add(fileDTO);
            }
        }
    }
}
