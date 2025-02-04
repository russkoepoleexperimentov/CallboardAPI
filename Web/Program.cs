using Application.DTOs;
using Application.Mappers;
using Application.Services;
using Application.Validators;
using Common;
using Core.Repositiories;
using FluentValidation;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using Web.Middlewares;

namespace Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddLogging(logging => logging.AddConsole());

            builder.Services
                .AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        IssuerSigningKey = AuthConfig.GetSymmetricSecurityKey(),
                        ValidateIssuerSigningKey = true,
                    };
                });

            builder.Services.AddAuthorization();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(config =>
            {
                config.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                config.OperationFilter<SwaggerAuthorizeFilter>();
            });

            builder.Services.AddDbContext<ApplicationContext>(
                    options => options
                    .UseLazyLoadingProxies()
                    .UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"),
                    builder => builder.MigrationsAssembly("Web")
                ));

            builder.Services
                .AddScoped<IUserRepository, UserRepository>()
                .AddTransient<UserService>()
                .AddScoped<IValidator<UserRegistrationDto>, UserRegistrationValidator>()
                .AddScoped<IValidator<UserUpdateDto>, UserUpdateValidator>()
                .AddAutoMapper(typeof(UserMapper))
                
                .AddScoped<ICategoryRepository, CategoryRepository>()
                .AddTransient<CategoryService>()
                .AddAutoMapper(typeof(CategoryMapper))
                
                .AddScoped<ICategoryParameterRepository, CategoryParameterRepository>()
                .AddTransient<CategoryParameterService>()
                .AddAutoMapper(typeof(CategoryParameterMapper))
                
                .AddScoped<IImageRepository, ImageRepository>()
                .AddTransient<ImageService>()
                
                .AddScoped<IAdvertisementRepository, AdvertisementRepository>()
                .AddTransient<AdvertisementService>()
                .AddScoped<IValidator<AdvertisementCreateDto>, AdvertisementCreateValidator>()
                .AddScoped<IValidator<AdvertisementUpdateDto>, AdvertisementUpdateValidator>()
                .AddAutoMapper(typeof(AdvertisementMapper))

                .AddScoped<IAdvertisementImageRepository, AdvertisementImageRepository>()
                
                .AddScoped<IAdvertisementParameterValueRepository, AdvertisementParameterValueRepository>()
                .AddTransient<AdvertisementParameterValueService>()
                .AddAutoMapper(typeof(AdvertisementParameterValueMapper))
                
                .AddScoped<ICommentRepository, CommentRepository>()
                .AddTransient<CommentService>()
                .AddAutoMapper(typeof(CommentMapper));


            builder.Services
                .AddSingleton(new JWTService(AuthConfig.GetSymmetricSecurityKey(), AuthConfig.LIFETIME_MINUTES));

            ImageService.DirectoryPath = Environment.ProcessPath + "UserContent";

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var context = services.GetRequiredService<ApplicationContext>();
                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                }
            }

            //if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<ExceptionCatchMiddleware>();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
