using AuctionService.DTOs;
using AuctionService.Entities;
using AutoMapper;
using Contracts;

namespace AuctionService.RequestHelpers;

// Once we have the DTOs, we need some mapping profiles for AutoMapper to work with so we can automatically map us from the Auction to the
// AuctionDto

// If we have a property in the entity Auction.cs called ReservePrice, and we want AutoMapper to automatically map something into the AuctionDto,
// as long as it's got the same name as the mentioned property, then AutoMapper will take care of this mapping for us
public class MappingProfiles: Profile
{
    public MappingProfiles()
    {   
        //create mapping profiles
        CreateMap<Auction, AuctionDto>().IncludeMembers(x => x.Item);
        CreateMap<Item, AuctionDto>();
        CreateMap<CreateAuctionDto, Auction>().ForMember(d => d.Item, o => o.MapFrom(s => s));
        CreateMap<CreateAuctionDto, Item>();
        // So in order to publish the created aution to the service bus, first, we need to map it into a AuctionCreated object
        CreateMap<AuctionDto, AuctionCreated>();

    }
}