using MediatR;

namespace Jiwebapi.Catalog.Application.Features.Categories.Commands.DeleteCategory
{
    public class DeleteCategoryCommand: IRequest
    {
        public Guid CategoryId { get; set; }
    }
}
