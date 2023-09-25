using AutoMapper;
using Contracts;
using SearchService.Models;

namespace SearchService;

// we need a mapping profile so that we can go from AuctionCreated to the Item, which is what MongoDB needs to save
public class MappingProfiles : Profile
{
    public MappingProfiles() {
        // created a mapping from AuctionCreated to item in Search service
        CreateMap<AuctionCreated,  Item>();

        CreateMap<AuctionUpdated,  Item>();

    }

}
