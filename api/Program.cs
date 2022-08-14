using MySql.Data.MySqlClient;
using Microsoft.Extensions.FileProviders;


var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();


String connectionString = builder.Configuration["ConnectionStrings:DefaultConnection"];
MySqlConnection conexao = new MySqlConnection(connectionString);
MySqlCommand comando = new MySqlCommand("SELECT * FROM produto", conexao);
comando.ExecuteReader();

/*
var provider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Public"));
provider.Watch("*.*");

var options = new StaticFileOptions()
{
    RequestPath = "",
    FileProvider = provider
};
app.UseStaticFiles(options);
*/

app.MapGet("/", () => "Carregando produtos...");

app.Run();
