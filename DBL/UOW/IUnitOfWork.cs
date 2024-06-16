using DBL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBL.UOW
{
    public interface IUnitOfWork
    {
        ISettingsRepository SettingsRepository { get; }
        IAuthRepository AuthRepository { get; }
    }
}
