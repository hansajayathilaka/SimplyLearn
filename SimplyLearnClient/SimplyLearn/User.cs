using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyLearn
{
    public class User
    {

        public string Username { get; set; }
        public string Password { get; set; }

        public bool Login(IRepository repository)
        {
            bool loginStatus = false;
            try
            {
                loginStatus = repository.LoginUser(this);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return loginStatus;
        }

    }
}
