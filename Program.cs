var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var PetsDB = new PetsDB();

app.MapGet("/", () => "Hello World!");

app.Run();
