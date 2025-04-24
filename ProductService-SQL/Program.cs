using Microsoft.EntityFrameworkCore;
using ProductService_SQL.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. Ajouter DbContext PostgreSQL
builder.Services.AddDbContext<ProductDbContext>(opts =>
    opts.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Enregistrer Repository
builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v2", new()
    {
        Title = "ProductService API",
        Version = "v2"
    });
});

var app = builder.Build();

// 3. Appliquer automatiquement les migrations
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ProductDbContext>();
    db.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v2/swagger.json", "ProductService v2");
    c.RoutePrefix = string.Empty;
});

app.MapControllers();
app.Run();
