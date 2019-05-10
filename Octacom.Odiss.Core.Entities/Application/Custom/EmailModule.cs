namespace Octacom.Odiss.Core.Entities.Application.Custom
{
    public class EmailModule
    {
        /// <summary>
        /// Restrict certain domains when sending emails
        /// </summary>
        /// <example>
        /// Restrict octacom and google domains. Separate domains using semicolon (;)
        /// <code>
        /// @octacom.ca;@google.com
        /// </code>
        /// </example>
        public string DomainsRestriction { get; set; }
    }
}
