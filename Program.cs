
using ELib_IDSFintech_Internship.Data;
using Microsoft.EntityFrameworkCore;

namespace ELib_IDSFintech_Internship
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<ELibContext>(options =>
                options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
                    new MySqlServerVersion(new Version(8, 0, 31))));

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
            });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Logging.AddConsole();
            builder.Logging.AddDebug();

            /*builder.Services.AddScoped<StudentService>();
            builder.Services.AddScoped<LocationService>();*/

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.CreateDbIfNotExists();


            app.MapControllers();

            // Copied from one of my projects, to allow requests from front end website(s)
            app.UseCors(options => options
            .WithOrigins("http://localhost:3000", "http://localhost:3001")
            .AllowAnyMethod()
            .AllowAnyHeader());

            // Default message to show on default / page
            app.MapGet("/", () => @"Tasks management API. Navigate to /swagger to open the Swagger test UI.");

            app.Run();
        }
    }
}
