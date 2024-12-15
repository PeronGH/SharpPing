if (args.Length < 2)
{
    Console.WriteLine("Usage: dotnet run <host> <port>");
    return;
}

var host = args[0];
var port = int.Parse(args[1]);

var latencies = new List<double>();

Console.CancelKeyPress += (sender, e) =>
{
    var successfulLatencies = latencies.Where(latency => latency > 0);

    var totalCount = latencies.Count();
    var successfulCount = successfulLatencies.Count();
    var successfulRate = totalCount > 0 ? (double)successfulCount / totalCount : 0;

    var averageLatency = successfulLatencies.Any() ? successfulLatencies.Average() : 0;
    var minLatency = successfulLatencies.Any() ? successfulLatencies.Min() : 0;
    var maxLatency = successfulLatencies.Any() ? successfulLatencies.Max() : 0;

    Console.WriteLine($"Finished.");
    Console.WriteLine($"{totalCount} attempts made, {successfulCount} successful, successful rate: {successfulRate:P2}.");
    Console.WriteLine($"Avg: {averageLatency:F2} ms, Min: {minLatency:F2} ms, Max: {maxLatency:F2} ms.");
};

while (true)
{
    var latency = TcpPinger.Ping(host, port);
    latencies.Add(latency);

    if (latency <= 0) continue;

    Console.WriteLine($"TCP ping to {host}:{port} succeeded in {latency} ms.");

    if (latency < 1000)
    {
        Thread.Sleep(1000 - (int)latency);
    }
}

