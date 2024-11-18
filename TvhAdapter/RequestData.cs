namespace TvhAdapter
{
    /// <summary>
    /// Contains all neccesarry data for send request to TvHeadendServer.
    /// </summary>
    public class RequestData
    {
        public string apiPath { get; set; }
        public bool ignoreResponseData { get; set; } = false;

    }
}
