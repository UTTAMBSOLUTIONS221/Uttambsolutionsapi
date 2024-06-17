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

        #region Verify and Validate System Staff
        public Task<UsermodelResponce> ValidateSystemStaff(string userName, string password)
        {
            return Task.Run(async () =>
            {
                UsermodelResponce userModel = new UsermodelResponce { };
                var resp = db.AuthRepository.VerifySystemStaff(userName);
                if (resp.RespStatus == 0)
                {
                    Encryptdecrypt sec = new Encryptdecrypt();
                    string descpass = sec.Decrypt(resp.Usermodel.Passwords, resp.Usermodel.Passharsh);
                    if (password == descpass)
                    {
                        userModel = new UsermodelResponce
                        {
                            RespStatus = resp.RespStatus,
                            RespMessage = resp.RespMessage,
                            Token = "",
                            Usermodel = new UsermodeldataResponce
                            {
                                Userid = resp.Usermodel.Userid,
                                Firstname = resp.Usermodel.Firstname,
                                Fullname = resp.Usermodel.Fullname,
                                Phonenumber = resp.Usermodel.Phonenumber,
                                Username = resp.Usermodel.Username,
                                Emailaddress = resp.Usermodel.Emailaddress,
                                Roleid = resp.Usermodel.Roleid,
                                Rolename = resp.Usermodel.Rolename,
                                Passharsh = resp.Usermodel.Passharsh,
                                Passwords = resp.Usermodel.Passwords,
                                Isactive = resp.Usermodel.Isactive,
                                Isdeleted = resp.Usermodel.Isdeleted,
                                Loginstatus = resp.Usermodel.Loginstatus,
                                Passwordresetdate = resp.Usermodel.Passwordresetdate,
                                Createdby = resp.Usermodel.Createdby,
                                Modifiedby = resp.Usermodel.Modifiedby,
                                Lastlogin = resp.Usermodel.Lastlogin,
                                Datemodified = resp.Usermodel.Datemodified,
                                Datecreated = resp.Usermodel.Datecreated,
                            }
                        };
                        return userModel;
                    }
                    else
                    {
                        userModel.RespStatus = 1;
                        userModel.RespMessage = "Incorrect Username or Password";
                    }
                }
                else
                {
                    userModel.RespStatus = 1;
                    userModel.RespMessage = resp.RespMessage;
                }
                return userModel;
            });
        }
        #endregion

        #region System Modules
        public Task<IEnumerable<Systemmodule>> Getsystemmoduledata()
        {
            return Task.Run(() =>
            {
                var Resp = db.ModulesRepository.Getsystemmoduledata();
                return Resp;
            });
        }
        #endregion

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
                    bool data = emlsnd.UttambsolutionssendemailAsync("fkingori@uttambsolutions.com", commtempdata.Templatesubject, message, true, "", "", "");
                    if (data)
                    {
                        var commtemprespdata = db.SettingsRepository.Getsystemcommunicationtemplatedatabyname(true, "Bookingademoresptemplate");
                        if (commtemprespdata != null)
                        {
                            StringBuilder StrBodyEmailresp = new StringBuilder(commtemprespdata.Templatebody);
                            StrBodyEmailresp.Replace("@Fullname", Obj.Fullname);
                            StrBodyEmailresp.Replace("@PhoneNumber", Obj.PhoneNumber);
                            StrBodyEmailresp.Replace("@Emailaddress", Obj.Emailaddress);
                            StrBodyEmailresp.Replace("@Module", Obj.Module);
                            StrBodyEmailresp.Replace("@CurrentYear", DateTime.Now.Year.ToString());
                            string messageresp = StrBodyEmailresp.ToString();
                            bool data1 = emlsnd.UttambsolutionssendemailAsync(Obj.Emailaddress, commtemprespdata.Templatesubject, messageresp, true, "", "", "");
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
