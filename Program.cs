using System.Reflection;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Quiz
{
    internal class Program
    {
        static string? error = null;
        static string? success = null;

        static bool isAuthed = false;

        static Menu? currentMenu;
        static User? currentUser;

        #region Methods

        static void LoginAction()
        {
            Console.Write("Enter login: ");
            string? login = Console.ReadLine();
            Console.Write("Enter password: ");
            string? password = Console.ReadLine();
            if (Authentification.Login(login, password))
            {
                currentUser = Content.FindUser(login);
                isAuthed = true;
                success = "You are logged in";
                Console.Clear();
            }
            else
            {
                error = "Invalid login or password";
                Console.Clear();
            }
        }

        static void RegisterAction()
        {
            Console.Write("Enter login: ");
            string? newLogin = Console.ReadLine();
            Console.Write("Enter the password: ");
            string? newPassword = Console.ReadLine();
            
            while (!Utils.isPasswordValid(newPassword))
            {

                Console.Write("Enter the valid password: ");
                newPassword = Console.ReadLine();
                
            }
            Console.WriteLine("Enter birthdate: ");
            string? birthDate = Console.ReadLine();
            if (Authentification.Register(newLogin, newPassword, birthDate))
            {
                success = "User created!";
                Console.Clear();
            }
            else
            {
                error = "User already exists!";
                Console.Clear();
            }
        }

        static void StartQuizAction(List<Quiz> quizes)
        {
            Quiz quiz; 
            Console.WriteLine("Choose a quiz:");
            while (true)
            {
                Utils.DisplayMessages(ref error, ref success);


                for (var i = 0; i < quizes.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {quizes[i].Category}");
                }
                int choice;
                try
                {
                    choice = Convert.ToInt32(Console.ReadLine());
                }
                catch (FormatException)
                {
                    choice = 0;
                }
                if (choice > 0 && choice <= quizes.Count)
                {
                    quiz = quizes[choice - 1];
                    break;
                }
                else
                {
                    error = "Invalid choice";
                }
                Console.Clear();
            }


            var score = quiz.Start();
            if (currentUser != null)
                Content.AddUserStat(currentUser.Login, quiz.Category, score);
           
        }

        static void ShowStatsAction()
        {
            List<Dictionary<string, int>>? stats; 
            if (currentUser != null)
            {
                Console.WriteLine("Your score: "+currentUser.OverallScore+"\n");
                stats = Content.GetUserStats(currentUser.Login);

                if (stats != null)
                {
                    foreach (var stat in stats)
                    {
                        foreach (var item in stat)
                        {
                            Console.WriteLine($"{item.Key}: {item.Value}/5");
                        }
                    }
                }
                else error = "No stats found";
            }
            else error = "User unauthorized";

            Console.WriteLine(
                "Press any key to continue...");
            Console.ReadKey();
            Console.Clear();
        }

        static void ShowLeadersAction()
        {
            Console.WriteLine("Top 20 Leaders:");
            List<User>? leaders = Content.GetLeaders();
            if (leaders != null)
            {
                var leadersCount = leaders.Count > 20 ? 20 : leaders.Count;
                for (var i = 0; i < leadersCount ;i++)
                {
                    Console.WriteLine($"{leaders[i].Login}: {leaders[i].OverallScore}");
                }
            }
            else error = "No leaders found";

            if (currentUser!=null)
                Console.WriteLine("Your Score: " + Content.FindUser(currentUser.Login)?.OverallScore);

            Console.WriteLine(
                               "Press any key to continue...");
            Console.ReadKey();
            Console.Clear();
        }

        #endregion

        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit);

            Console.WriteLine("Loading users....");
            Content.LoadStats("stats.json");
            Content.LoadUsers("users.json");
            Console.WriteLine("Loaded");

            Console.WriteLine("Initializing Menus....");

            Menu mainMenu = new Menu("1. Login\n2. Register\n3. Exit");
            Menu authedMenu = new Menu("Authed Menu", "1. Start Quiz\n2. Show Leaders\n3. Show Stats\n4. Logout");
            
            currentMenu = mainMenu;
            Console.WriteLine("Menus initialized");

            List<Quiz> quizes = new List<Quiz>();

            Quiz quiz = new Quiz("Capitals");
            quiz.AddQuestion(new Question("What is the capital of USA?", new string[] { "Washington", "New York", "Los Angeles", "Chicago" }, "Washington", "Capitals"));
            quiz.AddQuestion(new Question("What is the capital of France?", new string[] { "Paris", "Marseille", "Lyon", "Toulouse" }, "Paris", "Capitals"));
            quiz.AddQuestion(new Question("What is the capital of Germany?", new string[] { "Berlin", "Hamburg", "Munich", "Cologne" }, "Berlin", "Capitals"));
            quiz.AddQuestion(new Question("What is the capital of Russia?", new string[] { "Moscow", "Saint Petersburg", "Novosibirsk", "Yekaterinburg" }, "Moscow", "Capitals"));
            quiz.AddQuestion(new Question("What is the capital of China?", new string[] { "Beijing", "Shanghai", "Guangzhou", "Shenzhen" }, "Beijing", "Capitals"));
            quizes.Add(quiz);
            

            Quiz quiz1 = new Quiz("Math");
            quiz1.AddQuestion(new Question("2 + 2", new string[] { "4", "5", "6", "7" }, "4", "Math"));
            quiz1.AddQuestion(new Question("2 * 2", new string[] { "4", "5", "6", "7" }, "4", "Math"));
            quiz1.AddQuestion(new Question("2 - 2", new string[] { "4", "5", "6", "7" }, "0", "Math"));
            quiz1.AddQuestion(new Question("2 / 2", new string[] { "4", "5", "6", "7" }, "1", "Math"));
            quiz1.AddQuestion(new Question("Lim(x->49)(5! * sqrt(x))", new string[] { "840", "5", "6", "7" }, "840", "Math"));
            quizes.Add(quiz1);

            Quiz quiz2 = new Quiz("History");
            quiz2.AddQuestion(new Question("When was the Declaration of Independence signed?", new string[] { "1776", "1789", "1799", "1800" }, "1776", "History"));
            quiz2.AddQuestion(new Question("When did the French Revolution start?", new string[] { "1789", "1799", "1800", "1812" }, "1789", "History"));
            quiz2.AddQuestion(new Question("When did the World War 2 start?", new string[] { "1939", "1940", "1941", "1942" }, "1939", "History"));
            quiz2.AddQuestion(new Question("When did the World War 1 start?", new string[] { "1914", "1915", "1916", "1917" }, "1914", "History"));
            quiz2.AddQuestion(new Question("When did the Cold War start?", new string[] { "1947", "1948", "1949", "1950" }, "1947", "History"));
            quizes.Add(quiz2);

            Quiz quiz3 = new Quiz("Programming");
            quiz3.AddQuestion(new Question("What is the most popular programming language?", new string[] { "Java", "C#", "Python", "JavaScript" }, "Java", "Programming"));
            quiz3.AddQuestion(new Question("What is the most popular IDE?", new string[] { "Visual Studio", "IntelliJ IDEA", "PyCharm", "Eclipse" }, "Visual Studio", "Programming"));
            quiz3.AddQuestion(new Question("What is the most popular OS?", new string[] { "Windows", "Linux", "MacOS", "Android" }, "Windows", "Programming"));
            quiz3.AddQuestion(new Question("What is the most popular DBMS?", new string[] { "MySQL", "PostgreSQL", "SQLite", "MongoDB" }, "MySQL", "Programming"));
            quiz3.AddQuestion(new Question("What is the most popular VCS?", new string[] { "Git", "Mercurial", "SVN", "Perforce" }, "Git", "Programming"));
            quizes.Add(quiz3);

            Console.WriteLine("Quizzes initialized");

            Console.Clear();
            // MainLoop

            while (true)
            {


                currentMenu.Show();

                Utils.DisplayMessages(ref error, ref success);
                Utils.ResetMessages(ref error, ref success);
                int choice;
                try
                {
                    choice = Convert.ToInt32(Console.ReadLine());
                }
                catch (FormatException)
                {
                    choice = 0;
                }
                Console.Clear();


                switch (choice)
                {
                    case 1:
                        if (isAuthed)
                        {
                            StartQuizAction(quizes);
                            break;
                        }
                        else
                        {
                            LoginAction();
                            currentMenu = authedMenu;
                        }

                        break;
                    case 2:
                        if (isAuthed)
                            ShowLeadersAction();

                        else RegisterAction();
                        break;
                    case 3:
                        if (isAuthed)
                        {
                            if (currentUser!=null)
                                ShowStatsAction();
                            else error = "User unauthorized";

                            break;
                        }
                        else return;

                    case 4:
                        if (isAuthed)
                        {
                            isAuthed = false;
                            currentMenu = mainMenu;
                            success = "You are logged out";
                        }
                        else error = "Invalid choice";
                        break;
                    default:
                        error = "Invalid choice";
                        break;
                }

            }

        }

        static void OnProcessExit(object? sender, EventArgs e)
        {
            Content.SaveUsers("users.json");
            Content.SaveStats("stats.json");
        }


    }
}