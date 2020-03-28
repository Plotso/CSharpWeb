namespace SIS.MvcFramework.Attributes
{
    using HTTP.Enums;

    public class HttpPostAttribute : HttpMethodAttribute
    {
        public HttpPostAttribute()
        {
        }

        public HttpPostAttribute(string url)
            : base(url)
        {
        }

        public override HttpMethodType Type => HttpMethodType.Post;
    }
}