namespace Jiwebapi.Catalog.Domain.Common
{
    public class AuditableEntity
    {
        public string? CreatedBy { get; set; } 
        public DateTime CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
