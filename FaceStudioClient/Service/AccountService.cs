using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceStudioClient.Service
{
    public class AccountService : BaseService
    {
        public event Action OnRegisterCompleted;
        public event Action OnLogonCompleted;
        public event Action OnLogoutCompleted;

        public void Register(string username, string email, string password, string confirmpwd, Action<Exception> failFunc)
        {
            string url = "http://localhost:8888/api/account/register";
            var para = new
            {
                UserName=username,
                Email = email,
                Password = password,
                ConfirmPassword = confirmpwd,
            };
            web.AsyncJsonPost(url, para, 
                (response, cookie, exp) => {
                    if(null != exp)
                    {
                        failFunc(exp);
                    }
                    else
                    {
                        if(OnRegisterCompleted != null)
                            OnRegisterCompleted();
                    }
                });
        }

        public void Logon(string username, string password, Action<Exception> failFunc)
        {
            string url = "http://localhost:8888/api/account/logon";
            var para = new
            {
                UserName = username,
                Password = password,
            };
            web.AsyncJsonPost(url, para,
                (response, cookie, exp) => {
                    if (null != exp)
                    {
                        failFunc(exp);
                    }
                    else
                    {
                        if (OnLogonCompleted != null)
                            OnLogonCompleted();
                    }
                });
        }

        public void Logoff(Action<Exception> failFunc)
        {
            string url = "http://localhost:8888/api/account/Logout";
            web.AsyncJsonPost<string>(url, null,
                (response, cookie, exp) => {
                    if (null != exp)
                    {
                        failFunc(exp);
                    }
                    else
                    {
                        if (OnLogoutCompleted != null)
                            OnLogoutCompleted();
                    }
                });
        }
    }
}
