using System.Net;
using System.Net.Sockets;
using System.Text;

namespace VisionInspector;

public class TcpInspectionServer
{
    private TcpListener? _listener;
    private CancellationTokenSource? _cts;
    private readonly Func<string, string> _handler;

    public TcpInspectionServer(Func<string, string> handler) => _handler = handler;

    public void Start(int port)
    {
        _cts = new CancellationTokenSource();
        _listener = new TcpListener(IPAddress.Loopback, port); // 다른 PC 접속은 IPAddress.Any
        _listener.Start();
        _ = Task.Run(() => AcceptLoop(_cts.Token));
    }

    public void Stop()
    {
        _cts?.Cancel();
        _listener?.Stop();
    }

    private async Task AcceptLoop(CancellationToken ct)
    {
        try
        {
            while (!ct.IsCancellationRequested)
            {
                var client = await _listener!.AcceptTcpClientAsync(ct);
                _ = Task.Run(() => HandleClient(client, ct));
            }
        }
        catch (OperationCanceledException) { }
        catch (ObjectDisposedException) { }
        catch (SocketException) { }
    }

    private async Task HandleClient(TcpClient client, CancellationToken ct)
    {
        using (client)
        using (var stream = client.GetStream())
        using (var reader = new StreamReader(stream, Encoding.UTF8))
        using (var writer = new StreamWriter(stream, new UTF8Encoding(false))
        { AutoFlush = true, NewLine = "\n" })
        {
            try
            {
                string? line;
                while ((line = await reader.ReadLineAsync(ct)) != null)
                {
                    string reply;
                    try { reply = _handler(line.Trim()); }
                    catch (Exception ex) { reply = $"ERROR,{ex.Message}"; }
                    await writer.WriteLineAsync(reply);
                }
            }
            catch (Exception) { /* 연결 끊김 등은 무시 */ }
        }
    }
}