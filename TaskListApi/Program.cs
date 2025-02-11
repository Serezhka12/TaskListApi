using MongoDB.Driver;
using Mapster;
using TaskListApi.Endpoints;
using TaskListApi.Middleware;
using TaskListApi.Repositories.Implementation;
using TaskListApi.Repositories.Interfaces;
using TaskListApi.Services.Implementation;
using TaskListApi.Services.Interfaces;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var connectionString = configuration["MongoDbSettings:ConnectionString"]
                           ?? throw new ArgumentNullException("ConnectionString not configured");
    return new MongoClient(connectionString);
});

TypeAdapterConfig.GlobalSettings.Scan(typeof(Program).Assembly);

builder.Services.AddScoped<ITaskListRepository, TaskListRepository>();
builder.Services.AddScoped<ITaskListService, TaskListService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapTaskListEndpoints();
app.Run();