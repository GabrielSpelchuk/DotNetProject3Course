using Microsoft.AspNetCore.Mvc;
using Project.DAL.Repositories.Interfaces;
using Project.DAL.Repositories;
using Project.BLL.Mapping;
using Project.BLL.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);


var configuration = builder.Configuration;
var conn = configuration.GetConnectionString("Default");


builder.Services.AddSingleton<IDbConnectionFactory>(sp => new NpgsqlConnectionFactory(conn));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(sp => new UnitOfWork(sp.GetRequiredService<IDbConnectionFactory>()));

builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddScoped<IOrderService, Project.BLL.Services.OrderService>();

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var problem = new ValidationProblemDetails(context.ModelState);
            return new BadRequestObjectResult(problem);
        };
    })
    .AddNewtonsoftJson();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


app.UseExceptionHandler(errApp =>
{
    errApp.Run(async context =>
    {
        context.Response.ContentType = "application/problem+json";
        var exFeature = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();
        var ex = exFeature?.Error;

        var pd = new ProblemDetails
        {
            Title = ex?.GetType().Name ?? "Error",
            Detail = ex?.Message,
            Status = 500,
            Instance = context.Request.Path
        };

        if (ex is KeyNotFoundException) pd.Status = 404;
        else if (ex is ArgumentException) pd.Status = 400;

        context.Response.StatusCode = pd.Status.Value;
        await System.Text.Json.JsonSerializer.SerializeAsync(context.Response.Body, pd, pd.GetType(), new System.Text.Json.JsonSerializerOptions { PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase });
    });
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();
