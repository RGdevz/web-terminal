using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using spa.Helpers;


struct my_json
{
 public string my_username;
 public string my_password;

}


public class my_json2
{

 public string? username { get; set; }

 public string? password { get; set; }

}



public class Program
 {
 public static void Main(string[] args)
 {
 new main().init();
 }

 }



public class main
{



 
public void init()
{
 
 
 
 
var builder = WebApplication.CreateBuilder(new WebApplicationOptions()
{
    
/*WebRootPath = "wwwroot"*/
}
);




try
{
 var port = builder.Configuration.GetSection("Urls").Value;
 port = port.Substring(port.LastIndexOf(":")).Substring(1);
 Console.WriteLine($"http://localhost:{port}");
}
catch (Exception e)
{
 
}




 var json = File.ReadAllText("appsettings.json");

 var stuff = JsonConvert.DeserializeObject<my_json>(json)!;




 builder.Services.AddSignalR(options =>
 {
 options.EnableDetailedErrors = true; 
 } 
 );
 
 
 
 
 builder.Services.AddCors(options =>
 {
  options.AddPolicy("CorsApi", corsPolicyBuilder => corsPolicyBuilder
 .SetIsOriginAllowedToAllowWildcardSubdomains()
 .SetIsOriginAllowed(_=>true)
 .AllowAnyHeader()
 .AllowCredentials()
 .AllowAnyMethod());
  
 }
 );
 
 
 

 
var provider = new ManifestEmbeddedFileProvider(typeof(Program).Assembly,"embedded/clientapp/dist");
/*provider.Assembly.GetManifestResourceNames().ToList().ForEach(x => Console.WriteLine(x));*/




builder.Services.AddControllersWithViews();





var app = builder.Build();




// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
}


app.UseStaticFiles(new StaticFileOptions 
{
 FileProvider = provider
}
);




app.UseRouting();

app.UseCors("CorsApi");




  app.Use(async (httpContext, next) =>
  {

   
   if (!httpContext.Request.Path.Equals("/auth/login"))
   {
   await next.Invoke();
   return;
   }
   
   

  if (httpContext.Request.Method.Equals("GET"))
  {
  await httpContext.Response.SendFileAsync(provider.GetFileInfo("index.html"));
  return;
  }

  
  try
  {
   
  

   var form = await httpContext.Request.ReadFromJsonAsync<my_json2>();
   
    
   if (string.IsNullOrWhiteSpace(form?.username))
   {
    throw new Exception("no username");
   }
    
   if (string.IsNullOrWhiteSpace(form?.password))
   {
    throw new Exception("no password");
   }
   
   
   
   
   
    if (form.username.Equals(stuff.my_username))
    {
    if (form.password.Equals(stuff.my_password))
    {
    httpContext.Response.Cookies.Append("secret",Jwt.create_jwt(),new CookieOptions(){Expires = DateTimeOffset.UtcNow.AddDays(7)});
    await httpContext.Response.WriteAsync("ok");
    return;
    }
    }
    
    
   throw new Exception("bad credentials");
    
  }
  catch (Exception e)
  {
  httpContext.Response.StatusCode = 400;
  await httpContext.Response.WriteAsync(e.Message);
  }

  
 }
 );


 
  
  
  

  
  
  app.Use(async (context, next) =>
  {
   

  try
  {
   
  
   

  context.Request.Cookies.TryGetValue("secret", out var secret);
  

  if (string.IsNullOrWhiteSpace(secret))
  {
  throw new Exception("no secret");
  }
  
 Jwt.decode_jwt(secret);
  
 await next.Invoke();

 }
 catch (Exception e)
 {
 context.Response.StatusCode = 400;
 context.Response.ContentType = "text/html";
 await context.Response.WriteAsync("<b>Not logged in</> <br/> <a href=\"/auth/login\">Log in</>");
 }

 }
 );


 

 
 
 
  

  
  
  
  
app.MapControllerRoute(name: "default", pattern: "{controller}/{action=Index}/{id?}");
  
  

app.MapHub<SocketServer>("/websocket");

app.MapFallback((context) => context.Response.SendFileAsync(provider.GetFileInfo("index.html")));





app.Run();

}

}


