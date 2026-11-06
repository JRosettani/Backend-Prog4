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

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
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

// Repositorios
builder.Services.AddScoped<IRepository<Categoria>, Repository<Categoria>>();
builder.Services.AddScoped<IRepository<Talle>, Repository<Talle>>();
builder.Services.AddScoped<IRepository<Usuario>, Repository<Usuario>>();
builder.Services.AddScoped<IRepository<Rol>, Repository<Rol>>();
builder.Services.AddScoped<IRepository<Marca>, Repository<Marca>>();
builder.Services.AddScoped<IRepository<Pedido>, Repository<Pedido>>();
builder.Services.AddScoped<IRepository<EstadoPedido>, Repository<EstadoPedido>>();
builder.Services.AddScoped<IRepository<ProductoTalle>, Repository<ProductoTalle>>();
builder.Services.AddScoped<IRepository<PedidoItem>, Repository<PedidoItem>>();

// DB
builder.Services.AddDbContext<AppDbContext>(options =>
{
    string conn = builder.Configuration.GetConnectionString("dbDev") ?? string.Empty;
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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(opt =>
{
    opt.AllowAnyHeader();
    opt.AllowAnyMethod();
    opt.AllowAnyOrigin();
});

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();