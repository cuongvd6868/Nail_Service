using Nail_Service.Data;
using Nail_Service.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Nail_Service.Repository;
using Nail_Service.Repository.Impl;

using Microsoft.IdentityModel.Tokens;
using VNPAY.NET;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddOData(opt =>
        opt.Select().Filter().OrderBy().Expand().SetMaxTop(100).Count()
            .AddRouteComponents("odata", GetEdmModel()))
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    });


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();

// Add services to the container
//builder.Services.AddControllers()
//    .AddOData(opt =>
//        opt.Select().Filter().OrderBy().Expand().SetMaxTop(100).Count()
//            .AddRouteComponents("odata", GetEdmModel()));


// Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
}).AddEntityFrameworkStores<AppDbContext>();

// Dependency Injection
// Đăng ký VNPay service
builder.Services.AddSingleton<IVnpay, Vnpay>();
builder.Services.AddScoped<INailSalonRepository, NailSalonRepository>();



builder.Services.AddLogging(logging =>
{
    logging.AddConsole();
    logging.AddDebug();
});

// JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"])),
        //ClockSkew = TimeSpan.Zero, 
    };
});




//builder.Services.AddControllers().AddJsonOptions(opt => // khắc phục lỗi vòng lặp tuần hoàn, Nếu gặp đối tượng đã serialize trước đó → không serialize lại, mà dùng $ref
//{
//    opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
//});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

// CORS policy
app.UseCors(builder =>
{
    builder.AllowAnyMethod() // Cho phép tất cả các phương thức HTTP (GET, POST, PUT, DELETE, v.v.)
           .AllowAnyHeader() // Cho phép tất cả các header (Authorization, Content-Type..)
           .AllowCredentials() //	Cho phép gửi cookie hoặc Authorization header cùng với request.
           .SetIsOriginAllowed(origin => true); // Cho phép tất cả các origin
});

app.UseHttpsRedirection();
app.UseAuthentication(); // JWT
app.UseAuthorization();

app.MapControllers();

app.Run();

static IEdmModel GetEdmModel()
{
    var builder = new ODataConventionModelBuilder();
    //builder.EntitySet<Category>("Categories");
    return builder.GetEdmModel();
}