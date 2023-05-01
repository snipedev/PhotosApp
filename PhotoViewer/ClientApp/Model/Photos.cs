using Newtonsoft.Json;

namespace ClientApp.Model
{
    public class Photos
    {
            [JsonProperty("id")]
            public string PhotoId { get; set; }
            [JsonProperty("title")]
            public string Title { get; set; }
            [JsonProperty("thumbNailUrl")]
            public string ThumbnailUrl { get; set; }
            [JsonProperty("largeUrl")]
            public string LargeUrl { get; set; }
            [JsonProperty("originalUrl")]
            public string OriginalUrl { get; set; }
    }

    public class PhotoResponse
    {
        public string Text { get; set; }
        public IList<Photos> Photos { get; set; }
    }
}
