using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using TechWizWebApp.Data;
using TechWizWebApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<IFileService,FileService>();
builder.Services.AddScoped<IMailService, MailService>();

builder.Services.AddDbContext<TechwizDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
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


app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "Images")),
    RequestPath = "/Images"
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
