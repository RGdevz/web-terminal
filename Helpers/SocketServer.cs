using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace spa.Helpers
{

 public class SocketServerManager
 {

  private static SocketServerManager? _socketServerRef = null;

  public readonly ConcurrentDictionary<string, IClientProxy> ClientProxies = new();


   public async Task send_all(string method, string args)
   {
    
    

        await Parallel.ForEachAsync( ClientProxies.Values.ToArray(), async (proxy, token) =>
        {
        try
        {
         await proxy.SendAsync(method, args, cancellationToken: token);

        }
        catch (Exception e)
        {
         Console.WriteLine(e);
        }

       }
       );
       }
   
   
   
   
   
  
   public static SocketServerManager Instance
   {
   get
   {

    if (_socketServerRef is null)
    {
     _socketServerRef = new SocketServerManager();
    }

    return _socketServerRef;

   }
   }

   }
 
 
 
 
   public class SocketServer : Hub
   {
   public override Task OnDisconnectedAsync(Exception? exception)
   {

   Console.WriteLine("disconnected");


   try
   {
    SocketServerManager.Instance.ClientProxies.TryRemove(Context.ConnectionId, out IClientProxy _);

   }
   catch (Exception e)
   {
    Console.Error.WriteLine(e);
   }
   
   
   return base.OnDisconnectedAsync(exception);
  }
  
  
   
   

   public override Task OnConnectedAsync()
   {

   try
   {
    
   SocketServerManager.Instance.ClientProxies.TryAdd(Context.ConnectionId, Clients.Caller);
   }
   catch (Exception e)
   {
   Context.Abort();
   Console.Error.WriteLine(e);
   }


   return base.OnConnectedAsync();
  }


  
  
  
  
   public async Task send_pty(string cmd)
   {

    

   try
   {
    
    var term = await PtyHelper.Instance.get_terminal();

    await term.WriterStream.WriteAsync(Encoding.UTF8.GetBytes(cmd));
    await term.WriterStream.FlushAsync();

   }
   catch (Exception e)
   {
    await Console.Error.WriteLineAsync(e.Message);
   }

   /*await Clients.All.SendAsync("ReceiveMessage", $"received {user}");*/
  }

  
 }
 }