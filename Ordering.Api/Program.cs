using EventBus.Messages.Common;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Ordering.Application;
using Ordering.Application.Contracts;
using Ordering.Infraestructure.Persistence;
using Ordering.Infraestructure.Repositories;
using System.Text;

namespace Ordering.Api
{
    public class Program
    {
        

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
           
            builder.Services.AddDbContext<OrderContext>(options =>
              options.UseSqlServer(builder.Configuration.GetConnectionString("OrderingConnectionString")));
            //Repositorio
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            builder.Services.AddApplicationServices();

            //masstransit-rabbitMQ

            builder.Services.AddMassTransit(x =>
            {
                x.AddConsumer<EventBusConsumer.EventBusConsumer>();

                x.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);

                    cfg.ReceiveEndpoint(EventBusConstants.BasketCheckoutQueue, c =>
                    {
                        c.ConfigureConsumer<EventBusConsumer.EventBusConsumer>(ctx);
                    });

                });

            });

            builder.Services.AddMassTransitHostedService();

            builder.Services.AddScoped<EventBusConsumer.EventBusConsumer>();

            builder.Services.AddAutoMapper(typeof(Program));


            var key = Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("Identity:Key"));
           
            ///Autenticacion
            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                    };

                });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}