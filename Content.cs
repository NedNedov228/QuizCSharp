using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Quiz
{
    internal class Content
    {
        private static Dictionary<string, User>? _users;

        private static Dictionary<string,List<Dictionary<string, int>>>? _userStats;

        public static void LoadUsersTxt(string path)
        {
            // Reads the file and loads the users into the _users dictionary

            // The file should be in the following format:
            // login password birthDate


         
            if (!File.Exists(path))            // If the file does not exist, create an empty file
                File.Create(path).Close();

            _users = new Dictionary<string, User>();

            string[] lines = File.ReadAllLines(path);

            foreach (string line in lines)
            {
                string[] parts = line.Split(' ');
                string login = parts[0];
                string password = parts[1];
                string birthDate = parts[2];

                User user = new User(login, password, birthDate);
                _users.Add(login, user);
            }
        }

        public static void SaveUsersTxt(string path)
        {
            // Saves the users from the _users dictionary to the file

            // The file should be in the following format:
            // login password birthDate

            if (_users == null)
                throw new InvalidOperationException("Users are not loaded");

            List<string> lines = new List<string>();

            foreach (var user in _users)
            {
                string line = $"{user.Value.Login} {user.Value.Password} {user.Value.BirthDate}";
                lines.Add(line);
            }

            File.WriteAllLines(path, lines);
        }


        public static void LoadUsers(string path) // using System.Text.Json;
        {
            // Reads the file and loads the users into the _users dictionary

            // The file should be in the following format:
            // [
            //     {
            //         "login": "login",
            //         "password": "password",
            //         "birthDate": "birthDate"
            //     },
            //     ...
            // ]

            _users = new Dictionary<string, User>();

            if (!File.Exists(path))            // If the file does not exist, create an empty file
                File.Create(path).Close();

            string json = File.ReadAllText(path);

            if (json == "")
                return;

            List<User>? users = System.Text.Json.JsonSerializer.Deserialize<List<User>>(json);
            
            if(users == null)
                return;

            foreach (var user in users)
            {
                _users.Add(user.Login, user);
            }
        }


        public static void SaveUsers(string path) 
        {

            if (_users == null)
                throw new InvalidOperationException("Users are not loaded");

            List<User> users = new List<User>();

            foreach (var user in _users)
            {
                users.Add(user.Value);
            }

            string json = System.Text.Json.JsonSerializer.Serialize(users);

            File.WriteAllText(path, json);
        }
   


        public static User? FindUser(string login)
        {
            if (_users == null)
                throw new InvalidOperationException("Users are not loaded");

            if (_users.ContainsKey(login))
                return _users[login];
            else
                return null;
        }

        public static void AddUser(string login, string password, string birthDate)
        {
            if (_users == null)
                throw new InvalidOperationException("Users are not loaded");

            if (_users.ContainsKey(login))
                throw new InvalidOperationException("User already exists");

            User user = new User(login, password, birthDate);
            _users.Add(login, user);
        }

        public static void AddUser(User user)
        {
            if (_users == null)
                throw new InvalidOperationException("Users are not loaded");

            if (_users.ContainsKey(user.Login))
                throw new InvalidOperationException("User already exists");

            _users.Add(user.Login, user);
        }

        public static void LoadStats(string path)
        {
            // Reads the file and loads the stats into the _userStats dictionary

            // The file should be in the following format:
            // {
            //     "login": [
            //         {
            //             "category": 1,
            //             "correctAnswers": 2
            //         },
            //         ...
            //     ],
            //     ...
            // }

            _userStats = new Dictionary<string, List<Dictionary<string, int>>>();

            if (!File.Exists(path))            // If the file does not exist, create an empty file
                File.Create(path).Close();

            string json = File.ReadAllText(path);

            if (json == "")
                return;

            _userStats = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, List<Dictionary<string, int>>>>(json);
        }
        
        public static void SaveStats(string path)
        {
            // Saves the stats from the _userStats dictionary to the file

            // The file should be in the following format:
            // {
            //     "login": [
            //         {
            //             "category": 1,
            //             "correctAnswers": 2
            //         },
            //         ...
            //     ],
            //     ...
            // }

            if (_userStats == null)
                throw new InvalidOperationException("Stats are not loaded");

            string json = System.Text.Json.JsonSerializer.Serialize(_userStats);

            File.WriteAllText(path, json);
        }

        public static List<Dictionary<string, int>>? GetUserStats(string login)
        {
            if (_userStats == null)
                throw new InvalidOperationException("Stats are not loaded");

            if (_userStats.ContainsKey(login))
                return _userStats[login];
            else
                return null;
        }

        public static void AddUserStat(string login, string category, int correctAnswers)
        {
            if (_userStats == null)
                throw new InvalidOperationException("Stats are not loaded");

            if (!_userStats.ContainsKey(login))
                _userStats.Add(login, new List<Dictionary<string, int>>());

            Dictionary<string, int> stat = new Dictionary<string, int>();
            stat.Add(category, correctAnswers);

            _userStats[login].Add(stat);

            if (_users != null)
                _users[login].OverallScore += correctAnswers;

        }

        public static List<User>? GetLeaders()
        {
            if (_users == null)
                throw new InvalidOperationException("Users are not loaded");

            List<User> leaders = _users.Values.ToList();
            leaders.Sort((x, y) => y.OverallScore.CompareTo(x.OverallScore));

            return leaders;
        }

    }
}
