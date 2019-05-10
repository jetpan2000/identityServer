using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Octacom.Odiss.Core.Identity.Contracts;

namespace Octacom.Odiss.Core.Identity.Bootstrap.Odiss5.Implementations
{
    public class SmsService : ISmsService
    {
        public Task SendAsync(IdentityMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
