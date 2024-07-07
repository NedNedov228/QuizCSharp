using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz
{
    internal class Menu
    {
        private string _name { get; set; }
        private string _contents { get; set; }
        public Menu(string contents)
        {
            _name = "Unnamed";
            _contents = contents;
        }

        public Menu(string name,string contents)
        {
            _name = name;
            _contents = contents;
        }

        public string Name { get { return _name; } }

        public void Show()
        {
            Console.WriteLine(_contents);
        }
    }
}
