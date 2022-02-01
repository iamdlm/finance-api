using System.Net;
using System.Text.Json.Serialization;

namespace FinApi.Responses
{
    public abstract class BaseResponse
    {
        [JsonIgnore()]
        public HttpStatusCode StatusCode { get; set; }
        
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Message { get; set; }
    }

}
