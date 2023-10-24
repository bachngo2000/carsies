using AuctionService.Data;
using Grpc.Core;

namespace AuctionService;

// create a service to listen for our gRPC requests and send back the gRPC response
public class GrpcAuctionService: GrpcAuction.GrpcAuctionBase
{
    private readonly AuctionDbContext _dbContext;

    // inject our dbContext into this b/c we need access to our database to get the auction we're looking for 
    public GrpcAuctionService(AuctionDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public override async Task<GrpcAuctionResponse> GetAuction(GetAuctionRequest request, 
        ServerCallContext context) 
    {
        Console.WriteLine("==> Received Grpc request for an auction");

        // get hold of the auction the gRPC Client is looking for based on the request's id
        var auction = await _dbContext.Auctions.FindAsync(Guid.Parse(request.Id));

        if (auction == null) {
            throw new RpcException(new Status(StatusCode.NotFound, "Not found"));
        }

        // return the GrpcAuctionResponse
        var response = new GrpcAuctionResponse
        {   
            // the Auction that we return as part of the "response" is a GrpcAuctionModel
            Auction = new GrpcAuctionModel
            {   
                // fill out the properties that we've required for that particular object that the Bidding service needs for an auction
                // some properties need to be converted to String since there's no date object that we can use in gRPC
                AuctionEnd = auction.AuctionEnd.ToString(),
                Id = auction.Id.ToString(),
                ReservePrice = auction.ReservePrice,
                Seller = auction.Seller
            }
        };

        return response;
    }

}
