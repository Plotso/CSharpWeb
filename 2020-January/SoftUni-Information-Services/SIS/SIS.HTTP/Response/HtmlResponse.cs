namespace SIS.HTTP.Response
{
    using System.Text;
    using Enums;
    using Models;
    
    /// <summary>
    /// Represents an HTML Response with properties for the <c>Response Status Line</c>, <c>Response Headers</c> and <c>Response Body</c>.
    /// </summary>
    public class HtmlResponse : HttpResponse
    {
        /// <summary>
        /// Initializes a new <see cref="HtmlResponse"/> class.
        /// </summary>
        /// <param name="html">HTML text</param>
        public HtmlResponse(string html)
        {
            StatusCode = HttpResponseCode.Ok;
            byte[] byteData = Encoding.UTF8.GetBytes(html);
            Body = byteData;
            Headers.Add(new Header("Content-Type", "text/html"));
            Headers.Add(new Header("Content-Length", Body.Length.ToString()));
        }
    }
}