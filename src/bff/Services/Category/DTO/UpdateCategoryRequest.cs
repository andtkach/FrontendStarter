﻿namespace BFF.Services.Category.DTO
{
    public class UpdateCategoryRequest
    {
        public Guid CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
