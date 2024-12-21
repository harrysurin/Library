using Microsoft.EntityFrameworkCore;
using LibraryRepository.Models;
using LibraryServices.Interfaces;
using Microsoft.OpenApi.Models;
using AutoMapper;
using Library.ViewModels;

var builder = WebApplication.CreateBuilder(args);

var connectionString = "Server=localhost\\SQLEXPRESS;Database=LibraryApp;Trusted_Connection=True;TrustServerCertificate=True";
builder.Services.AddDbContext<LibraryContext>(options =>
    options.UseSqlServer(connectionString));




builder.Services.AddTransient<IRepository<Author>, Repository<Author>>();
builder.Services.AddTransient<IRepository<Book>, Repository<Book>>();
builder.Services.AddTransient<IRepository<RentHistory>, Repository<RentHistory>>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<IAuthorServices, AuthorService>();
builder.Services.AddTransient<IBookServices, BookService>();
builder.Services.AddTransient<IRentHistoryServices, RentHistoryService>();

Mapper.Initialize(
        cfg =>
        {
            cfg.CreateMap<Author, AuthorViewModel>();
            cfg.CreateMap<AuthorViewModel, Author>();

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

app.MapControllers();
app.Run();

