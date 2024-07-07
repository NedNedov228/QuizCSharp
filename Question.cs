using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz
{
    internal class Question
    {
        private string _question { get; set; }
        private string _answer { get; set; }
        private string _category { get; set; }

        string[] _options;

        public Question(string question, string[] options, string answer, string category)
        {
            _question = question;
            _answer = answer;
            _category = category;
            _options = options;
        }

        
        public string QuestionMessage
        {
            get { return _question; }
            set { _question = value; }
        }
        public string Answer
        {
            get { return _answer; }
            set { _answer = value; }
        }
        public string Category
        {
            get { return _category; }
            set { _category = value; }
        }

        public string[] Options
        {
            get { return _options; }
            set { _options = value; }
        }
    }
}
