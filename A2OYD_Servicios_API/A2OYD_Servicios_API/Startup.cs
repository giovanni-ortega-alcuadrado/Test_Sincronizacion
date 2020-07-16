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
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using A2OYD_Servicios_API.ContextosDB;
using Microsoft.EntityFrameworkCore;
using System.Text;
using AutoMapper;
using A2Utilidades.Web.API.Generico.Utilidades;
using A2OYD_Servicios_API.Utilidades;
using System.Reflection;
using System.IO;
using Newtonsoft.Json;
using System.Buffers;

namespace A2OYD_Servicios_API
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
            services.AddMvcCore(options => options.OutputFormatters.Add(new Codificador (new JsonSerializerSettings(), ArrayPool<char>.Shared, Configuration["ConfiguracionParametros:Codificacion"])))
            .AddJsonFormatters();
            services.AddAutoMapper();
            services.AddIdentity<A2OYD_Servicios_API.Models.Seguridad.ApplicationUser, IdentityRole>()
                    .AddEntityFrameworkStores<ContextoDbUtil>()
                    .AddDefaultTokenProviders();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddTransient<UtilidadesGenericas>();
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "A2 ServicioAPI", Version = "v1" });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
             options.TokenValidationParameters = new TokenValidationParameters
             {
                 ValidateIssuer = false,
                 ValidateAudience = false,
                 ValidateLifetime = true,
                 ValidateIssuerSigningKey = true,
                 IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(Configuration["ConfiguracionParametros:JWT:KEY"])),
                 ClockSkew = TimeSpan.Zero
             });
            services.AddDbContext<ContextoDbOyd>(opciones => opciones.UseSqlServer(@A2Utilidades.Cifrar.descifrar(Configuration.GetConnectionString("dbOYDConnectionString"))));
            services.Configure<string>(opciones => opciones.ToString());
            //services.AddDbContext<ContextoDbOyd>(opciones => opciones.UseSqlServer(Configuration.GetConnectionString("dbOYDConnectionString"))); //.GetConnectionString("conexion")));
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
                app.UseHsts();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                if (!String.IsNullOrEmpty(Configuration["ConfiguracionParametros:DirectorioVirtual"]))
                {
                    c.SwaggerEndpoint("/" + Configuration["ConfiguracionParametros:DirectorioVirtual"] + "/swagger/v1/swagger.json", "A2 OYD ServicioAPI");
                }
                else
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "A2 OYD ServicioAPI");
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
