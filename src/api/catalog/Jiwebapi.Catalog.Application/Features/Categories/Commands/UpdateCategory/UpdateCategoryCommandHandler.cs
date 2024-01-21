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

namespace Jiwebapi.Catalog.Application.Features.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand>
    {
        private readonly IAsyncRepository<Category> _categoryRepository;
        private readonly IMapper _mapper;
        private readonly ILoggedInUserService _loggedInUserService;
        private readonly IPublishService _publishService;
        private readonly ILogger<UpdateCategoryCommandHandler> _logger;

        public UpdateCategoryCommandHandler(IMapper mapper, IAsyncRepository<Category> categoryRepository,
            ILoggedInUserService loggedInUserService, IPublishService publishService, ILogger<UpdateCategoryCommandHandler> logger)
        {
            _mapper = mapper;
            _categoryRepository = categoryRepository;
            _loggedInUserService = loggedInUserService;
            _publishService = publishService;
            _logger = logger;
        }

        public async Task Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"UpdateCategoryCommandHandler for {_loggedInUserService.DataTraceId} data trace id");

            var categoryToUpdate = await _categoryRepository.GetByIdAsync(request.CategoryId);
            if (categoryToUpdate == null)
            {
                throw new NotFoundException(nameof(Category), request.CategoryId);
            }

            var validator = new UpdateCategoryCommandValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (validationResult.Errors.Count > 0)
                throw new ValidationException(validationResult);

            _mapper.Map(request, categoryToUpdate, typeof(UpdateCategoryCommand), typeof(Category));

            categoryToUpdate.UserId = Guid.Parse(_loggedInUserService.UserId);
            await _categoryRepository.UpdateAsync(categoryToUpdate);

            await _publishService.PublishEvent(new CategoryUpdated
                {
                    Id = Guid.NewGuid(),
                    UserId = _loggedInUserService.UserId,
                    Name = categoryToUpdate.Name,
                    CategoryId = categoryToUpdate.CategoryId,
                    DataTraceId = _loggedInUserService.DataTraceId,
            });
        }
    }
}