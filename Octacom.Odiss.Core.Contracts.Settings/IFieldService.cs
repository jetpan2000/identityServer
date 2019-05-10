using Octacom.Odiss.Core.Contracts.Settings.Entities;

namespace Octacom.Odiss.Core.Contracts.Settings
{
    public interface IFieldService
    {
        Field GetField(string fieldIdentifier, string applicationIdentifier);
        SearchField GetSearchField(string fieldIdentifier, string applicationIdentifier);
    }
}