using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Project.Mongo.API.Middleware;
using Project.Mongo.Application.Behaviors;
using Project.Mongo.Application.Commands.CreateReview;
using Project.Mongo.Infrastructure.Mongo;

var builder = WebApplication.CreateBuilder(args);

// config
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDb"));

// infra
builder.Services.AddSingleton<MongoContext>();
builder.Services.AddScoped<ReviewRepository>();

// MediatR + Validators + pipeline
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateReviewCommand>());
builder.Services.AddValidatorsFromAssemblyContaining<Project.Mongo.Application.Validators.CreateReviewValidator>();
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

// controllers, swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Mongo Reviews API", Version = "v1" });
});

var app = builder.Build();

// middleware
app.UseMiddleware<GlobalExceptionMiddleware>();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
