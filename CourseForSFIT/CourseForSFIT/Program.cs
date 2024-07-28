using Data.Data;
using Data.Jwt;
using Data.Mapping;
using Data.MappingProfile;
using Dtos.Models.AuthModels;
using Dtos.Models.CourseModels;
using Dtos.Models.CourseTypeModels;
using Dtos.Models.ExerciseModels;
using Dtos.Models.TestCaseModels;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Repositories.Repositories.Base;
using Services.Courses;
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

//Validators
builder.Services.AddScoped<IValidator<ResetPassword>, ResetPasswordValidator>();
builder.Services.AddScoped<IValidator<VerifyVerificationCodeRequest>, VerifyVerificationCodeRequestValidator>();
builder.Services.AddScoped<IValidator<UserLoginDto>, UserLoginDtoValidator>();
builder.Services.AddScoped<IValidator<UserSignUpDto>, UserSignUpDtoValidator>();
builder.Services.AddScoped<IValidator<TagExerciseAddDto>, TagExerciseAddDtoValidator>();
builder.Services.AddScoped<IValidator<TagExerciseUpdateDto>, TagExerciseUpdateDtoValidator>();
builder.Services.AddScoped<IValidator<CourseTypeAdd>, CourseTypeAddValidator>();
builder.Services.AddScoped<IValidator<CourseTypeUpdate>, CourseTypeUpdateValidator>();
builder.Services.AddScoped<IValidator<ExerciseAddDto>, ExerciseAddDtoValidator>();
builder.Services.AddScoped<IValidator<TestCaseExerciseAddDto>, TestCaseExerciseAddDtoValidator>();
builder.Services.AddScoped<IValidator<TestCaseExerciseUpdateDto>, TestCaseExerciseUpdateDtoValidator>();
builder.Services.AddScoped<IValidator<ExerciseUpdateDto>, ExerciseUpdateDtoValidator>();
builder.Services.AddScoped<IValidator<TestCaseSolve>, TestCaseSolveValidator>();
builder.Services.AddScoped<IValidator<CourseAddDto>, CourseAddDtoValidator>();
builder.Services.AddScoped<IValidator<ChapterAdd>, ChapterAddValidator>();
builder.Services.AddScoped<IValidator<LessonAddDto>, LessonAddDtoValidator>();

//Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<ITagExerciseService, TagExerciseService>();
builder.Services.AddScoped<IExerciseService, ExerciseService>();
builder.Services.AddScoped<IExerciseCommentService, ExerciseCommentService>();
builder.Services.AddScoped<ITestCaseService, TestCaseService>();
builder.Services.AddScoped<ISolveTestCaseService, SolveTestCaseService>();
builder.Services.AddScoped<IUserExerciseService, UserExerciseService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICourseTypeService, CourseTypeService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IChapterService, ChapterService>();
builder.Services.AddScoped<ILessonService, LessonService>();


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
