
using Backend_Prog4.Config;
using Backend_Prog4.Models;
using Backend_Prog4.Repositories;
using Backend_Prog4.Services;
using Backend_Prog4.Utils.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "pedidos",
        Version = "v1"
    });

    c.AddSecurityDefinition("token", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Token JWT"
    });

    c.OperationFilter<AuthOperationFilter>();
});

// Servicios
builder.Services.AddScoped<CategoriasService>();
builder.Services.AddScoped<TallesService>();
builder.Services.AddScoped<RolesService>();
builder.Services.AddScoped<IAuthService, AuthServices>();
builder.Services.AddScoped<UsuariosService>();
builder.Services.AddScoped<IEncoderServices, EncoderServices>();
builder.Services.AddScoped<MarcaServices>();
builder.Services.AddScoped<PedidoService>();
builder.Services.AddScoped<ProductoServices>();
builder.Services.AddScoped<ProductoTalleServices>();

// Repositorios genéricos
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Base de datos
builder.Services.AddDbContext<AppDbContext>(options =>
{
    string conn = builder.Configuration.GetConnectionString("dbDev")
        ?? string.Empty;

    options.UseSqlServer(conn);
});

// JWT
string secret = builder.Configuration.GetSection("JWT:Secret").Value
    ?? throw new InvalidOperationException("JWT:Secret no configurado");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;

        var key = Encoding.UTF8.GetBytes(secret);

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true
        };
    });

var app = builder.Build();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// CORS
app.UseCors(opt =>
{
    opt.AllowAnyHeader();
    opt.AllowAnyMethod();
    opt.AllowAnyOrigin();
});

// Middleware
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Controllers
app.MapControllers();

app.Run();
