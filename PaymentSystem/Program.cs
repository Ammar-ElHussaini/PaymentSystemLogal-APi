using Data_Access_Layer.ProjectRoot.Core.Interfaces;
using Data_Access_Layer.ProjectRoot.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PaymentSystem.Data_Acess_Layer.Models;
using PaymentSystem.Services.Imp;
using PaymentSystem.Services.Interfaces;
using PaymentSystem.Services.Interfaces.ITransferMangent;
using System.Text;
using Telegram.Bot;

namespace PaymentSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var appSettingsSection = builder.Configuration.GetSection("AppSettings");
            var appSettings = appSettingsSection.Get<AppSettings>();
            builder.Services.Configure<AppSettings>(appSettingsSection);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = appSettings.Jwt.Issuer,
                    ValidAudience = appSettings.Jwt.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.Jwt.Key))
                };
            });

            builder.Services.AddAuthorization();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(appSettings.ConnectionStrings.DefaultConnection));

            builder.Services.AddScoped<ApplicationDbContext>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddScoped<IJwtService, JwtService>();
            builder.Services.AddScoped<IUser,UserService>();
            builder.Services.AddScoped<AppSettings>();
            builder.Services.AddScoped<ITransferService, TransferService>();
            builder.Services.AddScoped<ITelegramBotService,TelegramBotService>();

            builder.Services.AddSingleton<TelegramBotClient>(provider =>
                new TelegramBotClient(appSettings.TelegramSettings.BotToken)); 

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", policy =>
                {
                    policy.WithOrigins(appSettings.AllowedOrigins.ToArray())
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            var botClient = new TelegramBotClient(appSettings.TelegramSettings.BotToken);
             botClient.SetWebhook(appSettings.WebhookSettings.WebhookUrl);

            var app = builder.Build();

        

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors("CorsPolicy");

            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
