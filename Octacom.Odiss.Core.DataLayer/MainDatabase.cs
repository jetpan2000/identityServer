using System.Configuration;
using Octacom.DapperRepository;

namespace Octacom.Odiss.Core.DataLayer
{
    public class MainDatabase : Database
    {
        public MainDatabase() : base(ConfigurationManager.ConnectionStrings["main"].ConnectionString)
        {
        }
    }
}
