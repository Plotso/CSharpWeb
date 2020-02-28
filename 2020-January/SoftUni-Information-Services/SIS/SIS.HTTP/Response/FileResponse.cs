namespace SIS.HTTP.Response
{
    using Enums;
    using Models;

    /// <summary>
    /// Represents a File Response with properties for the <c>Response Status Line</c>, <c>Response Headers</c> and <c>Response Body</c>.
    /// </summary>
    public class FileResponse : HttpResponse
    {
        /// <summary>
        /// Initializes a new <see cref="FileResponse"/> class.
        /// </summary>
        /// <param name="fileContent">File bytes</param>
        /// <param name="contentType">MIME Content type</param>
        public FileResponse(byte[] fileContent, string contentType)
        {
            StatusCode = HttpResponseCode.Ok;
            Body = fileContent;
            Headers.Add(new Header("Content-Type", contentType));
            Headers.Add(new Header("Content-Length", Body.Length.ToString()));
        }
    }
}