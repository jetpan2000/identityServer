using System.Configuration;
using Octacom.DapperRepository;

namespace Octacom.Odiss.Core.Identity.Dapper
{
    public class MainDatabase : Database
    {
        public MainDatabase() : base(ConfigurationManager.ConnectionStrings["main"].ConnectionString)
        {
        }
    }
}
