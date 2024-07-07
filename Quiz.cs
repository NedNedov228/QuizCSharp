using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Quiz
{
    internal class Quiz
    {
        private List<Question> _questions;
        private int _currentQuestionIndex;
        private int _correctAnswers;

        private string? _error;
        private string? _success;


        private string _category;

        public int QuestionCount { get { return _questions.Count; } }
        public int CorrectAnswers { get { return _correctAnswers; } }

        public string Category { get { return _category; } }

        public Quiz()
        {
            _category = "General";
            _questions = new List<Question>();
        }

        public Quiz(string name)
        {
            _category = name;
            _questions = new List<Question>();
        }

        public void AddQuestion(Question question)
        {
            _questions.Add(question);
        }

        public Question? GetNextQuestion()
        {
            if (_currentQuestionIndex < _questions.Count)
            {
                return _questions[_currentQuestionIndex++];
            }
            return null;
        }

        public Question? GetPrevQuestion()
        {
            if (_currentQuestionIndex > 0)
            {
                return _questions[--_currentQuestionIndex];
            }
            return null;
        }

        public void AnswerCurrentQuestion(string answer)
        {
            if (_currentQuestionIndex >= 0 && _currentQuestionIndex < _questions.Count)
            {
                

                if (_questions[_currentQuestionIndex].Answer == answer)
                {
                    _correctAnswers++;
                }
                _currentQuestionIndex++;
            }
            else Console.WriteLine("Out of range");
        }

        public void Reset()
        {
            _currentQuestionIndex = 0;
        }

        public int Start()
        {
            // Start the quiz
            _currentQuestionIndex = 0;
            _correctAnswers = 0;

            while (_currentQuestionIndex < _questions.Count)
            {
                Console.Clear();
                Utils.DisplayMessages(ref _error, ref _success);

                Question question = _questions[_currentQuestionIndex];
                var opts = question.Options;
                Console.WriteLine(question.QuestionMessage);
                for (int i = 0; i < opts.Length; i++)
                {
                    Console.WriteLine($"{i + 1}. {opts[i]}");
                }
                int choise = 0;
                try
                {
                    choise = Convert.ToInt32(Console.ReadLine());
                }catch (Exception e)
                {
                    _error = e.Message;
                }

                Console.WriteLine(choise);
                if (choise > 0 && choise <= opts.Length)
                {
                    AnswerCurrentQuestion(opts[choise - 1]);
                }
                else
                {
                    _error = "Invalid option";
                }

            }

            
            Console.Clear();
            Console.WriteLine(
                $"\nYou have answered {_correctAnswers} questions correctly\n\n" +
                "Press Any button to go to main menu"
                );

            Console.ReadKey();
            Console.Clear();

            return _correctAnswers;
        }
        
    }
}
