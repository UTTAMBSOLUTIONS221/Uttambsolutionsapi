using DBL.Entities;
using DBL.Helpers;
using DBL.Models;
using DBL.UOW;
using System.Text;

namespace DBL
{
    public class BL
    {

        private UnitOfWork db;
        private string _connString;
        static bool mailSent = false;
        Encryptdecrypt sec = new Encryptdecrypt();
        Stringgenerator str = new Stringgenerator();
        EmailSenderHelper emlsnd = new EmailSenderHelper();
        public BL(string connString)
        {
            this._connString = connString;
            db = new UnitOfWork(connString);
        }

        #region Send Subscription Email
        public Task<Genericmodel> Sendnewcustomersubscriptionemail(Newcustomersubscription Obj)
        {
            Genericmodel model = new Genericmodel();

            return Task.Run(() =>
            {
                var commtempdata = db.SettingsRepository.Getsystemcommunicationtemplatedatabyname(true, "Bookingademotemplate");
                if (commtempdata != null)
                {
                    StringBuilder StrBodyEmail = new StringBuilder(commtempdata.Templatebody);
                    StrBodyEmail.Replace("@CompanyLogo", commtempdata.Modulelogo);
                    StrBodyEmail.Replace("@CompanyName", commtempdata.Module);
                    StrBodyEmail.Replace("@CompanyEmail", commtempdata.Moduleemail);
                    StrBodyEmail.Replace("@Fullname", Obj.Fullname);
                    StrBodyEmail.Replace("@CurrentYear", DateTime.Now.Year.ToString());
                    string message = StrBodyEmail.ToString();
                    bool data = emlsnd.UttambsolutionssendemailAsync(Obj.Emailaddress, commtempdata.Templatesubject, message, true, "", "", "");
                    if (data)
                    {
                        model.RespStatus = 0;
                        model.RespMessage = "Email Sent";
                    }
                    else
                    {
                        model.RespStatus = 1;
                        model.RespMessage = "Email not Sent";
                    }
                }
                else
                {
                    model.RespStatus = 1;
                    model.RespMessage = "Template not found!";
                }
                return model;
            });
        }
        #endregion

    }
}
