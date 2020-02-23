namespace SIS.HTTP
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Threading.Tasks;
    
    public class HttpServer : IHttpServer
    {
        private const string HttpNewLine = "\r\n";
        private readonly TcpListener _tcpListener;
        private static readonly Dictionary<string, int> SessionStorage = new Dictionary<string, int>();
        //ToDo: Actions
        public HttpServer(int port)
        {
            _tcpListener = new TcpListener(IPAddress.Loopback, port);
            
        }
        public async Task StartAsync()
        {
            _tcpListener.Start();
            while (true)
            {
                TcpClient tcpClient = await _tcpListener.AcceptTcpClientAsync();
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                Task.Run(async () => await ProcessClientAsync(tcpClient));
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            }
        }

        public async Task ResetAsync()
        {
            Stop();
            await StartAsync();
        }

        public void Stop()
        {
           _tcpListener.Stop();
        }
        
        private async Task ProcessClientAsync(TcpClient tcpClient)
        {
            NetworkStream networkStream = tcpClient.GetStream();
            byte[] requestBytes = new byte[1_000_000]; //4096 in most cases //ToDo: USE BUFFER
            int bytesRead = await networkStream.ReadAsync(
                buffer: requestBytes,
                offset: 0,
                size: requestBytes.Length,
                cancellationToken: CancellationToken.None);
            string requestToString = Encoding.UTF8.GetString(
                bytes: requestBytes,
                index: 0,
                count: bytesRead);
            Console.WriteLine(requestToString);
            Console.WriteLine(new string('=', 60));

            var sessionId = Regex.Match(requestToString, @"sessionId=[^\n]*\n").Value?.Replace("sessionId=", string.Empty).Trim();
            Console.WriteLine(sessionId);
            var newSessionId = Guid.NewGuid().ToString();
            var sessionCount = 0;
            if (SessionStorage.ContainsKey(sessionId))
            {
                SessionStorage[sessionId]++;
                sessionCount = SessionStorage[sessionId];
            }
            else
            {
                sessionId = null;
                SessionStorage[newSessionId] = 1;
                sessionCount = 1;
            }

            string responseText = "<h1>" + sessionCount + "</h1>" + "<h1>" + DateTime.UtcNow + "</h1>";
            string response = "HTTP/1.0 200 OK" + HttpNewLine +
                              "Server: SoftUniServer/1.0" + HttpNewLine +
                              "Content-Type: text/html" + HttpNewLine +
                              "Set-Cookie: user=Niki; Max-Age: 3600; HttpOnly;" + HttpNewLine +
                              (string.IsNullOrWhiteSpace(sessionId) ?
                                  ("Set-Cookie: sessionId=" + newSessionId + HttpNewLine)
                                  : string.Empty) +
                              // "Location: https://google.com" + HttpNewLine +
                              // "Content-Disposition: attachment; filename=niki.html" + HttpNewLine +
                              "Content-Length: " + responseText.Length + HttpNewLine +
                              HttpNewLine +
                              responseText;
            byte[] responseToBytes = Encoding.UTF8.GetBytes(response);
            await networkStream.WriteAsync(
                buffer: responseToBytes,
                offset: 0,
                size: responseToBytes.Length,
                cancellationToken: CancellationToken.None);
        }
    }
}