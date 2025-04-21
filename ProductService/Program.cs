using Cassandra;
using ProductService.Models; // pour ton modèle
using ProductService.Data;   // pour ton repository (à créer après)

var builder = WebApplication.CreateBuilder(args);

// CASSANDRA SETUP
var cassandraConfig = builder.Configuration.GetSection("Cassandra");

var contactPoint = cassandraConfig["ContactPoints"] ?? "localhost";
var port = int.Parse(cassandraConfig["Port"] ?? "9042");
var keyspace = cassandraConfig["Keyspace"] ?? "product_ks";


var cluster = Cluster.Builder()
    .AddContactPoint(contactPoint)
    .WithPort(port)
    .Build();

var session = cluster.Connect();
session.Execute($@"
    CREATE KEYSPACE IF NOT EXISTS {keyspace}
    WITH REPLICATION = {{ 'class' : 'SimpleStrategy', 'replication_factor' : 1 }};");
session.ChangeKeyspace(keyspace);

builder.Services.AddSingleton<Cassandra.ISession>(session);
builder.Services.AddSingleton<ProductRepository>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();                                   // :contentReference[oaicite:0]{index=0}
builder.Services.AddControllers();

var app = builder.Build();
// Enable middleware to serve generated Swagger as JSON and the UI
app.UseSwagger();                                                  // serve /swagger/v1/swagger.json
app.UseStaticFiles();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProductService v1");
    c.RoutePrefix = string.Empty;  // serve UI at root: http://<host>/
});

app.MapControllers();
app.Run();