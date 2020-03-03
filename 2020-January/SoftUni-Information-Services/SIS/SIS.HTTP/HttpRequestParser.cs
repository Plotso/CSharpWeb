namespace SIS.HTTP
{
    using System;
    using System.Text;
    using Enums;
    using Models;

    public static class HttpRequestParser
    {
        public static void ParseRequest(string httpRequestAsString, HttpRequest request)
        {
            var lines = httpRequestAsString.Split(
                new[] {HttpConstants.NewLine}, 
                StringSplitOptions.None);

            ParseHttpRequestLine(lines, request);
            ParseHeadersAndBody(lines, request);
        }
        
         private static void ParseHttpRequestLine(string[] lines, HttpRequest request)
        {
            var httpInfoHeader = lines[0];
            var infoHeaderParts = httpInfoHeader.Split(' ');
            if (infoHeaderParts.Length != 3)
            {
                throw new HttpServerException("Invalid HTTP header line.");
            }

            var httpMethod = infoHeaderParts[0];
            request.Method = httpMethod switch
            {
                "GET" => HttpMethodType.Get,
                "POST" => HttpMethodType.Post,
                "PUT" => HttpMethodType.Put,
                "DELETE" => HttpMethodType.Delete,
                _ => HttpMethodType.Unknown
            };

            request.Path = infoHeaderParts[1];

            var httpVersion = infoHeaderParts[2];
            request.Version = httpVersion switch
            {
                "HTTP/1.0" => HttpVersionType.Http10,
                "HTTP/1.1" => HttpVersionType.Http11,
                "HTTP/2.0" => HttpVersionType.Http20,
                _ => HttpVersionType.Http11
            };
        }
        
        private static void ParseHeadersAndBody(string[] lines, HttpRequest request)
        {
            var isInHeader = true;
            var bodyBuilder = new StringBuilder();
            for (int i = 1; i < lines.Length; i++)
            {
                var line = lines[i];
                if (string.IsNullOrWhiteSpace(line))
                {
                    isInHeader = false;
                    continue;
                }

                if (isInHeader)
                {
                    var headerParts = line.Split(
                        new string[] {": "},
                        2,
                        StringSplitOptions.None);
                    if (headerParts.Length != 2)
                    {
                        throw new HttpServerException("Invalid header: " + line);
                    }

                    var header = new Header(headerParts[0], headerParts[1]);
                    request.Headers.Add(header);

                    if (headerParts[0] == "Cookie")
                    {
                        ParseCookies(headerParts, request);
                    }
                }
                else
                {
                    bodyBuilder.AppendLine(line);
                }
            }

            request.Body = bodyBuilder.ToString().TrimEnd('\r', '\n');
        }

        private static void ParseCookies(string[] cookieHeader, HttpRequest request)
        {
            var cookiesAsString = cookieHeader[1];
            var cookies = cookiesAsString.Split(new string[] {"; "}, StringSplitOptions.RemoveEmptyEntries);
            foreach (var cookie in cookies)
            {
                var cookieParts = cookie.Split(new char[] {'='}, 2);
                if (cookieParts.Length == 2)
                {
                    request.Cookies.Add(new Cookie(cookieParts[0], cookieParts[1]));
                }
            }
        }
    }
}