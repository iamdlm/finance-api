using System.Text.Json.Serialization;

namespace Fin.Application.ViewModels
{
    public abstract class BaseResponse
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Message { get; set; }
    }
}
