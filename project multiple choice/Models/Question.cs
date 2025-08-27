using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_multiple_choice.Models
{
    class Question
    {
        public int QuestionNumber { get; set; }
        public string Content { get; set; }
        public List<Answer> Answers { get; set; } = new();

    }
}
