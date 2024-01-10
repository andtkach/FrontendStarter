using Jiwebapi.Catalog.Application.Models;

namespace Jiwebapi.Catalog.Application.Features.Categories.Queries.GetCategoryDetail
{
    public class CategoryDetailVm : IVmData
    {
        public Guid CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
