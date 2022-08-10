using Microsoft.Extensions.FileProviders;


var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();


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
