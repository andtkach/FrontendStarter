using Jiwebapi.Catalog.Application.Responses;

namespace Jiwebapi.Catalog.Application.Features.Categories.Commands.CreateCateogry
{
    public class CreateCategoryCommandResponse: BaseResponse
    {
        public CreateCategoryCommandResponse(): base()
        {
        }

        public CreateCategoryDto Category { get; set; } = default!;
    }
}