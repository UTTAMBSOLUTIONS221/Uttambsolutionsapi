using DBL.Models;

namespace DBL.Repositories
{
    public interface IAuthRepository
    {
        UsermodelResponce VerifySystemStaff(string Username);
    }
}
