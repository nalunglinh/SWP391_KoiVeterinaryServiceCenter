using KoiServiceVetBooking.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace BookingVetKoi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Cấu hình DbContext
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Thêm dịch vụ cho Controllers
            builder.Services.AddControllers();

            // Cấu hình Authentication
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.LoginPath = "/api/account/login";
                options.LogoutPath = "/api/account/logout";
                options.AccessDeniedPath = "/api/account/accessDenied";
            });

            // Cấu hình Swagger/OpenAPI
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Koi Veterinary Service API", Version = "v1" });
            });

            // Thêm dịch vụ CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    policy => policy.AllowAnyOrigin()
                                    .AllowAnyMethod()
                                    .AllowAnyHeader());
            });

            // Xây dựng ứng dụng
            var app = builder.Build();

            // Cấu hình HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
                    c.RoutePrefix = string.Empty;
                });
            }

            // Sử dụng CORS
            app.UseCors("AllowAllOrigins");

            // Kích hoạt xác thực và ủy quyền
            app.UseAuthentication();
            app.UseAuthorization();

            // Map các Controllers
            app.MapControllers();

            // Chạy ứng dụng
            app.Run();
        }
    }
}
