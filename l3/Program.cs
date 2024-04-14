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
            // �����, �� ������� �������� ������� ��� �������� ������
            ValidateIssuer = true,
            // �����, �� ����������� �������
            ValidIssuer = AuthOptions.ISSUER,

            // �� ������� �������� ���������� ������
            ValidateAudience = true,
            // ������������ ���������� ������
            ValidAudience = AuthOptions.AUDIENCE,
            // �� ������� �������� ���� ���������
            ValidateLifetime = true,

            // ������������ ����� �������
            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
            // �������� ����� �������
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
