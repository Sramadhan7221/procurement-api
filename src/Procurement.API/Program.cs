using Procurement.Application;
using Procurement.Infrastructure;
using Procurement.Infrastructure.Persistence;
using Procurement.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Procurement API",
        Version = "v1",
        Description = "Procurement System (Pengadaan Barang) REST API"
    });
});

var app = builder.Build();

await SeedData.SeedAsync(app.Services);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.MapControllers();

app.Run();
