using Data.Data;
using Data.Jwt;
using Data.Mapping;
using Data.MappingProfile;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Repositories.Repositories.Base;
using Services.Exercises;
using Services.TestCases;
using Services.Users;
using Shared.Configs;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<CourseForSFITContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CourseForSFITContext")));

builder.Services.AddHttpClient();
var provider = new FileExtensionContentTypeProvider();
provider.Mappings[".outOutputs"] = "application/octet-stream";
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigin",
        builder => builder
            .WithOrigins("http://127.0.0.1:5500", "http://localhost:3000") // Điền vào tên miền của dự án giao diện của bạn
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials() // Cho phép sử dụng credentials (cookies, xác thực)
    );
});





//Repositories
builder.Services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));


//Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<ITagExerciseService, TagExerciseService>();
builder.Services.AddScoped<IExerciseService, ExerciseService>();
builder.Services.AddScoped<IExerciseCommentService, ExerciseCommentService>();
builder.Services.AddScoped<ITestCaseService, TestCaseService>();
builder.Services.AddScoped<ISolveTestCaseService, SolveTestCaseService>();
builder.Services.AddScoped<IUserExerciseService, UserExerciseService>();

//Mapper
builder.Services.AddAutoMapper(typeof(UserMapper));
builder.Services.AddAutoMapper(typeof(ExerciseMapper));
builder.Services.AddAutoMapper(typeof(TestCaseMapper));


//Setting
builder.Services.Configure<EmailSetting>(builder.Configuration.GetSection("Email"));

builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddCookie(x =>
    {
        x.Cookie.Name = "token";
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            //2 dong
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                context.Token = context.Request.Cookies["X-Access-Token"];
                return Task.CompletedTask;
            }
        };
    });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Role", policy => policy.RequireClaim("Role", "1")); //1: User, 2: Admin
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseStaticFiles(new StaticFileOptions
{
    ContentTypeProvider = provider
});
app.UseJwtMiddleware();
app.UseCors("AllowOrigin");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
