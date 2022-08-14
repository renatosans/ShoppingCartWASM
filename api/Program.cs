using MySql.Data.MySqlClient;
using Microsoft.Extensions.FileProviders;


var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();


String connectionString = builder.Configuration["ConnectionStrings:DefaultConnection"];
MySqlConnection conexao = new MySqlConnection(connectionString);
conexao.Open();

MySqlCommand comando = new MySqlCommand("SELECT * FROM produto", conexao);
var reader = comando.ExecuteReader();


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

app.MapGet("/", (HttpRequest req, HttpResponse res) => res.WriteAsync("Carregando produtos..."));

app.Run();
