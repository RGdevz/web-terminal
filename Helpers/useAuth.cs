using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace spa.Helpers
{
 
 public class MyAuthAttribute : Attribute,IAuthorizationFilter
 {
  
 
  public void OnAuthorization(AuthorizationFilterContext context)
  
  {


   context.HttpContext.Request.Cookies.TryGetValue("secret", out var secret);
   
   
   try
   {
    

    if (string.IsNullOrWhiteSpace(secret))
    {
    throw new Exception("no secret");
    }
  
    Jwt.decode_jwt(secret);
    

    }
    catch (Exception e)
    {
    context.Result = new BadRequestResult();
    }
   

   
   }
   }
   }