using DBL.Repositories;

namespace DBL.UOW
{
    public interface IUnitOfWork
    {
        ISettingsRepository SettingsRepository { get; }
        IAuthRepository AuthRepository { get; }
        IModulesRepository ModulesRepository { get; }
    }
}
