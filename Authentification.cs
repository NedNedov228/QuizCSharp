using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz
{
    internal class Authentification
    {

        public static bool Login(string login, string password)
        {
            User? user = Content.FindUser(login);
            if ( user == null)
            {
                return false;
            }
            if (user.Login == login)
            {
                return Utils.VerifyPassword(password, user.Password);
            }
            return false;
        }
        
        public static bool Register(string login, string password, string birthDate)
        {
            User? user = Content.FindUser(login);
            if (user != null)
            {
                return false;
            }
            User newUser = new User(login, Utils.HashPassword(password), birthDate);
            Content.AddUser(newUser);
            return true;
        }

        
    }
}
