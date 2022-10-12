using System;
using JWT.Algorithms;
using JWT.Builder;

namespace spa.Helpers
{
 public class Jwt
 {

  private static string secret = "ggggdfgdgddddggggggggdddddddddggg";
  
  
   public static string create_jwt()
   {

    if (secret.Length < 32)
    {
     throw new Exception("jwt secret too short");
    }
   
    var token = JwtBuilder.Create()
    .WithAlgorithm(new HMACSHA256Algorithm())
    .WithSecret(secret)
    .AddClaim("exp", DateTimeOffset.UtcNow.AddDays(7).ToUnixTimeSeconds())
    .Encode();

   return token;
   }




    public static string decode_jwt(string token)
    {

    try
    {
     var json = JwtBuilder.Create()
     .WithAlgorithm(new HMACSHA256Algorithm())
     .WithSecret(secret)
     .MustVerifySignature()
     .Decode(token);

     return json;
    }
     catch (Exception e)
    {
    /*Console.Error.WriteLine(e.Message);*/
    throw e;
    }

    }
   
   
  
  
  }
  } 