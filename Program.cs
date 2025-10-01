using Microsoft.AspNetCore.Mvc;
using PetsOwnersApiSql;
// Käyttäen Microsoft.AspNetCore.Mvc oli ainoa keino miten sain toimimaan.

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var PetsDB = new PetsDB();

app.MapGet("/", () => "Hello World!");
//Ilman [FromBody] tuli erroria. 
app.MapPost("/Owners", ([FromBody] Owner owner) =>
{
    PetsDB.AddOwner(owner.Name, owner.PhoneNumber);
    return Results.Ok();
});
app.MapPost("/Pets", ([FromBody] Pet pet) =>
{
    PetsDB.AddPet(pet.Name, pet.OwnerId);
    return Results.Ok();
});
app.MapGet("/Pets", (string petName) =>
{
    var phoneNumber = PetsDB.SearchOwnersPhoneNumberByPet(petName);
    if (phoneNumber != null)
    {
        return Results.Ok(phoneNumber);
    }
    else
    {
        return Results.NotFound("Owner not found");
    }
});

app.Run();
