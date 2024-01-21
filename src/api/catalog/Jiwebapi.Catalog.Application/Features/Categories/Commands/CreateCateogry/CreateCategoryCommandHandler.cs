using AutoMapper;
using Jiwebapi.Catalog.Application.Contracts;
using Jiwebapi.Catalog.Application.Contracts.Cache;
using Jiwebapi.Catalog.Application.Contracts.Message;
using Jiwebapi.Catalog.Application.Contracts.Persistence;
using Jiwebapi.Catalog.Domain.Common;
using Jiwebapi.Catalog.Domain.Entities;
using Jiwebapi.Catalog.Domain.Message.Category;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Jiwebapi.Catalog.Application.Features.Categories.Commands.CreateCateogry
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, CreateCategoryCommandResponse>
    {
        private readonly IAsyncRepository<Category> _categoryRepository;
        private readonly IMapper _mapper;
        private readonly ILoggedInUserService _loggedInUserService;
        private readonly IPublishService _publishService;
        private readonly ILogger<CreateCategoryCommandHandler> _logger;

        public CreateCategoryCommandHandler(IMapper mapper, IAsyncRepository<Category> categoryRepository,
            ILoggedInUserService loggedInUserService, IPublishService publishService, ILogger<CreateCategoryCommandHandler> logger)
        {
            _mapper = mapper;
            _categoryRepository = categoryRepository;
            _loggedInUserService = loggedInUserService;
            _publishService = publishService;
            _logger = logger;
        }

        public async Task<CreateCategoryCommandResponse> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"CreateCategoryCommandHandler for {_loggedInUserService.DataTraceId} data trace id");

            var createCategoryCommandResponse = new CreateCategoryCommandResponse();

            var validator = new CreateCategoryCommandValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (validationResult.Errors.Count > 0)
            {
                createCategoryCommandResponse.Success = false;
                createCategoryCommandResponse.ValidationErrors = new List<string>();
                foreach (var error in validationResult.Errors)
                {
                    createCategoryCommandResponse.ValidationErrors.Add(error.ErrorMessage);
                }
            }
            if (createCategoryCommandResponse.Success)
            {
                var category = new Category() { Name = request.Name };
                category.UserId = Guid.Parse(_loggedInUserService.UserId);
                category = await _categoryRepository.AddAsync(category);
                createCategoryCommandResponse.Category = _mapper.Map<CreateCategoryDto>(category);
                
                await _publishService.PublishEvent(new CategoryCreated
                {
                    Id = Guid.NewGuid(),
                    UserId = _loggedInUserService.UserId,
                    Name = category.Name,
                    CategoryId = category.CategoryId,
                    DataTraceId = _loggedInUserService.DataTraceId,
                });
            }

            return createCategoryCommandResponse;
        }
    }
}
