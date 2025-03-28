
using System;
using HotelManagement.Models.Dtos.Identity;
using System.Text;
using HotelManagement.Models.Entities;
using HotelManagement.Repository.Abstraction;
using HotelManagement.Repository.Data;
using HotelManagement.Repository.Implementation;
using HotelManagement.Service.Abstraction;
using HotelManagement.Service.Implementation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace HotelManagementSystem1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "HotelManagement API", Version = "v1" });

                // Add JWT Bearer authentication to Swagger UI
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
            });

            builder.Services.AddEndpointsApiExplorer();

            // Register DbContext
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                   options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
                       sql => sql.MigrationsAssembly("HotelManagement.Repository")));



            // Register Identity
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
           
    
   


            // Register JWT Options from appsettings.json
            builder.Services.Configure<JwtOption>(
                builder.Configuration.GetSection("JwtOption")
            );

            // Read JWT secret and configure authentication
            var jwtSettings = builder.Configuration.GetSection("JwtOption").Get<JwtOption>();
            var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings.Issuer,

                    ValidateAudience = true,
                    ValidAudience = jwtSettings.Audience,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),

                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdminRole", policy =>
                    policy.RequireRole("Admin"));

                options.AddPolicy("RequireManagerRole", policy =>
                    policy.RequireRole("Manager"));

                options.AddPolicy("RequireGuestRole", policy =>
                    policy.RequireRole("Guest"));
            });
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


            // Dependency Injection
            builder.Services.AddScoped<IHotelRepository, HotelRepository>();
            builder.Services.AddScoped<IHotelService, HotelService>();
            builder.Services.AddScoped<IRoomRepository, RoomRepository>();
            builder.Services.AddScoped<IRoomService, RoomService>();
            builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IManagerRepository, ManagerRepository>();
            builder.Services.AddScoped<IManagerService, ManagerService>();
            builder.Services.AddScoped<IGuestService, GuestService>();
            builder.Services.AddScoped<IGuestRepository, GuestRepository>();
            builder.Services.AddScoped<IreservationService, ReservationService>();
            builder.Services.AddScoped<IReservationRepository, ReservationRepository>();







            var app = builder.Build();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();

            app.UseAuthorization();



            app.MapControllers();

            app.Run();
        }
    }
}
