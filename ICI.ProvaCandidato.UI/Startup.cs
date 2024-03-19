using ICI.ProvaCandidato.Dados;
using ICI.ProvaCandidato.Negocio;
using ICI.ProvaCandidato.Negocio.Validators;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ICI.ProvaCandidato.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddTransient<NoticiaValidator>();
            services.AddTransient<TagValidator>();
            services.AddTransient<UsuarioValidator>();

            services.AddScoped<UsuarioService>();
            services.AddScoped<TagService>();
            services.AddScoped<NoticiaService>();

            services.AddDbContext<IciDbContext>(options =>
                 options.UseSqlServer(Configuration.GetConnectionString("ProvaCandidatoSqlite")));
           
        }
   
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthorization();
                       
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "Cadastro",
                    pattern: "Tags/Cadastro",
                    defaults: new { controller = "Home", action = "Cadastro" });

                endpoints.MapControllerRoute(
                    name: "TagsEdicao",
                    pattern: "Tags/Edicao/{id}",
                    defaults: new { controller = "Home", action = "Cadastro" });

                endpoints.MapControllerRoute(
                    name: "CriarNoticia",
                    pattern: "Noticias/Criar",
                    defaults: new { controller = "Noticias", action = "Criar" });

                endpoints.MapControllerRoute(
                    name: "CriarNoticia",
                    pattern: "Noticias/Criar",
                    defaults: new { controller = "Noticias", action = "Criar" });

                endpoints.MapControllerRoute(
                    name: "EditarNoticia",
                    pattern: "Noticias/Criar/{id}",
                    defaults: new { controller = "Noticias", action = "Editar" });

                endpoints.MapControllerRoute(
                    name: "CadastroUsuario",
                    pattern: "Usuarios/Criar",
                    defaults: new { controller = "Usuarios", action = "Criar" });

                endpoints.MapControllerRoute(
                    name: "EditarUsuario",
                    pattern: "Usuarios/Criar/{id}",
                    defaults: new { controller = "Usuarios", action = "Criar" });

                endpoints.MapControllerRoute(
                    name: "ConfirmDeleteUsuario",
                    pattern: "Usuarios/ConfirmDelete/{id}",
                    defaults: new { controller = "Usuarios", action = "ConfirmDelete" });
            });

        }
    }
}
