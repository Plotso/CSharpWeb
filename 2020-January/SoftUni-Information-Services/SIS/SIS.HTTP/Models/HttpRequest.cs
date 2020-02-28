namespace SIS.HTTP.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Enums;

    /// <summary>
    /// Represents an HTTP Request with properties for the <c>Request Line</c>, <c>Request Headers</c> and <c>Request Body</c>.
    /// </summary>
    public class HttpRequest
    {
        public HttpRequest(string httpRequestAsString)
        {
            if (string.IsNullOrWhiteSpace(httpRequestAsString))
            {
                return;
            }
            
            Headers = new List<Header>();
            Cookies = new List<Cookie>();

            var lines = httpRequestAsString.Split(
                new[] {HttpConstants.NewLine}, 
                StringSplitOptions.None);

            ParseHttpRequestLine(lines);
            ParseHeadersAndBody(lines);
        }
        
        /// <summary>
        /// HTTP Request line Method.
        /// </summary>
        public HttpMethodType Method { get; set; }
        
        /// <summary>
        /// HTTP Request line Path.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// HTTP Request line Version.
        /// </summary>
        public HttpVersionType Version { get; set; }
        
        // <summary>
        /// Collection of HTTP Request Headers.
        /// </summary>
        public IList<Header> Headers { get; set; }
        
        // <summary>
        /// Collection of HTTP Request Cookies.
        /// </summary>
        public IList<Cookie> Cookies { get; set; }
        
        /// <summary>
        /// HTTP Request Body.
        /// </summary>
        public string Body { get; set; }
        
        /// <summary>
        /// Used to populate information about current session
        /// </summary>
        public IDictionary<string, string> SessionData { get; set; }

        //ToDo: Extract below method to a parser class
        private void ParseHttpRequestLine(string[] lines)
        {
            var httpInfoHeader = lines[0];
            var infoHeaderParts = httpInfoHeader.Split(' ');
            if (infoHeaderParts.Length != 3)
            {
                throw new HttpServerException("Invalid HTTP header line.");
            }

            var httpMethod = infoHeaderParts[0];
            Method = httpMethod switch
            {
                "GET" => HttpMethodType.Get,
                "POST" => HttpMethodType.Post,
                "PUT" => HttpMethodType.Put,
                "DELETE" => HttpMethodType.Delete,
                _ => HttpMethodType.Unknown
            };

            Path = infoHeaderParts[1];

            var httpVersion = infoHeaderParts[2];
            Version = httpVersion switch
            {
                "HTTP/1.0" => HttpVersionType.Http10,
                "HTTP/1.1" => HttpVersionType.Http11,
                "HTTP/2.0" => HttpVersionType.Http20,
                _ => HttpVersionType.Http11
            };
        }
        
        private void ParseHeadersAndBody(string[] lines)
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
                    Headers.Add(header);

                    if (headerParts[0] == "Cookie")
                    {
                        ParseCookies(headerParts);
                    }
                }
                else
                {
                    bodyBuilder.AppendLine(line);
                }
            }

            Body = bodyBuilder.ToString().TrimEnd('\r', '\n');
        }

        private void ParseCookies(string[] cookieHeader)
        {
            var cookiesAsString = cookieHeader[1];
            var cookies = cookiesAsString.Split(new string[] {"; "}, StringSplitOptions.RemoveEmptyEntries);
            foreach (var cookie in cookies)
            {
                var cookieParts = cookie.Split(new char[] {'='}, 2);
                if (cookieParts.Length == 2)
                {
                    Cookies.Add(new Cookie(cookieParts[0], cookieParts[1]));
                }
            }
        }
    }
}