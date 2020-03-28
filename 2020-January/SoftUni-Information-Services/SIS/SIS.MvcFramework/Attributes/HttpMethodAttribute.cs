namespace SIS.MvcFramework.Attributes
{
    using System;
    using HTTP.Enums;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public abstract class HttpMethodAttribute : Attribute
    {
        protected HttpMethodAttribute()
        {
        }
        
        protected HttpMethodAttribute(string url)
        {
            Url = url;
        }
        
        public string Url { get; }
        
        public abstract HttpMethodType Type { get; }
    }
}