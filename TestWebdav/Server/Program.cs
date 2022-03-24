using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

//tentative pour webdav
app.UseCors(builder => {
    builder.AllowAnyOrigin();
    builder.AllowAnyMethod();
    builder.AllowAnyHeader();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

//app.UseHttpsRedirection();
var lHostEnv = app.Services.GetService<IHostEnvironment>();
//config webdav
app.Use(async (context, next) =>
{
    if (context.Request.Path.StartsWithSegments("/Webdav"))
    {
        //TODO put a user of your system here
        byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes("admin:admin");
        string base64string = System.Convert.ToBase64String(toEncodeAsBytes);
        context.Request.Headers.Remove("Authorization");
        context.Request.Headers.Add("Authorization", "Basic " + base64string);

        //AppliGeckosSL.WriteLogDebug($"intercept {context.Request.Path}, Method : {context.Request.Method}", null);
        //var lFileInfo = lHostEnv.ContentRootFileProvider.GetFileInfo(context.Request.Path);
        //if (lFileInfo != null)
        //{

        //HttpApplication application = (HttpApplication)pSource;
        //HttpContext context = application.Context;
        //attention sur certaines version de word et/ou iis on se prend un header Authorization ajouté par word qu'il faut absolument supprimé

        // }
    }



    // Call the next delegate/middleware in the pipeline.
    await next(context);
});

//on tente de mapper directement dans IIS
//if (Directory.Exists(Sys.Web.AppliIs.Path_Webdav))
//{
//    var lOptions = new FileServerOptions
//    {
//        FileProvider = new PhysicalFileProvider(Sys.Web.AppliIs.Path_Webdav),
//        RequestPath = new PathString("/" + Sys.Web.AppliIs.WEBDAV_FOLDER),
//        EnableDirectoryBrowsing = false,

//    };
//    //lOptions.StaticFileOptions.ContentTypeProvider = provider;
//    lOptions.StaticFileOptions.OnPrepareResponse = (context) =>
//    {
//        var headers = context.Context.Response.Headers;
//        var contentType = headers["Content-Type"];

//        //si on ne met pas ca les fichioers html et txt sont mal encodés

//        contentType += "; charset=ISO-8859-1";

//        headers["Content-Type"] = contentType;

//    };
//    app.UseFileServer(lOptions);
//}
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
           Path.Combine(builder.Environment.ContentRootPath, "WebdavRepository")),
    RequestPath = "/Webdav"
});





app.UseRouting();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
