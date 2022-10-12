using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Pty.Net;

namespace spa.Helpers
{
 public class PtyHelper
 {
  private IPtyConnection? _ptyConnection = null;

  private static PtyHelper? _ptyHelper = null;

  private CancellationTokenSource? _cancellationToken = null;
  protected UTF8Encoding Encoding = new(false);

  private PtyHelper()
   {
   if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
   {
   EmbeddedFiles.Instance.extract_winpty_lib();
   }
   }
  
  


   public static PtyHelper Instance
   {
   get
   {

    if (_ptyHelper is null)
    {
     _ptyHelper = new PtyHelper();
    }

    return _ptyHelper;

   }



   }
 


  
  
  public void kill_terminal()
  {
   _cancellationToken?.Cancel();
   _ptyConnection?.Dispose();
   _ptyConnection = null;

  }


  
  
  

   public async Task<IPtyConnection> get_terminal()
   {

   if (_ptyConnection is null)
   {


    string shell_type = string.Empty;



    if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
    {

     shell_type = "bash";
     
    }
    else
    {

     shell_type = "cmd";
     
    }


    if (string.IsNullOrWhiteSpace(shell_type))
    {
     throw new Exception("unsupported os");
    }
   
    

    _cancellationToken = new CancellationTokenSource();

    _ptyConnection = await PtyProvider.SpawnAsync(new PtyOptions() {Cols = 90, Rows = 30, App = shell_type, Cwd = "/", VerbatimCommandLine = true, ForceWinPty = true}, _cancellationToken.Token);
    

    
    _ptyConnection.ProcessExited += (sender, args) =>
     {
     kill_terminal();
     };

    
    
      new Task( () =>
      {


     read_term(_ptyConnection.ReaderStream);
     
          

      }, _cancellationToken.Token,TaskCreationOptions.LongRunning).Start();


      }



     return _ptyConnection;


      }


   
   
   
   

   public async void read_term(Stream term)
   {
    
    
    
    byte[] buffer = new byte[8096];

    while (!_cancellationToken!.IsCancellationRequested)
    {
     
     try
     {


     int count = await term.ReadAsync(buffer, 0, buffer.Length, this._cancellationToken.Token);

     if (count == 0)
     {
      return;
     }
     
     
     var stuff = Encoding.GetString(buffer, 0, count);

      /*await clientProxy.SendAsync("shell_session", stuff);*/

    

       await SocketServerManager.Instance.send_all("shell_session", stuff);



      }
      catch (Exception e)
      {
       await Console.Error.WriteLineAsync(e.Message);
      }

     
     
     }
    
    
 
    
     }


   
  
 
    }




    }