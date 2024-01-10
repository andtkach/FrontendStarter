using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jiwebapi.Catalog.Application.Features.Categories.Queries.GetCategoriesListWithEvents
{
    public class CategoryEventListVmResponse
    {
        public required IEnumerable<CategoryEventListVm> Result { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
    }
}
