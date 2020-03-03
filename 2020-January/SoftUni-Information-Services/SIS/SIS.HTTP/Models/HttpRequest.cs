namespace SIS.HTTP.Models
{
    using System.Collections.Generic;
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
            
            HttpRequestParser.ParseRequest(httpRequestAsString, this);
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
    }
}