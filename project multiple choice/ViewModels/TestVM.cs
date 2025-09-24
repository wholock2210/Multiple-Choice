using project_multiple_choice.Models;
using project_multiple_choice.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
        private bool randomizeQuestions;
        public bool RandomizeQuestions
        {
            get => randomizeQuestions;
            set { randomizeQuestions = value; OnPropertyChanged(); }
        }
        public TestVM(ExamPackage obj,bool random)
        {
            Exam = obj;
            randomizeQuestions = random;
            
        }   
    }
}
