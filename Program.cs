using Backend_Prog4.Config;
using Backend_Prog4.Models;
using Backend_Prog4.Repositories;
using Backend_Prog4.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// inyeccion de dependencia

builder.Services.AddScoped<CategoriasService>();
builder.Services.AddScoped<TallesService>();
builder.Services.AddScoped<IRepository<Categoria>, Repository<Categoria>>();
builder.Services.AddScoped<IRepository<Talle>, Repository<Talle>>();


// conficuracion de db : 

builder.Services.AddDbContext<AppDbContext>(options =>
{
    string conn = builder.Configuration.GetConnectionString("dbDev") ?? string.Empty;
    options.UseSqlServer(conn);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//middleware de cors que me permite enviar peticiones hacia el back

app.UseCors(opt =>
{
    opt.AllowAnyHeader();
    opt.AllowAnyMethod();
    opt.AllowAnyOrigin();
});


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();