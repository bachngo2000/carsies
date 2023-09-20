using AuctionService.Data;
using Microsoft.EntityFrameworkCore;

// builder is a WebApplicationBuilder
// builder has configuration, logging, and many other services added to the DI (dependency injection) container.
var builder = WebApplication.CreateBuilder(args);

// 1. Add services to the container (Services required by the app are configured)
builder.Services.AddControllers();

// added a service to our container for the DbContext
// This particular class, DbContext, is smth that we're going to want to inject into classes that need to get access to something in the datbase
// So we add this as a service, and then we use dependency injection in order to make use of that service
// "AuctionDbContext" - name of the DBContext we created
builder.Services.AddDbContext<AuctionDbContext>(opt => {
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// added AutoMapper as a service
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//build our application
var app = builder.Build();

// Configure the HTTP request pipeline (The app's request handling pipeline is defined as a series of middleware components)

// When a request comes into one of our API controllers, it goes through the HTTP request pipeline.  This gives us the option to add software or
// middleware, which can then affect or do smth with that request in some way as it comes into our API server and leaves our API server
app.UseAuthorization();

// middlware to map the controllers. Each one of our API controllers is going to have a route and this middleware allows the framework to direct the HTTP request
// to the correct API endpoint
app.MapControllers();

// we need to get hold our DBContext service
try
{
    DbInitializer.InitDb(app);
}
catch (Exception e)
{
    
    Console.WriteLine(e);
}

// run our application
app.Run();
