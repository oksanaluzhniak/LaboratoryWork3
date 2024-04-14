using l3;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // вказує, чи потрібна перевірка видавця при перевірці токена
            ValidateIssuer = true,
            // рядок, що представляє видавця
            ValidIssuer = AuthOptions.ISSUER,

            // чи потрібна перевірка отримувача токена
            ValidateAudience = true,
            // встановлення отримувача токена
            ValidAudience = AuthOptions.AUDIENCE,
            // чи потрібна перевірка часу існування
            ValidateLifetime = true,

            // встановлення ключа безпеки
            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
            // перевірка ключа безпеки
            ValidateIssuerSigningKey = true,
        };
    });

builder.Services.AddSingleton<People>();
builder.Services.AddSingleton<Users>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
