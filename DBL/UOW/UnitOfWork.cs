using DBL.Repositories;

namespace DBL.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private string connString;
        private bool _disposed;

       
        private ISettingsRepository settingsRepository;
        private IAuthRepository authRepository;
       
        public UnitOfWork(string connectionString) => connString = connectionString;
        public ISettingsRepository SettingsRepository
        {
            get { return settingsRepository ?? (settingsRepository = new SettingsRepository(connString)); }
        }
        public IAuthRepository AuthRepository
        {
            get { return authRepository ?? (authRepository = new AuthRepository(connString)); }
        }

        public void Reset()
        {
            settingsRepository = null;
            authRepository = null;
        }

        public void Dispose()
        {
            dispose(true);
            GC.SuppressFinalize(this);
        }

        private void dispose(bool disposing)
        {
            if (!_disposed)
            {
                _disposed = true;
            }
        }

        ~UnitOfWork()
        {
            dispose(false);
        }
    }
}
