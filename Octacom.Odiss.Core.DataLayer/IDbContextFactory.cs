using System;
using System.Data.Entity;

namespace Octacom.Odiss.Core.DataLayer
{
    [Obsolete("Use System.Data.Entity.Infrastructure.IDbContextFactory<DbContext> instead")]
    public interface IDbContextFactory
    {
        DbContext Get();
    }
}
