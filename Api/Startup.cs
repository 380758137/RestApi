using Api.Core.Interfaces;
using Api.Infrastructure;
using Api.Infrastructure.Repostitories;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using FluentValidation.AspNetCore;
using Api.Resources;
using Api.Resources.Validators;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Diagnostics;
using Serilog;
using Serilog.Events;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Api
{
    public class Startup
    {
        private readonly ILoggerFactory _loggerFactory;

        public Startup(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper>(factory => 
            {
                var actionContext = factory.GetService<IActionContextAccessor>().ActionContext;
                return new UrlHelper(actionContext);
            });
            services.AddScoped<ICityRepository, CityRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddDbContext<MyDbContext>(options => { options.UseInMemoryDatabase("MyDatabase"); });
            services.AddMvc(options=> 
            {
                options.ReturnHttpNotAcceptable = true;
                options.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
                options.InputFormatters.Add(new XmlDataContractSerializerInputFormatter());
            }).AddFluentValidation();
            services.AddAutoMapper();
            services.AddTransient<IValidator<CityAddResource>, CityAddOrUpdateResourceValidator<CityAddResource>>();
            services.AddTransient<IValidator<CityUpdateResource>, CityUpdateResourceValidator>();
            services.AddTransient<IValidator<CountryAddResource>, CountryAddResourceValidator>();
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File(Path.Combine("logs", @"log.txt"), rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseStatusCodePages();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(builder => 
                {
                    builder.Run(async context => 
                    {
                        var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
                        if (exceptionHandlerFeature != null)
                        {
                            var logger = _loggerFactory.CreateLogger("全局异常记录");
                            logger.LogError(500, exceptionHandlerFeature.Error, exceptionHandlerFeature.Error.Message);
                        }
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync(exceptionHandlerFeature?.Error?.Message ?? "服务器端错误");
                    });
                });
            }

            app.UseMvc();
        }
    }
}
