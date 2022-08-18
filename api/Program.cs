
var builder = WebApplication.CreateBuilder(args);


// Inject Connection String and Create EFCore DB Context 
builder.Services.AddDbContext<CommerceDB>(options => {
    String connectionString = builder.Configuration["ConnectionStrings:Default"];
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});


// Inject Swagger Services 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => { });


// builder.Services.AddSingleton<TokenService>(new TokenService());
// builder.Services.AddSingleton<IUserRepositoryService>(new UserRepositoryService());
// builder.Services.AddAuthorization();
// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt => { });


await using var app = builder.Build();
// app.UseAuthentication();
// app.UseAuthorization();


//Use Swagger in application. 
app.UseSwagger();
app.UseSwaggerUI();


// Sample Endpoint 
app.MapGet("/", () => "Hello! This is .NET 6 Minimal API.   /swagger to get Endpoints doc.").ExcludeFromDescription();


// Get all Products from database
app.MapGet("/products", async (CommerceDB db) => await db.Products.ToListAsync())
.Produces<List<Product>>(StatusCodes.Status200OK)
.WithName("GetAllProducts").WithTags("Getters");


// Get Products by id from database
app.MapGet("/products/{id}", async (CommerceDB db, int id) =>
   await db.Products.SingleOrDefaultAsync(s => s.id == id) is Product prod ? Results.Ok(prod) : Results.NotFound()
 )
.Produces<Product>(StatusCodes.Status200OK)
.WithName("GetProductByID").WithTags("Getters");


// Get all Products from database using Paged Methods
app.MapGet("/products_by_page", async (int pageNumber, int pageSize, CommerceDB db) =>
    await db.Products.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync()
)
.Produces<List<Product>>(StatusCodes.Status200OK)
.WithName("GetProductsByPage").WithTags("Getters");


// Add a new Product to database
app.MapPost("/products", async ([FromBody] Product addProd, [FromServices] CommerceDB db, HttpResponse response) => {
        db.Products.Add(addProd);
        await db.SaveChangesAsync();
        response.StatusCode = 200;
        response.Headers.Location = $"products/{addProd.id}";
})
.Accepts<Product>("application/json")
.Produces<Product>(StatusCodes.Status201Created)
.WithName("AddNewProduct").WithTags("Setters");


// Update existing Product
app.MapPut("/products/{id}", async (int id, [FromBody] Product updateProd, [FromServices] CommerceDB db, HttpResponse response) => {
        var prod = db.Products.SingleOrDefault(s => s.id == id);
        if (prod == null) return Results.NotFound();

        prod.nome = updateProd.nome;
        prod.preco = updateProd.preco;
        prod.descricao = updateProd.descricao;
        prod.foto = updateProd.foto;
        prod.formatoImagem = updateProd.formatoImagem;
        prod.dataCriacao = updateProd.dataCriacao;

        await db.SaveChangesAsync();
        return Results.Created("/products", prod);
})
.Produces<Product>(StatusCodes.Status201Created).Produces(StatusCodes.Status404NotFound)
.WithName("UpdateProduct").WithTags("Setters");


// Search Products that contain {keyword} in the description
app.MapGet("/products/search/{keyword}", (string keyword, CommerceDB db) => {
    var _selectedProducts = db.Products.Where(x => x.descricao.ToLower().Contains(keyword.ToLower())).ToList();
    return (_selectedProducts.Count > 0) ? Results.Ok(_selectedProducts) : Results.NotFound(Array.Empty<Product>());
})
.Produces<List<Product>>(StatusCodes.Status200OK)
.WithName("Search").WithTags("Getters");


// Run the application
app.Run();
