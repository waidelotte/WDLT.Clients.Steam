using Newtonsoft.Json;

namespace WDLT.Clients.Steam.Models.Json
{
    public class JSuccess<T>
    {
        [JsonProperty("success")]
        public T Success { get; set; }
    }
}