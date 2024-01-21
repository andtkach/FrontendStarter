namespace Jiwebapi.Catalog.Application.Models
{
    public class BaseVmResponse
    {
        public required IVmData Data { get; set; }
        public string Meta { get; set; } = string.Empty;
    }
}