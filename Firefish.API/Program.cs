using Firefish.Core.Contracts.Repositories;
using Firefish.Core.Contracts.Services;
using Firefish.Infrastructure.Repositories;
using Firefish.Infrastructure.Services;
using Microsoft.OpenApi.Models;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

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
    options.AddPolicy("AllowAnyOrigin", b => b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Firefish API Task", Version = "v1" });
});

WebApplication app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAnyOrigin");
app.UseRouting();
app.MapControllers();

app.Run();
