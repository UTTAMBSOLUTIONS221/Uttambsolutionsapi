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
                    StrBodyEmail.Replace("@Fullname", Obj.Fullname);
                    StrBodyEmail.Replace("@PhoneNumber", Obj.PhoneNumber);
                    StrBodyEmail.Replace("@Emailaddress", Obj.Emailaddress);
                    StrBodyEmail.Replace("@Module", Obj.Module);
                    StrBodyEmail.Replace("@CurrentYear", DateTime.Now.Year.ToString());
                    string message = StrBodyEmail.ToString();
                    bool data = emlsnd.UttambsolutionssendemailAsync("fkingori@uttambsolutions.com", commtempdata.Templatesubject, Obj.Message, true, "", "", "");
                    if (data)
                    {
                        var commtemprespdata = db.SettingsRepository.Getsystemcommunicationtemplatedatabyname(true, "Bookingademotemplate");
                        if (commtemprespdata != null)
                        {
                            StringBuilder StrBodyEmailresp = new StringBuilder(commtempdata.Templatebody);
                            StrBodyEmailresp.Replace("@Fullname", Obj.Fullname);
                            StrBodyEmailresp.Replace("@PhoneNumber", Obj.PhoneNumber);
                            StrBodyEmailresp.Replace("@Emailaddress", Obj.Emailaddress);
                            StrBodyEmailresp.Replace("@Module", Obj.Module);
                            StrBodyEmailresp.Replace("@CurrentYear", DateTime.Now.Year.ToString());
                            string messageresp = StrBodyEmailresp.ToString();
                            bool data1 = emlsnd.UttambsolutionssendemailAsync(Obj.Emailaddress, commtempdata.Templatesubject, Obj.Message, true, "", "", "");
                            if (data1)
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

        public Task<Genericmodel> Sendnewcustomersubscriptionrespemail(Newcustomersubscription Obj)
        {
            Genericmodel model = new Genericmodel();

            return Task.Run(() =>
            {
                var commtempdata = db.SettingsRepository.Getsystemcommunicationtemplatedatabyname(true, "Bookingademoresptemplate");
                if (commtempdata != null)
                {
                    StringBuilder StrBodyEmail = new StringBuilder(commtempdata.Templatebody);
                    StrBodyEmail.Replace("@Fullname", Obj.Fullname);
                    StrBodyEmail.Replace("@PhoneNumber", Obj.PhoneNumber);
                    StrBodyEmail.Replace("@Emailaddress", Obj.Emailaddress);
                    StrBodyEmail.Replace("@Module", Obj.Module);
                    StrBodyEmail.Replace("@CurrentYear", DateTime.Now.Year.ToString());
                    string message = StrBodyEmail.ToString();
                    bool data = emlsnd.UttambsolutionssendemailAsync(Obj.Emailaddress, commtempdata.Templatesubject, Obj.Message, true, "", "", "");
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
