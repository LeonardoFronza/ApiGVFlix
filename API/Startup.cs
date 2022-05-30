using API.AutoMapper;
using API.Consultas;
using API.Consultas.IConsultas;
using API.Context;
using API.Ferramentas;
using API.Repositorios;
using API.Repositorios.IRepositorios;
using API.Servicos;
using API.Servicos.IServicos;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace API
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
            // ConnectionString.
            var connectionString = Configuration.GetConnectionString("GVFlixConnectionString");
            services.AddDbContext<Contexto>(options => options.UseSqlServer(connectionString));

            // Cors.
            services.AddCors();

            // Injeção de Dependências.
            services.AddScoped<IConsultaDeFilme, ConsultaDeFilme>();
            services.AddScoped<IConsultaDeAtor, ConsultaDeAtor>();
            services.AddScoped<IConsultaDeFilmeAtor, ConsultaDeFilmeAtor>();
            services.AddScoped<IConsultaDeUsuario, ConsultaDeUsuario>();
            services.AddScoped<IConsultaDeHistorico, ConsultaDeHistorico>();

            services.AddScoped<IRepositorioDeFilme, RepositorioDeFilme>();
            services.AddScoped<IRepositorioDeAtor, RepositorioDeAtor>();
            services.AddScoped<IRepositorioDeFilmeAtor, RepositorioDeFilmeAtor>();
            services.AddScoped<IRepositorioDeUsuario, RepositorioDeUsuario>();
            services.AddScoped<IRepositorioDeHistorico, RepositorioDeHistorico>();

            services.AddScoped<IServicoDeFilme, ServicoDeFilme>();
            services.AddScoped<IServicoDeAtor, ServicoDeAtor>();
            services.AddScoped<IServicoDeFilmeAtor, ServicoDeFilmeAtor>();
            services.AddScoped<IServicoDeUsuario, ServicoDeUsuario>();
            services.AddScoped<IServicoDeHistorico, ServicoDeHistorico>();

            services.AddScoped<IConversor, Conversor>();

            // Adicionando controllers.
            services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            // Adicionando AutoMapper.
            /*Poderia ser [services.AddAutoMapper()], mas para realizar a injeção de dependência ferei do modo abaixo*/
            services.AddScoped(provider => new MapperConfiguration(cfg => 
            { 
                // Adicionando a injeção do IConversor.
                cfg.AddProfile(new MapeadorDeEntidade(provider.GetService<IConversor>()));
            }).CreateMapper());

            // Adicionando Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", 
                    new OpenApiInfo 
                    { 
                        Title = "GVFlix",
                        Version = "v1",
                        Description = "Treinamento GVtalentos."
                    });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                
                // Swagger
                app.UseSwagger();
                app.UseSwaggerUI(opt =>
                {
                    opt.SwaggerEndpoint("/swagger/v1/swagger.json", "GVFlix V1");
                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            // Liberando Cors para todas as origens, métodos e cabeçalhos.
            app.UseCors(Configure => Configure.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                // Definindo padrão das requisições (controllers);
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");

                endpoints.MapControllers();
            });

            // Aplicando migrações pendentes no banco de dados.
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<Contexto>();
                context.Database.Migrate();
            }
        }
    }
}
