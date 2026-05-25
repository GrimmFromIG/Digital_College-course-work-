using System.Text;
using DigitalCollege.BLL.Interfaces;
using DigitalCollege.BLL.Services;
using DigitalCollege.DAL.Context;
using DigitalCollege.DAL.Entities;
using DigitalCollege.DAL.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DepartmentDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAcademicService, AcademicService>();
builder.Services.AddScoped<IManagementService, ManagementService>();
builder.Services.AddScoped<IPublicService, PublicService>();
builder.Services.AddScoped<IAuthService, AuthService>();

var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazor", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DepartmentDbContext>();
    db.Database.EnsureCreated();

    if (!db.Users.Any())
    {
        db.Users.Add(new User 
        { 
            Email = "admin@college.ua", 
            PasswordHash = "admin", 
            Role = "Manager" 
        });

        var g1 = new Group { Name = "КН-21" };
        db.Groups.Add(g1);
        db.SaveChanges();

        var t1 = new Teacher { FullName = "Олександр Петрович" };
        db.Teachers.Add(t1);
        db.SaveChanges();

        db.Users.Add(new User 
        { 
            Email = "teacher@college.ua", 
            PasswordHash = "teacher", 
            Role = "Teacher",
            TeacherId = t1.Id
        });

        var d1 = new Discipline { Name = "C# та .NET", TeacherId = t1.Id };
        db.Disciplines.Add(d1);

        var s1 = new Student { FullName = "Богдан Петрощук", GroupId = g1.Id };
        db.Students.Add(s1);
        db.SaveChanges();

        db.Users.Add(new User 
        { 
            Email = "student@college.ua", 
            PasswordHash = "student", 
            Role = "Student",
            StudentId = s1.Id
        });
        
        db.Grades.Add(new Grade { StudentId = s1.Id, DisciplineId = d1.Id, Value = 95, DateAssigned = DateTime.Now });
        
        db.SaveChanges();
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseCors("AllowBlazor");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();