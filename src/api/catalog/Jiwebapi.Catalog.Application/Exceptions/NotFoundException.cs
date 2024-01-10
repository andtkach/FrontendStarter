namespace Jiwebapi.Catalog.Application.Exceptions
{
    public class NotFoundException : Exception
    {
        public string Code { get; private set; }

        public NotFoundException(string name, object key)
            : base($"{name} ({key}) is not found")
        {
            Code = string.Empty;
        }
        
        public NotFoundException(string code, string name, object key)
            : base($"{name} ({key}) is not found")
        {
            this.Code = code;
        }
    }
}
