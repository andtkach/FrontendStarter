namespace BFF.Services.Category.DTO
{
    public class Categories
    {
        public required List<Category> Result { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
    }
}
