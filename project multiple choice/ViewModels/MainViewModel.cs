using project_multiple_choice.Models;
using project_multiple_choice.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace project_multiple_choice.ViewModels
{
    class MainViewModel : BaseNotify
    {
        private object _CurrentView;
        public object CurrentView
        {
            get { return _CurrentView; }
            set { _CurrentView = value; OnPropertyChanged(); }
        }

        public ICommand ShutDownCommand { get; set; }
        public ICommand HomeCommand { get; set; }
        public ICommand SettingCommand { get; set; }
        public ICommand InfoCommand { get; set; }
        public ICommand DashboardCommand { get; set; }
        public ICommand TextCommand { get; set; }
        public ICommand ChooseExamCommand { get; set; }
        public ICommand TestCommand { get; set; }
        public MainViewModel()
        {
            ShutDownCommand = new RelayCommand(ShutDownApp);
            HomeCommand = new RelayCommand(Home);
            SettingCommand = new RelayCommand(Setting);
            InfoCommand = new RelayCommand(Info);
            DashboardCommand = new RelayCommand(Dashboard);
            TextCommand = new RelayCommand(Text);
            ChooseExamCommand = new RelayCommand(ChooseExam);
            TestCommand = new RelayCommand(Test);
            CurrentView = new HomeVM();
            
        }

        private void ShutDownApp(object obj) { 
            Application.Current.Shutdown();
        }
        private void Home(object obj) => CurrentView = new HomeVM();
        private void Setting(object obj) => CurrentView = new SettingVM();
        private void Info(object obj) => CurrentView = new InfoVM();
        private void Dashboard(object obj) => CurrentView = new DashboardVM();
        private void Text(object obj) => CurrentView = new TextVM();
        private void ChooseExam(object obj) => CurrentView = new ChooseExamVM();

        private void Test(object obj)
        {
            if(obj is ExamPackage exam)
            {
                CurrentView = new TestVM(exam);
            }
            
        }

    }
}
