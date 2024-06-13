using DBL.Models;

namespace DBL.Repositories
{
    public interface ISettingsRepository
    {
        CommunicationTemplateModel Getsystemcommunicationtemplatedatabyname(bool Isemail, string Templatename);
    }
}
