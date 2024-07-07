using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz
{
    [Serializable]
    internal class User
    {
        private string _login { get; set; }
        private string _password { get; set; }
        private string _birthDate { get; set; }
        private int _overallScore { get; set; }

        public User(string login, string password, string birthdate)
        {
            _login = login;
            _password = password;
            _birthDate = birthdate;
            _overallScore = 0;
        }

        
        public string Login
        {
            get { return _login; }
            set { _login = value; }
        }
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }
        public string BirthDate
        {
            get { return _birthDate; }
            set { _birthDate = value; }
        }

        public int OverallScore 
        {
            get { return _overallScore; }
            set { _overallScore = value; }
        }

        /*public string GetStats()
        {
            return "Stats ;)";
        }*/

    }
}
