using AutoMapper;
using Basket.API.Entities;
using EventBus.Messages.Events;

namespace Basket.API.Mappings;

public class MappingProfiles : Profile
{
	public MappingProfiles()
	{
		CreateMap<BasketCheckout, BasketCheckoutEvent>().ReverseMap();
	}
}
