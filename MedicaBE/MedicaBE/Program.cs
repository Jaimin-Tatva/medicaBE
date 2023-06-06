
using MedicaBE.Entities.Interface;
using MedicaBE.Entities.Models;
using MedicaBE.Repository.Repository;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection(nameof(DatabaseSettings)));
builder.Services.AddSingleton<DatabaseSettings>(sp => sp.GetRequiredService<IOptions<DatabaseSettings>>().Value);
builder.Services.AddSingleton<IMongoClient>(s => new MongoClient(builder.Configuration.GetValue<string>("DatabaseSettings:ConnectionString")));

//builder.Services.AddSingleton<IMongoDatabase>(option =>
//{
//    var settings = builder.Configuration.GetSection("DatabaseSettings").Get<DatabaseSettings>();
//    var client = new MongoClient(settings.ConnectionString);
//    return client.GetDatabase(settings.DatabaseName);
//});

builder.Services.AddScoped<IRetailerRepository, RetailerRepository>();

// Add services to the container.
//mongodb+srv://bhaktiravaltatvasoft:<password>@cluster0.huxpk70.mongodb.net/

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
