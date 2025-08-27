using project_multiple_choice.Models;
using project_multiple_choice.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_multiple_choice.ViewModels
{
    class TestVM : BaseNotify
    {
        private ExamPackage exam;
        public ExamPackage Exam
        {
            get => exam;
            set { exam = value; OnPropertyChanged(); }
        }
        public TestVM(ExamPackage obj)
        {
            Exam = obj;
        }   
    }
}
