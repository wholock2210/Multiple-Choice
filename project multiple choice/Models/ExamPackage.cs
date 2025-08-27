using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_multiple_choice.Models
{
    class ExamPackage
    {
        public Exam ExamInfo { get; set; }
        public List<PartQuestion> Parts { get; set; }
    }
}
