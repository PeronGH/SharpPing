using System.Net.Sockets;
using System.Diagnostics;

public class TcpPinger
{
    public static double Ping(string host, int port)
    {
        var stopwatch = new Stopwatch();
        try
        {
            using var client = new TcpClient();

            stopwatch.Start();
            var connectionTask = client.ConnectAsync(host, port);
            var timeoutTask = Task.Delay(5000);

            if (Task.WhenAny(connectionTask, timeoutTask).Result == timeoutTask)
            {
                throw new TimeoutException();
            }
            stopwatch.Stop();

            return stopwatch.Elapsed.TotalMilliseconds;
        }
        catch (Exception e)
        {
            Console.WriteLine($"TCP ping to {host}:{port} failed. Reason: {e.Message}");
            return -1;
        }
    }
}