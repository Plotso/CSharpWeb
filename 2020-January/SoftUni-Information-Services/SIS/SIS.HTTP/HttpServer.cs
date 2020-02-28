namespace SIS.HTTP
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Enums;
    using Models;

    public class HttpServer : IHttpServer
    {
        private const string HttpNewLine = "\r\n";
        private readonly IList<Route> _routeTable;
        private readonly TcpListener _tcpListener;

        private readonly IDictionary<string, IDictionary<string, string>> _sessionStorage;

        //ToDo: Actions
        public HttpServer(int port, IList<Route> routeTable)
        {
            _routeTable = routeTable;
            _sessionStorage = new Dictionary<string, IDictionary<string, string>>();
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
            var networkStream = tcpClient.GetStream();
            try
            {
                var requestBytes = new byte[1_000_000]; //4096 in most cases //ToDo: USE BUFFER
                var bytesRead = await networkStream.ReadAsync(
                    buffer: requestBytes,
                    offset: 0,
                    size: requestBytes.Length,
                    cancellationToken: CancellationToken.None);
                var requestToString = Encoding.UTF8.GetString(
                    bytes: requestBytes,
                    index: 0,
                    count: bytesRead);
                Console.WriteLine(requestToString);
                Console.WriteLine(new string('=', 60));

                var request = new HttpRequest(requestToString);

                string newSessionId = null;
                var sessionCookie = request.Cookies.FirstOrDefault(s => s.Name == HttpConstants.SessionIdCookieName);

                if (sessionCookie != null && _sessionStorage.ContainsKey(sessionCookie.Value))
                {
                    request.SessionData = _sessionStorage[sessionCookie.Value];
                }
                else
                {
                    newSessionId = Guid.NewGuid().ToString();
                    var cookiesDictionary = new Dictionary<string, string>
                    {
                        {HttpConstants.SessionIdCookieName, newSessionId}
                    };
                    _sessionStorage.Add(newSessionId, cookiesDictionary);
                    request.SessionData = cookiesDictionary;
                }

                var routeMap =
                    _routeTable.FirstOrDefault(x => x.HttpMethod == request.Method && x.Path == request.Path);
                
                var response = routeMap == null ?
                    new HttpResponse(HttpResponseCode.NotFound, new byte[0]) 
                    : routeMap.Action(request);

                response.Headers.Add(new Header("Server", "SoftUniServer/1.0"));
                if (newSessionId != null)
                {
                    response.Cookies.Add(new ResponseCookie(HttpConstants.SessionIdCookieName, newSessionId)
                    {
                        HttpOnly = true,
                        MaxAge = 3600
                    });
                }

                var responseToBytes = Encoding.UTF8.GetBytes(response.ToString());

                await networkStream.WriteAsync(
                    buffer: responseToBytes,
                    offset: 0,
                    size: responseToBytes.Length,
                    cancellationToken: CancellationToken.None);
                await networkStream.WriteAsync(
                    response.Body,
                    0,
                    response.Body.Length);
            }
            catch (Exception ex)
            {
                var errorResponse = new HttpResponse(
                    HttpResponseCode.InternalServerError,
                    Encoding.UTF8.GetBytes(ex.ToString()));
                errorResponse.Headers.Add(new Header("Content-Type", "text/plain"));

                var responseBytes = Encoding.UTF8.GetBytes(errorResponse.ToString());
                await networkStream.WriteAsync(responseBytes, 0, responseBytes.Length);
                await networkStream.WriteAsync(errorResponse.Body, 0, errorResponse.Body.Length);
            }
        }

        [Obsolete]
        private static string HardcodedResponse(string sessionId, string newSessionId, string content)
        {
            return "HTTP/1.0 200 OK" + HttpNewLine +
                   "Server: SoftUniServer/1.0" + HttpNewLine +
                   "Content-Type: text/html" + HttpNewLine +
                   "Set-Cookie: user=Niki; Max-Age: 3600; HttpOnly;" + HttpNewLine +
                   (string.IsNullOrWhiteSpace(sessionId)
                       ? ("Set-Cookie: sessionId=" + newSessionId + HttpNewLine)
                       : string.Empty) +
                   // "Location: https://google.com" + HttpNewLine +
                   // "Content-Disposition: attachment; filename=niki.html" + HttpNewLine +
                   "Content-Length: " + content.Length + HttpNewLine +
                   HttpNewLine +
                   content;
        }
    }
}