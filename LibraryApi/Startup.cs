using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using LibraryApi.Services;
using LibraryApi.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.IO;
using LibraryApi.Mappers;
using AutoMapper;
using System.Text.Json.Serialization;
using RabbitMqUtils;

namespace LibraryApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddRabbit(Configuration);
            services.AddControllers()
                .AddJsonOptions(options =>
                    {
                        // think more about this before you put this in production...
                        options.JsonSerializerOptions.Converters.Add(new StringToIntConverter());
                        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    }
                );
            services.AddTransient<IGenerateEnrollmentIds, EnrollmentIdGenerator>();

            services.AddDbContext<LibraryDataContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("LibraryDatabase"))
            );

            services.AddScoped<IMapBooks, EfBookMapper>();
            services.AddScoped<ISendMessagesToTheReservationProcessor, RabbitMqReservationProcessor>();
            services.AddAutoMapper(typeof(Startup));

            services.AddSwaggerGen(c =>
            {
                
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Library Api",
                    Version = "1.0",
                    Contact = new OpenApiContact
                    {
                        Name="Jeff Gonzalez",
                        Email = "jeff@hypertheory.com"
                    },
                    Description = @"This is the API for our library. Make sure you pay your late fees!"

                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = Configuration.GetValue<string>("redisHost");
            });

            services.AddCors();
            services.AddResponseCaching();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(options =>
            {
                options.AllowAnyOrigin();
                options.AllowAnyMethod();
                options.AllowAnyHeader();
            });
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
               
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Library API");
                c.RoutePrefix = string.Empty;
            });

          //  app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseResponseCaching(); // probably think about this long and hard before you do it.
          
        }
    }
}
