
using CityPulse.Helpers;
using Library.Models;
using Library.Repository;
using Library.ViewModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

namespace Library
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers()
             .AddJsonOptions(options =>
             {
                 // تفعيل الحفاظ على المراجع لمنع الحلقات في JSON
                 options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
             });

            // Add services to the container.
            builder.Services.AddDbContext<AppDbContext>(OptionsBuilder =>
            {
                OptionsBuilder.UseSqlServer("Server=LAPTOP-R0QOES6Q\\SQLEXPRESS01;Database=LibraryDB;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true");

            });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<AuthServiceModel>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IUserRep, UserRep>();
            builder.Services.AddScoped<IBookRep, BookRep>();   
            builder.Services.AddScoped<IAdminRep, AdminRep>();



            // إعداد JWT Settings
            builder.Services.Configure<JWT>(builder.Configuration.GetSection("JwtSettings"));
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JWT>();
                var key = Encoding.UTF8.GetBytes(jwtSettings.Secret);

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });



            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                    policy =>
                    {
                        policy.AllowAnyOrigin()
                              .AllowAnyMethod()
                              .AllowAnyHeader();
                    });
            });



            

            var app = builder.Build();



            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.WebRootPath, "books")),
                RequestPath = "/books"
            });

            app.UseCors(MyAllowSpecificOrigins);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
