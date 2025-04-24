using Microsoft.EntityFrameworkCore;
using OrderService_SQL.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. Ajouter DbContext
builder.Services.AddDbContext<OrderDbContext>(opts =>
    opts.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Enregistrer Repository
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v2", new() { Title = "OrderService API", Version = "v2" });
});

var app = builder.Build();

// 3. Appliquer migrations auto
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
    db.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v2/swagger.json", "OrderService v2");
    c.RoutePrefix = string.Empty;
});

app.MapControllers();
app.Run();