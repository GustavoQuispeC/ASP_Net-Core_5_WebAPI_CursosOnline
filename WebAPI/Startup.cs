using Aplicacion.Contratos;
using Aplicacion.Cursos;
using Dominio;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Persistencia;
using Persistencia.DapperConexion;
using Persistencia.DapperConexion.Instructor;
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

            // usando Dapper para la conexión a la base de datos
            services.AddOptions();
            services.Configure<ConexionConfiguracion>(Configuration.GetSection("ConnectionStrings"));


            


            // Se agrega el servicio de MediatR para la implementación del patrón CQRS
            services.AddMediatR(typeof(Consulta.Manejador).Assembly);


            //agregamos fluent validation para las validaciones
            //services.AddControllers();

            services.AddControllers().AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssemblyContaining<Nuevo>());
           
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

            // Se agrega el servicio de autenticación para obtener el usuario actual
            services.AddScoped<IUsuarioSesion, UsuarioSesion>();

           
            // se agrega el servicio de automapper
            services.AddAutoMapper(typeof(Consulta.Manejador));



            // usamos swagger para documentar la API
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Service Mantenimiento de cursos", Version = "v1" });
                c.CustomSchemaIds(x => x.FullName);
            });

            //implementamos servicio de conexion a la base de datos con Dapper
            services.AddTransient<IFactoryConnection, FactoryConnection>();

            //implementamos el servicio de instructor
            services.AddScoped<IInstructor, InstructorRepositorio>();
           
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
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Service Mantenimiento de cursos v1"));
               

            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
