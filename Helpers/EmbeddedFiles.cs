using System;
using System.IO;
using System.Linq;
using Microsoft.Extensions.FileProviders;

namespace spa.Helpers
{
 public class EmbeddedFiles
 {


  private static EmbeddedFiles? _embeddedFiles = null;

  private ManifestEmbeddedFileProvider? _provider = null;
  
  
    public static EmbeddedFiles Instance
    {
   get
   {

    if (_embeddedFiles == null)
    {
     _embeddedFiles = new EmbeddedFiles();
    }

    return _embeddedFiles;

   }
   }





    public ManifestEmbeddedFileProvider get_embedded_provider()
    {

     if (_provider is null)
     {
     _provider = new ManifestEmbeddedFileProvider(typeof(Program).Assembly, "embedded");
     }
     

     return _provider;
    }
    
    
    
    
    
    
    
    
    
   private void CopyFromResourceLib(string path,string name)
   {
    
   using var resource = get_embedded_provider().GetFileInfo(path).CreateReadStream();
   using var file = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), name), FileMode.Create, FileAccess.Write);
   resource.CopyTo(file);
   }




   public void extract_winpty_lib()
   {
    
   if (!File.Exists("winpty.dll"))
   {
   CopyFromResourceLib("/winpty/winpty.dll","winpty.dll");
   }
  
   
   if (!File.Exists("winpty-agent.exe"))
   {
   CopyFromResourceLib("/winpty/winpty-agent.exe","winpty-agent.exe");
   }
   
   
   
   }
   



 

 }
 }