namespace BFF.Services.Category.DTO
{
    public class CategoriesResponse
    {
        public required IEnumerable<Category> Result { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
    }
}
