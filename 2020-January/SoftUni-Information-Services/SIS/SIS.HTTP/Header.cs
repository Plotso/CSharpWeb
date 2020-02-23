namespace SIS.HTTP
{
    /// <summary>
    /// Represents an HTTP Header information, consisting of <c>Name</c> and <c>Value</c>.
    /// </summary>
    public class Header
    {
        /// <summary>
        /// Initializes a new <see cref="Header"/> class.
        /// </summary>
        /// <param name="name">Header name</param>
        /// <param name="value">Header value</param>
        public Header(string name, string value)
        {
            Name = name;
            Value = value;
        }
        
        /// <summary>
        /// HTTP Header Name.
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// HTTP Header Value.
        /// </summary>
        public string Value { get; set; }
        
        public override string ToString() 
            => $"{this.Name}: {this.Value}";
    }
}