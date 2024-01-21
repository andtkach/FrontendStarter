using System.Text.Json.Serialization;

namespace Jiwebapi.Catalog.Identity.Dtos
{
    internal class ErrorResponse
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        public ErrorResponse() : this(string.Empty, string.Empty)
        {
        }

        public ErrorResponse(string code, string message)
        {
            this.Code = code;
            this.Message = message;
        }
    }
}
