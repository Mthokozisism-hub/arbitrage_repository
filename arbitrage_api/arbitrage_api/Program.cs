using arbitrage_api.Services.CryptoServices;
using arbitrage_api.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Bind arbitrageSettings to the ArbitrageSettings class
builder.Services.Configure<ArbitrageSettings>(builder.Configuration.GetSection("arbitrageSettings"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//register service
builder.Services.AddHttpClient<ICryptoService,CryptoService>();

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
