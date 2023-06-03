using Newtonsoft.Json;

namespace NextGenSoftware.OASIS.API.Providers.RedisOASIS.Entities
{
    public class SearchData
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        
        [JsonProperty("data")]
        public string Data { get; set; }
    }
}
