
using ELib_IDSFintech_Internship.Data;
using ELib_IDSFintech_Internship.Services.Books;
using ELib_IDSFintech_Internship.Services.Common;
using ELib_IDSFintech_Internship.Services.Tools;
using ELib_IDSFintech_Internship.Services.Users;
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

            //registering the MemoryCaching to the services DI container
            builder.Services.AddMemoryCache();

            //building services
            builder.Services.AddScoped<BookAuthorService>();
            builder.Services.AddScoped<BookGenreService>();
            builder.Services.AddScoped<BookLocationService>();
            builder.Services.AddScoped<BookService>();
            builder.Services.AddScoped<BookTagService>();
            builder.Services.AddScoped<CreditCardService>();
            builder.Services.AddScoped<SubscriptionService>();
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<BookFormatService>();
            builder.Services.AddScoped<LanguageService>(); 

            builder.Services.AddSingleton<AES256Encryption>();
            builder.Services.AddSingleton<SessionManagementService>();

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
