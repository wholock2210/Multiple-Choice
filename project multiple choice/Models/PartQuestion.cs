using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_multiple_choice.Models
{
    class PartQuestion
    {
        public string Title { get; set; }
        public List<Question> Questions { get; set; } = new();
    }
}
