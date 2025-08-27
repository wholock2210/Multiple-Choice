using project_multiple_choice.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media;
using System.Text.Json;
using project_multiple_choice.ViewModels;

namespace project_multiple_choice.Views
{
    /// <summary>
    /// Interaction logic for Text.xaml
    /// </summary>
    public partial class Text : UserControl
    {
        private string CurentPart = string.Empty;
        private string CurrentQuestionNumber = string.Empty;
        private List<PartQuestion> parts = new List<PartQuestion>();
        bool isFirstTime = false;
        public Text()
        {
            InitializeComponent();
        }
        private void UpdatePreviewPart()
        {
            if (parts.Count == 0)
                return;
            wpPart.Children.Clear();
            int partNumber = 1;
            foreach (PartQuestion part in parts)
            {
                Button btn = new Button()
                {
                    Content = part.Title,
                    Name = $"btnPart{partNumber}",
                };
                btn.Click += (s, e) =>
                {
                    UpdatePreviewQuestionNumber(part);
                };
                wpPart.Children.Add(btn);
                partNumber++;
                UpdatePreviewQuestionNumber(part);
            }
            if (String.IsNullOrEmpty(CurentPart) && isFirstTime)
            {
                CurentPart = parts[0].Title;
                UpdatePreviewQuestionNumber(parts[0]);
            }
        }
        private void UpdatePreviewQuestionNumber(PartQuestion part)
        {
            if(part.Questions.Count == 0)
                return;
            wpQuestion.Children.Clear();
            CurentPart = part.Title;
            int questionNumber = 1;
            foreach (Question question in part.Questions)
            {
                Button btn = new Button()
                {
                    Content = questionNumber,
                    Name = $"btn{questionNumber}",
                };
                btn.Click += (s, e) =>
                {
                    UpdatePreviewQuestion(question,question.QuestionNumber);
                };
                wpQuestion.Children.Add(btn);
                questionNumber++;
            }
            if(String.IsNullOrEmpty(CurrentQuestionNumber))
            {
                CurrentQuestionNumber = "1";
                UpdatePreviewQuestion(part.Questions[0], 1);
            }
        }

        private void UpdatePreviewQuestion(Question question, int questionNumber = 1)
        {
            CurrentQuestionNumber = questionNumber.ToString();
            CurrentNumberQuestion.Text = $"câu {questionNumber} (1 đáp án)";
            CurentQuestionTitle.Text = question.Content;
            int AnswerNumber = 1;
            string path = AppDomain.CurrentDomain.BaseDirectory;
            string imageFolder = Path.Combine(path, "Resources");
            string WrongImagePath = Path.Combine(imageFolder, "wrong.png");
            string RightImagePath = Path.Combine(imageFolder, "check.png");
            foreach (var answer in question.Answers)
            {
                if (AnswerNumber > 4)
                    break;
                if (AnswerNumber == 1)
                {
                    tQA.Text = answer.Content;
                    if (answer.IsCorrect) {
                        iQA.Source = new BitmapImage(new Uri($"{RightImagePath}", UriKind.Absolute));
                    }
                    else{
                        iQA.Source = new BitmapImage(new Uri($"{WrongImagePath}", UriKind.Absolute));
                    }
                }
                else if (AnswerNumber == 2)
                {
                    tQB.Text = answer.Content;
                    if (answer.IsCorrect)
                    {
                        iQB.Source = new BitmapImage(new Uri($"{RightImagePath}", UriKind.Absolute));
                    }
                    else
                    {
                        iQB.Source = new BitmapImage(new Uri($"{WrongImagePath}", UriKind.Absolute));
                    }
                }
                else if (AnswerNumber == 3)
                {
                    tQC.Text = answer.Content;
                    if (answer.IsCorrect)
                    {
                        iQC.Source = new BitmapImage(new Uri($"{RightImagePath}", UriKind.Absolute));
                    }
                    else
                    {
                        iQC.Source = new BitmapImage(new Uri($"{WrongImagePath}", UriKind.Absolute));
                    }
                }
                else if (AnswerNumber == 4)
                {
                    tQD.Text = answer.Content;
                    if (answer.IsCorrect)
                    {
                        iQD.Source = new BitmapImage(new Uri($"{RightImagePath}", UriKind.Absolute));
                    }
                    else
                    {
                        iQD.Source = new BitmapImage(new Uri($"{WrongImagePath}", UriKind.Absolute));
                    }
                }
                AnswerNumber++;
            }
        }

        private void inputBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var text = inputBox.Text;
            bool hasLetterOrDigit = Regex.IsMatch(text, @"[A-Za-z0-9]");
            if (hasLetterOrDigit)
            {
                Empty.Visibility = Visibility.Collapsed;
            }
            else
            {
                Empty.Visibility = Visibility.Visible;
            }
            var lines = inputBox.Text.Split(new[] { "\r\n", "\n" , "\r" },StringSplitOptions.None);
            
            PartQuestion currentPart = null;
            Question currentQuestion = null;
            parts.Clear();
            if (!lines[0].StartsWith("'"))
            {
                currentPart = new PartQuestion { Title = "Phần 1" };
                parts.Add(currentPart);
            }
            foreach (var rawLine in lines)
            {
                var line = rawLine.Trim();
                if (string.IsNullOrEmpty(line))
                {
                    if (currentQuestion != null && currentPart != null)
                    {
                        currentQuestion.QuestionNumber = currentPart.Questions.Count + 1;
                        currentPart.Questions.Add(currentQuestion);
                        currentQuestion = null;
                    }
                    continue;
                }

                if (line.StartsWith("'"))
                {
                    currentPart = new PartQuestion { Title = line.TrimStart('\'').Trim() };
                    parts.Add(currentPart);
                    continue;
                }

                if (currentQuestion == null)
                {
                    currentQuestion = new Question { Content = line };
                    continue;
                }


                bool isCorrect = line.StartsWith("*");
                string answerContent = isCorrect ? line.TrimStart('*').Trim() : line;
                currentQuestion.Answers.Add(new Answer { Content = answerContent, IsCorrect = isCorrect });

                if (currentQuestion.Answers.Count == 4)
                {
                    currentQuestion.QuestionNumber = currentPart.Questions.Count + 1;
                    currentPart.Questions.Add(currentQuestion);
                    currentQuestion = null;
                }
            }
            UpdatePreviewPart();
        }

        private void btnGuide_Click(object sender, RoutedEventArgs e)
        {
            guide.Visibility = Visibility.Visible;
        }

        private void btnOffGuide_Click(object sender, RoutedEventArgs e)
        {
            guide.Visibility = Visibility.Collapsed;
        }

        private void btnCopy_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(tbContentCopy.Text);
            CreateNotification("Đã sao chép nội dung vào clipboard");
        }

        private async void CreateNotification(string Notification)
        {
            Border border = new Border()
            {
                Margin = new Thickness(5),
                CornerRadius = new CornerRadius(20),
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3a434f")),
                Width = 500,
                Height = 40,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Padding = new Thickness(10),
                Visibility = Visibility.Visible,
                Effect = new DropShadowEffect()
                {
                    Color = (Color)ColorConverter.ConvertFromString("#5ba0fc"),
                    ShadowDepth = 5,
                },
                Child = new TextBlock()
                {
                    Text = Notification,
                    FontSize = 20,
                    FontWeight = FontWeights.Bold,
                    Foreground = new SolidColorBrush(Colors.White),
                    TextAlignment = TextAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                },
                Style = this.FindResource("border_notification") as Style

            };
            SideNotification.Children.Add(border);
            await Task.Delay(2000);

            Storyboard sb = new Storyboard();

            DoubleAnimation fadeOut = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.5),
            };
            Storyboard.SetTarget(fadeOut, border);
            Storyboard.SetTargetProperty(fadeOut, new PropertyPath(Border.OpacityProperty));

            DoubleAnimation slideUp = new DoubleAnimation
            {
                From = 0,
                To = -50,
                Duration = TimeSpan.FromSeconds(0.5),
            };
            Storyboard.SetTarget(slideUp, border);
            Storyboard.SetTargetProperty(slideUp, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.Y)"));

            sb.Children.Add(fadeOut);
            sb.Children.Add(slideUp);

            sb.Begin();

            await Task.Delay(500);


            SideNotification.Children.Remove(border);
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            if(String.IsNullOrEmpty(tbNameFile.Text))
            {
                CreateNotification("Vui lòng nhập tên file");
                return;
            }
            
            string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Exam");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            string filePath = Path.Combine(folderPath, $"{tbNameFile.Text}.json");

            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            var exam = new Exam
            {
                Name = tbNameFile.Text,
                Description = (!String.IsNullOrEmpty(tbInfo.Text)) ? tbInfo.Text : "Không có nội dung",
                Author = (!String.IsNullOrEmpty(tbNameAuthor.Text)) ? tbNameAuthor.Text : "Vô danh",
                CreatedDate = DateTime.Now
            };
            var package = new ExamPackage
            {
                ExamInfo = exam,
                Parts = parts
            };

            string json = JsonSerializer.Serialize(package, options);
            File.WriteAllText(filePath, json);
            CreateNotification($"Đã tạo file {tbNameFile.Text}.json trong thư mục Exam");

            _ = TurnBackHome();

        }
        private async Task TurnBackHome()
        {
            await Task.Delay(3000);
            var window = Window.GetWindow(this);
            if (window?.DataContext is MainViewModel vm)
            {
                vm.HomeCommand?.Execute(null);
            }
        }
    }
}
