using System.Net.Sockets;
using System.Text;

string cmd = args.Length > 0 ? $"START {args[0]}" : "START";

using var client = new TcpClient();
await client.ConnectAsync("127.0.0.1", 9000);
using var stream = client.GetStream();
using var reader = new StreamReader(stream, Encoding.UTF8);
using var writer = new StreamWriter(stream, new UTF8Encoding(false))
{ AutoFlush = true, NewLine = "\n" };

await writer.WriteLineAsync(cmd);
Console.WriteLine($"TX: {cmd}");
Console.WriteLine($"RX: {await reader.ReadLineAsync()}");