using Microsoft.EntityFrameworkCore;
using UrlShortenerAPI.Data;
using UrlShortenerAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure the database context (choose either SQL Server or PostgreSQL)
builder.Services.AddDbContext<URLDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register URLShortenerService and configure HttpClient for API calls
builder.Services.AddHttpClient<URLShortenerService>();
builder.Services.AddScoped<IURLShortenerService, URLShortenerService>();

// Configure AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

// Configure CORS to allow Angular application
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyMethod()
              .AllowAnyHeader());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Enable CORS for the specified policy
app.UseCors("AllowAngularApp");

app.UseAuthorization();
app.MapControllers();

app.Run();
