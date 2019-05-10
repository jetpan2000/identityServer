namespace Octacom.Odiss.Core.Contracts.Settings
{
    public interface ISettingsService
    {
        Entities.Settings Get();
        void Save(Entities.Settings settings);
    }
}
