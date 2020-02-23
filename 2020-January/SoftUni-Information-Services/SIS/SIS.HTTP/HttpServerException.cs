namespace SIS.HTTP
{
    using System;

    public class HttpServerException :Exception
    {
        public HttpServerException(string message) 
            : base(message)
        {
        }
    }
}