namespace MyELib.Hosts.Api
{
#pragma warning disable CS1591
    public class Program
    {
        public static void Main(string[] args)
#pragma warning restore CS1591 
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddServices();
            builder.Services.AddRepositories();
            builder.Services.AddMappers();
            builder.Services.AddValidators();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(opt =>
            {
                opt.AddSwaggerDoc();
                opt.AddSwaggerXML();
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}