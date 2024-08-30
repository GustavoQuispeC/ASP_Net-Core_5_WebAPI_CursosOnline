using Aplicacion.Contratos;
using Aplicacion.Cursos;
using Dominio;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Persistencia;
using Seguridad.TokenSeguridad;
using WebAPI.Middleware;

namespace WebAPI
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
            // Se agrega el contexto de la base de datos
            services.AddDbContext<CursosOnlineContext>(options =>
        options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // Se agrega el servicio de MediatR para la implementación del patrón CQRS
            services.AddMediatR(typeof(Consulta.Manejador).Assembly);


            //agregamos fluent validation para las validaciones
            //services.AddControllers();

            services.AddControllers(opt =>
            {
                // Se agrega la política de autorización por defecto para todos los endpoints
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                opt.Filters.Add(new AuthorizeFilter(policy));
            })
                // Se agrega el uso de FluentValidation para las validaciones de los dtos
                .AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssemblyContaining<Nuevo>());
           
            // Se agrega el servicio de Identity
            var builder = services.AddIdentityCore<Usuario>();
            var identityBuilder = new IdentityBuilder(builder.UserType, builder.Services);

            //habilitamos la autenticacion del usuario por medio de un token
            identityBuilder.AddEntityFrameworkStores<CursosOnlineContext>();

            //gestiona las operaciones de inicio de sesión
            identityBuilder.AddSignInManager<SignInManager<Usuario>>();
            //gestiona las operaciones de usuario
            services.TryAddSingleton<ISystemClock, SystemClock>();

            // Se agrega la configuración jwt para la autenticación
            services.AddScoped<IJwtGenerador, JwtGenerador>();

            // Se agrega la configuración de autenticación con jwt
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    //validamos el emisor del token
                    ValidateIssuerSigningKey = true,
                    //validamos la clave del token
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes("Mi palabra secreta")),

                    //no se enviara el token por https
                    ValidateIssuer = false,
                    //token global para la autenticacion
                    ValidateAudience = false
                };
            });

            // usamos swagger para documentar la API
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPI", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Se agrega el middleware de excepciones personalizado para capturar los errores de la aplicación
            app.UseMiddleware<ManejadorErrorMiddleware>();

            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();


                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPI v1"));

            }

           //pp.UseHttpsRedirection();
            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
