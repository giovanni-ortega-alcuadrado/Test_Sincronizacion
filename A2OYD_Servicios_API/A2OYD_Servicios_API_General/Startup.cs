using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using A2OYD_Servicios_API_General.ContextosDB;
using A2OYD_Servicios_API_General.Entidades.Generales;
using A2OYD_Servicios_API_General.Models.Generales;
using A2Utilidades.Web.API.Generico.Utilidades;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;

[assembly: ApiConventionType(typeof(DefaultApiConventions))]

namespace A2OYD_Servicios_API_General
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
            services.AddAutoMapper(typeof(Startup));

            services.AddMvc(congif =>
                {
                    congif.Conventions.Add(new ApiExplorerGroupPerVersionConvention());
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddTransient<UtilidadesGenericas>();
            services.AddTransient<GeneralesReglasNegocio>();

            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = Configuration["ConfiguracionParametros:RutaIdentityServer"];
                    options.RequireHttpsMetadata = false;
                    
                    options.ApiName = Configuration["ConfiguracionParametros:RecursoAplicacion"];
                    options.JwtBackChannelHandler = new HttpClientHandler()
                    {
                        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
                        Proxy = new WebProxy(Configuration["System:Proxy"])
                    };
                });

            services.AddDbContext<ContextoDbOyd>(opciones => opciones.UseSqlServer(A2Utilidades.Cifrar.descifrar(Configuration.GetConnectionString("dbOYDConnectionString"))));
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "A2 WebAPI Generales", Version = "v1", Description = "Web api creada para consultar la información general del aplicativo OYD." });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                string directorioVirtual = Configuration["ConfiguracionParametros:DirectorioVirtual"];
                if (!String.IsNullOrEmpty(directorioVirtual))
                {
                    c.SwaggerEndpoint("/" + directorioVirtual + "/swagger/v1/swagger.json", "A2 OYD ServicioAPI Generales V1");
                }
                else
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "A2 OYD ServicioAPI Generales V1");
                }
            });

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc();
        }
 
        public class ApiExplorerGroupPerVersionConvention : IControllerModelConvention
        {
            public void Apply(ControllerModel controller)
            {
                // Ejemplo: "Controllers.V1"
                var controllerNamespace = controller.ControllerType.Namespace;
                var apiVersion = controllerNamespace.Split('.').Last().ToLower();
                controller.ApiExplorer.GroupName = apiVersion;
            }
        }
    }
}
