using AutoMapper;
using Jiwebapi.Catalog.Application.Contracts;
using Jiwebapi.Catalog.Application.Contracts.Message;
using Jiwebapi.Catalog.Application.Contracts.Persistence;
using Jiwebapi.Catalog.Application.Exceptions;
using Jiwebapi.Catalog.Application.Features.Categories.Commands.CreateCateogry;
using Jiwebapi.Catalog.Domain.Entities;
using Jiwebapi.Catalog.Domain.Message.Category;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Jiwebapi.Catalog.Application.Features.Categories.Commands.DeleteCategory
{
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand>
    {
        private readonly IAsyncRepository<Category> _categoryRepository;
        private readonly IMapper _mapper;
        private readonly ILoggedInUserService _loggedInUserService;
        private readonly IPublishService _publishService;
        private readonly ILogger<DeleteCategoryCommandHandler> _logger;

        public DeleteCategoryCommandHandler(IMapper mapper, 
        IAsyncRepository<Category> categoryRepository,
        ILoggedInUserService loggedInUserService,
        IPublishService publishService, ILogger<DeleteCategoryCommandHandler> logger)
        {
            _mapper = mapper;
            _categoryRepository = categoryRepository;
            _loggedInUserService = loggedInUserService;
            _publishService = publishService;
            _logger = logger;
        }

        public async Task Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"DeleteCategoryCommandHandler for {_loggedInUserService.DataTraceId} data trace id");

            var categoryToDelete = await _categoryRepository.GetByIdAsync(request.CategoryId);

            if (categoryToDelete == null)
            {
                throw new NotFoundException(nameof(Event), request.CategoryId);
            }

            await _categoryRepository.DeleteAsync(categoryToDelete);

            await _publishService.PublishEvent(new CategoryDeleted
                {
                    Id = Guid.NewGuid(),
                    UserId = _loggedInUserService.UserId,
                    CategoryId = categoryToDelete.CategoryId,
                    DataTraceId = _loggedInUserService.DataTraceId,
            });
        }
    }
}
