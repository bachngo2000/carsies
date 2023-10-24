using AuctionService;
using Grpc.Net.Client;

namespace BiddingService;
public class GrpcAuctionClient {

    private readonly ILogger<GrpcAuctionClient> _logger;
    private readonly IConfiguration _config;

    public GrpcAuctionClient(ILogger<GrpcAuctionClient> logger, IConfiguration config)
    {
        _logger = logger;
        _config = config;
    }

    // return an auction that results from sending a request to find an auction based on its id to the gRPC server in the Auction service
    public Auction GetAuction(string id)
    {
        _logger.LogInformation("Calling GRPC Service");
        // get hold of the gRPC channel
        var channel = GrpcChannel.ForAddress(_config["GrpcAuction"]);
        var client = new GrpcAuction.GrpcAuctionClient(channel);
        // request that goes to the gRPC server
        var request = new GetAuctionRequest{Id = id};

        try
        {
            var reply = client.GetAuction(request);
            var auction = new Auction
            {
                ID = reply.Auction.Id,
                AuctionEnd = DateTime.Parse(reply.Auction.AuctionEnd),
                Seller = reply.Auction.Seller,
                ReservePrice = reply.Auction.ReservePrice
            };

            return auction;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Could not call GRPC Server");
            return null;
        }
    }

}
