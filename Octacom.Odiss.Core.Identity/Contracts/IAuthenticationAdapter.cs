namespace Octacom.Odiss.Core.Identity.Contracts
{
    public interface IAuthenticationAdapter
    {
        /// <summary>
        /// Checks if the given username and password will authenticate in the old Odiss 5 implementation (or other older versions)
        /// </summary>
        bool IsAuthenticatingInLegacyOdiss(string username, string password);
    }
}
