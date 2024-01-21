using AutoMapper;
using Jiwebapi.Catalog.Application.Features.Categories.Commands.CreateCateogry;
using Jiwebapi.Catalog.Application.Features.Categories.Commands.UpdateCategory;
using Jiwebapi.Catalog.Application.Features.Categories.Queries.GetCategoriesList;
using Jiwebapi.Catalog.Application.Features.Categories.Queries.GetCategoriesListWithEvents;
using Jiwebapi.Catalog.Application.Features.Categories.Queries.GetCategoryDetail;
using Jiwebapi.Catalog.Application.Features.Events.Commands.CreateEvent;
using Jiwebapi.Catalog.Application.Features.Events.Commands.UpdateEvent;
using Jiwebapi.Catalog.Application.Features.Events.Queries.GetEventDetail;
using Jiwebapi.Catalog.Application.Features.Events.Queries.GetEventsList;
using Jiwebapi.Catalog.Domain.Entities;

namespace Jiwebapi.Catalog.Application.Profiles
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<Event, EventListVm>().ReverseMap();
            CreateMap<Event, CreateEventCommand>().ReverseMap();
            CreateMap<Event, UpdateEventCommand>().ReverseMap();
            CreateMap<Event, EventDetailVm>().ReverseMap();
            CreateMap<Event, CategoryEventDto>().ReverseMap();
            CreateMap<Event, CreateEventDto>();

            CreateMap<Category, CategoryDto>();
            CreateMap<Category, CategoryListVm>();
            CreateMap<Category, CategoryEventListVm>();
            CreateMap<Category, CreateCategoryCommand>();
            CreateMap<Category, UpdateCategoryCommand>().ReverseMap();
            CreateMap<Category, CreateCategoryDto>();
            CreateMap<Category, CategoryDetailVm>().ReverseMap();
        }
    }
}
