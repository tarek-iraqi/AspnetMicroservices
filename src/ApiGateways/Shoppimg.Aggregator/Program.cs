using Shoppimg.Aggregator.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient<CatalogService>(options =>
{
    options.BaseAddress = new Uri(builder.Configuration["ApiSettings:CatalogUrl"]);
});

builder.Services.AddHttpClient<BasketService>(options =>
{
    options.BaseAddress = new Uri(builder.Configuration["ApiSettings:BasketUrl"]);
});

builder.Services.AddHttpClient<OrderService>(options =>
{
    options.BaseAddress = new Uri(builder.Configuration["ApiSettings:OrderingUrl"]);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
