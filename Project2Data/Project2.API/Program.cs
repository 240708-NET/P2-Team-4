using Microsoft.EntityFrameworkCore;
using Project2.Data;

namespace Project2.API {
    public class Program {
        public static void Main(string[] args) {
            //  Create API services
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<IData>(pData => new DataHandler(File.ReadAllText("../Project2.Data/ConnectionString")));

            // Build API
            var app = builder.Build();

            if (app.Environment.IsDevelopment()) {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.UseHttpsRedirection();
            app.MapControllers();

            app.UseCors(options => options.AllowAnyHeader()
                .WithOrigins("http://localhost:3000")
                .AllowAnyMethod()
                .AllowCredentials()
                );

            //  Run API
            app.Run();
        }
    }
}