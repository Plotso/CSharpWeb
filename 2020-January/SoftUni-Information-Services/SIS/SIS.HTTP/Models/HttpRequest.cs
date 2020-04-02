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
            FormData = new Dictionary<string, string>();
            QueryData = new Dictionary<string, string>();
            
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
        
        /// <summary>
        /// Used to save data from HTML Form attributes
        /// </summary>
        public IDictionary<string, string> FormData { get; set; }
        
        /// <summary>
        /// Used to save everything from the Path after '?' separator
        /// </summary>
        public string Query { get; set; }

        /// <summary>
        /// Used to save data from the Query
        /// </summary>
        public IDictionary<string, string> QueryData { get; set; }
    }
}