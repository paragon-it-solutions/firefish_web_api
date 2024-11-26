using Firefish.Core.Contracts.Repositories;
using Firefish.Core.Contracts.Services;
using Firefish.Core.Entities;
using Firefish.Infrastructure.Repositories;
using Firefish.Infrastructure.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Configure controllers and endpoint routing.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configure Repositories
builder.Services.AddScoped<ICandidateRepository, CandidateRepository>();

// Configure Services
builder.Services.AddScoped<ICandidateService, CandidateService>();

// CORS configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowAnyOrigin",
        b => b.AllowAnyOrigin()
                                        .AllowAnyMethod()
                                        .AllowAnyHeader()
        );
});

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors("AllowAnyOrigin");
app.UseRouting();
app.MapControllers();

app.Run();
