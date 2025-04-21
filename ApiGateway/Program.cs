using Microsoft.OpenApi.Models;
using MMLib.SwaggerForOcelot.DependencyInjection;
using MMLib.SwaggerForOcelot.Middleware;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Load ocelot.json
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

// 1) Ocelot
builder.Services.AddOcelot(builder.Configuration);

// 2) Small dummy swagger gen so DI can build SwaggerGenerator
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Unified API Docs", Version = "v1" });
});

// 3) SwaggerForOcelot
builder.Services.AddSwaggerForOcelot(builder.Configuration);

var app = builder.Build();

// Serve static files (for swagger-ui assets)
app.UseStaticFiles();

// Mount the aggregated UI at /swagger
app.UseSwaggerForOcelotUI(opt =>
{
    opt.PathToSwaggerGenerator = "/swagger/docs";
    // note: no DocumentTitle property here in v8.x
    // you can also set opt.RoutePrefix = "swagger";
});

// Finally the proxy
await app.UseOcelot();

app.Run();
