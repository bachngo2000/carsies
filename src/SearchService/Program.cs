using System.Net;
using MassTransit;
using Polly;
using Polly.Extensions.Http;
using SearchService;
using SearchService.Data;
using SearchService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// added AutoMapper as a service so we can map consumer objects
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// add the AuctionSvcHttpClient service so we can request data from the Auction service

// Due to the nature of synchronous http communication, in which both the Auction service and the Search service have to be available for the database of the search service to be populated.  If the Auction service is temporarily unavailable and then becomes available again, before adding http polling, the Search service only requests data once,
// And what we can do is add a bit of resilience into our Http request inside our search service where we've got this. We would like this to repeat until such time as the data is available and we can get a successful response back from the Auction service, even if it's down for some time. Now there is an approach we can take using Http polling, which means our request is going to repeat and repeat and repeat until such time as it succeeds.
builder.Services.AddHttpClient<AuctionSvcHttpClient>().AddPolicyHandler(GetPolicy());

// added and configured MassTransit and RabbitMQ as a service as our service bus/message broker
builder.Services.AddMassTransit(x => 
{   
    // once we have a AuctionCreatedConsumer consumer, we need to tell MassTransit about it
    x.AddConsumersFromNamespaceContaining<AuctionCreatedConsumer>();

    x.SetEndpointNameFormatter( new KebabCaseEndpointNameFormatter("search", false));

    x.UsingRabbitMq((context, cfg) => {

        // Configure retry policies on a per-endpoint basis
        // specify "search-auction-created" as the name of our endpoint
        cfg.ReceiveEndpoint("search-auction-created", e => {
            // So effectively we're going to try it five times and wait for five seconds between each interval.
            // So this is only going to apply for the AuctionCreatedConsumer for this particular configuration.
            e.UseMessageRetry(r => r.Interval(5,5));
            // And we also need to specify which consumer we're configuring this for.
            e.ConfigureConsumer<AuctionCreatedConsumer>(context);
        });

        // Now we've got this configure endpoints, which is just going to configure all the endpoints based on the consumers that we have based on this line "x.AddConsumersFromNamespaceContaining<AuctionCreatedConsumer>();"
        cfg.ConfigureEndpoints(context);
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseAuthorization();

app.MapControllers();

app.Lifetime.ApplicationStarted.Register(async() => {
    try
    {
        await DbInitializer.InitDb(app);
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
    }   
});

app.Run();

// return an IAsyncPolicy from Polly
// the type of response we're looking for is the http response message from an http request
// So this is what's going to happen if our auction service is down. Then we're going to handle the exception and we're going to keep on trying every three seconds until the auction service is back up. At that point, we'll be able to make a successful request and we'll stop trying.
static IAsyncPolicy<HttpResponseMessage> GetPolicy()
    => HttpPolicyExtensions
        .HandleTransientHttpError()
        .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
        .WaitAndRetryForeverAsync(_ => TimeSpan.FromSeconds(3)); // we want to keep trying until the Auction service becomes available
