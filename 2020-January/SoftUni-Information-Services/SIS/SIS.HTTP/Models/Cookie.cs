namespace SIS.HTTP.Models
{
    /// <summary>
    /// Represents an HTTP Cookie information, consisting of <c>Name</c> and <c>Value</c>.
    /// </summary>
    public class Cookie
    {
        /// <summary>
        /// Initializes a new <see cref="Cookie"/> class.
        /// </summary>
        /// <param name="name">Cookie name</param>
        /// <param name="value">Cookie value</param>
        public Cookie(string name, string value)
        {
            Name = name;
            Value = value;
        }
        
        /// <summary>
        /// HTTP Cookie Name.
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// HTTP Cookie Value.
        /// </summary>
        public string Value { get; set; }
    }
}