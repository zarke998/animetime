namespace AnimeTime.WebsiteProcessors
{
    public interface IWebsiteProcessorFactory
    {
        IWebsiteProcessor CreateWebsiteProcessor(string websiteName, string websiteUrl, string querySuffix);
    }
}