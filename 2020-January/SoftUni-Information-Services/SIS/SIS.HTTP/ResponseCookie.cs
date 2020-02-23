namespace SIS.HTTP
{
    using System;
    using System.Text;
    using Enums;

    public class ResponseCookie : Cookie
    {
        public ResponseCookie(string name, string value) 
            : base(name, value)
        {
        }
        
        /// <summary>
        /// Cookie scope attribute - <c>Domain</c>.
        /// Specifies allowed hosts to receive the cookie.
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// Cookie scope attribute - <c>Path</c>.
        /// Indicates a URL path that must exist in the requested URL in order to send the Cookie header.
        /// </summary>
        public string Path { get; set; }
        
        /// <summary>
        /// Cookie lifetime attribute - <c>Expires</c>.
        /// Sets an expiry date for when a cookie gets deleted.
        /// </summary>
        public DateTime? Expires { get; set; }

        /// <summary>
        /// Cookie lifetime attribute - <c>Max-Age</c>.
        /// Sets the time in seconds for when a cookie will be deleted.
        /// </summary>
        public long? MaxAge { get; set; }
        
        /// <summary>
        /// Cookie security attribute - <c>Secure</c> flag.
        /// The secure flag is an option that can be set by the application server when sending a new cookie to the user within an HTTP Response.
        /// The purpose of the secure flag is to prevent cookies from being observed by unauthorized parties due to the transmission of a the cookie in clear text.
        /// To accomplish this goal, browsers which support the secure flag will only send cookies with the secure flag when the request is going to a HTTPS page.
        /// Said in another way, the browser will not send a cookie with the secure flag set over an unencrypted HTTP request.
        /// </summary>
        public bool Secure { get; set; }

        /// <summary>
        /// Cookie security attribute - <c>HttpOnly</c> flag.
        /// Helps mitigate the risk of client side script accessing the protected cookie.
        /// If the <c>HttpOnly</c> flag is included in the HTTP response header, the cookie cannot be accessed through client side script
        /// </summary>
        public bool HttpOnly { get; set; }

        /// <summary>
        /// Cookie security attribute - <c>SameSite</c>. 
        /// SameSite prevents the browser from sending this cookie along with cross-site requests. 
        /// The main goal is mitigate the risk of cross-origin information leakage. It also provides some protection against cross-site request forgery attacks.
        /// </summary>
        public SameSiteType SameSite { get; set; }
        
        /// <summary>
        /// Returns formatted HTTP Response Cookie.
        /// </summary>
        public override string ToString()
        {
            var cookieBuilder = new StringBuilder();
            cookieBuilder.Append($"{Name}={Value}");
            if (MaxAge.HasValue)
            {
                cookieBuilder.Append("; Max-Age=" + MaxAge.Value);
            }
            else if (Expires.HasValue)
            {
                cookieBuilder.Append("; Expires=" + Expires.Value.ToString("R"));
            }

            if (!string.IsNullOrWhiteSpace(Domain))
            {
                cookieBuilder.Append("; Domain=" + Domain);
            }

            if (!string.IsNullOrWhiteSpace(Path))
            {
                cookieBuilder.Append("; Path=" + Path);
            }

            if (Secure)
            {
                cookieBuilder.Append("; Secure");
            }

            if (HttpOnly)
            {
                cookieBuilder.Append("; HttpOnly");
            }

            cookieBuilder.Append("; SameSite=" + SameSite);

            return cookieBuilder.ToString();
        }
    }
}