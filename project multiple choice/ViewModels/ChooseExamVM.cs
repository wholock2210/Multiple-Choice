using project_multiple_choice.Models;
using project_multiple_choice.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace project_multiple_choice.ViewModels
{
    class ChooseExamVM : BaseNotify
    {
        private ObservableCollection<ExamPackage> exams;
        public ObservableCollection<ExamPackage> Exams
        {
            get => exams;
            set { exams = value; OnPropertyChanged(); }
        }
        private ExamPackage selectedExam;
        public ExamPackage SelectedExam
        {
            get => selectedExam;
            set { selectedExam = value; OnPropertyChanged(); }
        }

        public ChooseExamVM()
        {
            LoadExams();
        }


        private void LoadExams()
        {
            Exams = new ObservableCollection<ExamPackage>();

            string folderPath = "Exam"; 
            if (!Directory.Exists(folderPath)) return;

            foreach (var file in Directory.GetFiles(folderPath, "*.json"))
            {
                string json = File.ReadAllText(file);
                ExamPackage? examPackage = JsonSerializer.Deserialize<ExamPackage>(json);
                if (examPackage != null)
                {
                    Exams.Add(examPackage);
                }
            }
        }
    }
}
