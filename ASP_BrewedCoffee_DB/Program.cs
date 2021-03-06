var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddSingleton<CConfService>();
builder.Configuration.AddCSVProviderData();
builder.Services.AddDbContext<CDBContext>();
CConfService.DB = builder.Services.BuildServiceProvider().GetService<CDBContext>();
builder.Configuration.AddDBProviderData();
builder.Services.AddSingleton<CRoutesService>();
builder.Services.AddSingleton<CApiService>();
//--------------------------------------------------
CRoutesService routes_service = builder.Services.BuildServiceProvider().GetService<CRoutesService>();
CApiService api_service = builder.Services.BuildServiceProvider().GetService<CApiService>();
builder.Services.Configure<RAdminData>(builder.Configuration);
//--------------------------------------------------
builder.Services.AddTransient<CPostsService>();
builder.Services.AddTransient<CCategoriesService>();
builder.Services.AddTransient<CAuthService>();
//--------------------------------------------------
builder.Services.AddDistributedMemoryCache();
builder.Services.AddMvc((options) => options.EnableEndpointRouting = false).AddSessionStateTempDataProvider();
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".MyApp.Session";
    options.IdleTimeout = TimeSpan.FromDays(90);
    options.Cookie.IsEssential = true;
    options.Cookie.HttpOnly = true;
    options.Cookie.MaxAge = TimeSpan.FromDays(999);
});
//--------------------------------------------------
var app = builder.Build();
// Configure the HTTP request pipeline.
CConfService.WebRootPath = app.Environment.WebRootPath;
api_service.UseApi(app, routes_service);
//--------------------------------------------------
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseMySessionData();
//--------------------------------------------------
routes_service.SetRoutes(app);
app.Run();