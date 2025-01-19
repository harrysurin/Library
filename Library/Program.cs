using Microsoft.EntityFrameworkCore;
using LibraryRepository.Models;
using LibraryServices.Interfaces;
using Microsoft.OpenApi.Models;
using AutoMapper;
using Library.ViewModels;
using LibraryServices.Validation;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration["ConnectionString"];
builder.Services.AddDbContext<LibraryContext>(options =>
    options.UseSqlServer(connectionString));


builder.Services.AddTransient<IRepository<Author>, Repository<Author>>();
builder.Services.AddTransient<IRepository<Book>, Repository<Book>>();
builder.Services.AddTransient<IRepository<RentHistory>, Repository<RentHistory>>();
builder.Services.AddTransient<IPictureRepository<BookPictures>, PictureRepository>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

builder.Services.AddTransient<IAuthorServices, AuthorService>();
builder.Services.AddTransient<IBookServices, BookService>();
builder.Services.AddTransient<IRentHistoryServices, RentHistoryService>();
builder.Services.AddTransient<IUserServices, UserServices>();
builder.Services.AddTransient<IBookPicturesServices, BookPicturesServices>();
builder.Services.AddTransient<IEmailServices, EmailServices>();

builder.Services.AddTransient<AuthorValidator>();
builder.Services.AddTransient<BookValidator>();
builder.Services.AddTransient<RentHistoryValidator>();

Mapper.Initialize(
        cfg =>
        {
            cfg.CreateMap<Author, AuthorViewModel>();
            cfg.CreateMap<AuthorViewModel, Author>();

            cfg.CreateMap<Book, BookViewModel>();
            cfg.CreateMap<BookViewModel, Book>();

            cfg.CreateMap<BookPictureViewModel, BookPictures>();
            cfg.CreateMap<BookPictures, BookPictureViewModel>();
            

        });


builder.Services.AddControllers();
builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "APIforSPA", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

builder.Services.AddIdentityApiEndpoints<User>()
    .AddRoles<Role>()
    .AddEntityFrameworkStores<LibraryContext>();

var app = builder.Build();
app.MapIdentityApi<User>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        string adminEmail = "admin@mail.com";
        string adminPassword = "adminPassword1!!";

        var userServices = services.GetRequiredService<IUserServices>();
        await RoleInitializer.InitializeAsync(userServices, adminEmail, adminPassword);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

app.Run();

