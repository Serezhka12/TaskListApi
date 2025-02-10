using MongoDB.Driver;
using TaskListApi.Repositories.Interfaces;
using TaskListApi.Repositories.Realisations;
using TaskListApi.Services.Interfaces;
using TaskListApi.Services.Realizations;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var connectionString = configuration["MongoDbSettings:ConnectionString"]
                           ?? throw new ArgumentNullException("ConnectionString not configured");
    return new MongoClient(connectionString);
});


builder.Services.AddScoped<ITaskListRepository, TaskListRepository>();
builder.Services.AddScoped<ITaskListService, TaskListService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();