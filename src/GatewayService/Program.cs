var builder = WebApplication.CreateBuilder(args);

//add the configuration we need for the reverse proxy using Yarp into our program class
builder.Services.AddReverseProxy().LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

app.MapReverseProxy();

app.Run();
