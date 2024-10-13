namespace RFService.RepoLib
{
    public class GetOptions
    {
        public object? Filters { get; set; }
        public Dictionary<string, GetOptions> Include { get; set; } = [];
    }
}
