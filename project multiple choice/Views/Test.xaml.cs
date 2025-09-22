using project_multiple_choice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace project_multiple_choice.Views
{
    /// <summary>
    /// Interaction logic for Test.xaml
    /// </summary>
    public partial class Test : UserControl
    {
        ExamPackage exam;
        PartQuestion CurrentPart;
        private Dictionary<Question, Answer> selectedAnswers = new Dictionary<Question, Answer>();
        private Dictionary<PartQuestion, Dictionary<Question, Answer>> partQuestion = new Dictionary<PartQuestion, Dictionary<Question, Answer>>();
        bool IsDone = false;
        public Test()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is ViewModels.TestVM vm)
            {
                exam = vm.Exam;
            }
            AddPart();
            AddQuestionNumber(CurrentPart);
            AddMainQuestion(CurrentPart);
        }
        private void AddPart()
        {
            CurrentPart = exam.Parts.FirstOrDefault();
            foreach (var part in exam.Parts)
            {
                Button btn = new Button
                {
                    Content = part.Title,
                    Margin = new Thickness(5),
                    Padding = new Thickness(10),
                    FontSize = 16,
                    Width = 200,
                    Height = 50
                };
                btn.Click += (s, e) => SetCurrentPart(part);
                splistPartExam.Children.Add(btn);
            }
        }
        private void SetCurrentPart(PartQuestion part)
        {
            CurrentPart = part;
            AddQuestionNumber(CurrentPart);
            if (IsDone)
                AddMainQuestionAfterDone(CurrentPart);
            else
                AddMainQuestion(CurrentPart);
        }
        private void AddQuestionNumber(PartQuestion cpart)
        {
            wpListQuestionNumber.Children.Clear();
            foreach (var question in cpart.Questions)
            {
                Button btn = new Button
                {
                    Content = question.QuestionNumber,
                    Margin = new Thickness(5),
                    Padding = new Thickness(10),
                    FontSize = 16,
                    Width = 50,
                    Height = 50
                };
                wpListQuestionNumber.Children.Add(btn);
            }
        }
        private void AddMainQuestion(PartQuestion cPart)
        {
            spMainQuestion.Children.Clear();
            foreach (var question in cPart.Questions)
            {
                Border border = new Border();
                spMainQuestion.Children.Add(border);
                StackPanel stackPanel = new StackPanel();
                border.Child = stackPanel;
                TextBlock numQuestion = new TextBlock
                {
                    Text = $"Câu {question.QuestionNumber} (1 đáp án)",
                    FontSize = 12
                };
                stackPanel.Children.Add(numQuestion);
                TextBlock contentQuestion = new TextBlock
                {
                    Text = question.Content,
                    FontSize = 16,
                    FontWeight = FontWeights.SemiBold
                };
                stackPanel.Children.Add(contentQuestion);
                foreach (var answer in question.Answers)
                {
                    TextBlock textBlock = new TextBlock
                    {
                        Text = answer.Content,
                    };
                    RadioButton radioButton = new RadioButton
                    {
                        Content = textBlock,
                        Margin = new Thickness(0, 10, 0, 0),
                        GroupName = $"Question{question.QuestionNumber}",
                        Tag = new { Question = question, Answer = answer }
                    };
                    radioButton.Checked += (s, e) =>
                    {
                        var rb = s as RadioButton;
                        dynamic data = rb.Tag;

                        if (!partQuestion.ContainsKey(cPart))
                            partQuestion[cPart] = new Dictionary<Question, Answer>();

                        partQuestion[cPart][data.Question] = data.Answer;

                    };
                    if (partQuestion.ContainsKey(cPart) && partQuestion[cPart].ContainsKey(question))
                    {
                        if (partQuestion[cPart][question] == answer)
                        {
                            radioButton.IsChecked = true;
                        }
                    }
                    stackPanel.Children.Add(radioButton);
                }
            }
        }
        private void AddMainQuestionAfterDone(PartQuestion cPart)
        {
            spMainQuestion.Children.Clear();
            foreach (var question in cPart.Questions)
            {
                Border border = new Border();
                spMainQuestion.Children.Add(border);

                StackPanel stackPanel = new StackPanel();
                border.Child = stackPanel;

                TextBlock numQuestion = new TextBlock
                {
                    Text = $"Câu {question.QuestionNumber} (1 đáp án)",
                    FontSize = 12
                };
                stackPanel.Children.Add(numQuestion);

                TextBlock contentQuestion = new TextBlock
                {
                    Text = question.Content,
                    FontSize = 16,
                    FontWeight = FontWeights.SemiBold
                };
                stackPanel.Children.Add(contentQuestion);

                Answer selectedAnswer = null;
                if (partQuestion.ContainsKey(cPart) && partQuestion[cPart].ContainsKey(question))
                    selectedAnswer = partQuestion[cPart][question];

                foreach (var answer in question.Answers)
                {
                    TextBlock textBlock = new TextBlock
                    {
                        Text = answer.Content,
                        FontWeight = FontWeights.Bold
                    };

                    RadioButton radioButton = new RadioButton
                    {
                        Content = textBlock,
                        Margin = new Thickness(0, 10, 0, 0),
                        GroupName = $"Question{question.QuestionNumber}",
                        Tag = new { Question = question, Answer = answer },
                    };
                    radioButton.IsHitTestVisible = false;

                    if (selectedAnswer == answer)
                    {
                        radioButton.IsChecked = true;
                        if (answer.IsCorrect)
                            textBlock.Foreground = Brushes.Green;

                        else
                            textBlock.Foreground = Brushes.Red;
                    }
                    else if (answer.IsCorrect)
                    {
                        textBlock.Foreground = Brushes.Green;
                    }

                    stackPanel.Children.Add(radioButton);
                }
            }
        }


        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            int correctCount = 0;
            int totalCount = 0;
            foreach (var part in exam.Parts)
            {
                foreach (var question in part.Questions) 
                {
                    totalCount++; 

                    Answer selectedAnswer = null;

                    if (partQuestion.ContainsKey(part) && partQuestion[part].ContainsKey(question))
                    {
                        selectedAnswer = partQuestion[part][question];
                    }

                    var correctAnswer = question.Answers.FirstOrDefault(a => a.IsCorrect);

                    if (selectedAnswer != null && selectedAnswer == correctAnswer)
                    {
                        correctCount++;
                    }
                }
            }
            double scored = (totalCount > 0)
                            ? ((double)correctCount / totalCount) * 100
                            : 0;
            score.Text = scored.ToString();
            congratulations.Visibility = Visibility.Visible;
        }

        private void btnWatchAwser_Click(object sender, RoutedEventArgs e)
        {
            IsDone = true;
            AddMainQuestionAfterDone(CurrentPart);
            congratulations.Visibility = Visibility.Collapsed;
        }
    }
}
