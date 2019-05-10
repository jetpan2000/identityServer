using System;

namespace Octacom.Odiss.Core.Contracts.Settings
{
    public interface IApplicationService
    {
        Application Get(string identifier);
        Application Get(Type type);
    }
}