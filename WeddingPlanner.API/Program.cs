using Microsoft.EntityFrameworkCore;
using WeddingPlanner.API.GraphQL;
using WeddingPlanner.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContextFactory<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddFiltering()
    .AddSorting()
    .RegisterDbContextFactory<AppDbContext>();

// Auth0 autentifikacija
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority =
            $"https://{builder.Configuration["Auth0:Domain"]}/";

        options.Audience =
            builder.Configuration["Auth0:Audience"];
    });

// Autorizacijska pravila - određuju koje dozvole korisnik mora imati
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ReadPartners", policy =>
        policy.RequireAssertion(context =>
            context.User.HasClaim(c =>
                c.Type == "scope" &&
                c.Value.Split(' ').Contains("read:partners"))));

    options.AddPolicy("WritePartners", policy =>
        policy.RequireAssertion(context =>
            context.User.HasClaim(c =>
                c.Type == "scope" &&
                c.Value.Split(' ').Contains("write:partners"))));
});
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGraphQL();

app.Run();