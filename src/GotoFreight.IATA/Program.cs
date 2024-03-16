using GotoFreight.IATA.Controllers;
using GotoFreight.AICore;
using GotoFreight.IATA;
using GotoFreight.IATA.Repository;
using GotoFreight.IATA.X;
using Microsoft.Extensions.DependencyInjection.Extensions;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);
var configFiles = Directory.GetFiles("configs");
foreach (var file in configFiles)
{
    if (file.EndsWith(".json"))
        builder.Configuration.AddJsonFile(file);
}

builder.Configuration.AddEnvironmentVariables().AddCommandLine(args);
#if DEBUG
builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json");
#endif

builder.Services.AddExceptionHandler<DefaultExceptionHandler>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Clear();
    options.JsonSerializerOptions.Converters.Add(new DateTimeJsonConverter());
});
builder.Services.AddHostedService<Bootstrap>();
builder.Services.TryAddSingleton<DbContext>();
builder.Services.Scan(scan => scan
    .FromAssemblyOf<HomeController>()
    .AddClasses(classes => classes.Where(q => q.Name.EndsWith("Service") || q.Name.EndsWith("Repository")))
    .AsSelf()
    .AsImplementedInterfaces()
    .WithScopedLifetime());

var useAzureOpenAI = builder.Configuration.GetValue<bool?>("UseAzureOpenAI");
if (useAzureOpenAI != null && useAzureOpenAI == true)
{
    builder.Services.AddAzureOpenAICoreService();
}
else
{
    builder.Services.AddOpenAICoreService();
}

builder.Services.AddHttpContextAccessor();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins, policy =>
    {
        policy.AllowAnyOrigin();
        policy.AllowAnyMethod();
        policy.AllowAnyHeader();
    });
});


var app = builder.Build();
app.UseExceptionHandler(opt => { });
app.UseStaticFiles();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();

app.UseCors(MyAllowSpecificOrigins);
app.MapControllers();

app.Run();