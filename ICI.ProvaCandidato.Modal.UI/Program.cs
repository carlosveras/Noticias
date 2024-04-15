using ICI.ProvaCandidato.Dados;
using ICI.ProvaCandidato.Negocio;
using ICI.ProvaCandidato.Negocio.Validators;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<IciDbContext>(options => 
                 options.UseSqlServer(builder.Configuration.GetConnectionString("ProvaCandidatoSqlite")));

// Add services to the container.

builder.Services.AddTransient<TagValidator>();
builder.Services.AddScoped<TagService>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

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
        name: "Index",
        pattern: "Tags/Index",
        defaults: new { controller = "Tag", action = "Index" });

    endpoints.MapControllerRoute(
        name: "Cadastro",
        pattern: "Tags/Cadastro",
        defaults: new { controller = "Tag", action = "Cadastro" });

    endpoints.MapControllerRoute(
        name: "TagsEdicao",
        pattern: "Tags/Edicao/{id}",
        defaults: new { controller = "Tag", action = "Cadastro" });

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


//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
