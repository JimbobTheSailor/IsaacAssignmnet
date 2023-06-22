using System;   
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        public MathGame MyMathGame { get; set; }
        public string MyQuestion { get; set; } = "Works";
        public MainWindow()
        {
            MyMathGame = new MathGame();
            InitializeComponent();
            SliderRounds.Value = MyMathGame.GameRounds;
            HideAllControls();
        }

        private void Hard_Checked(object sender, RoutedEventArgs e)
        {
            var cb = sender as CheckBox;
            if (cb == null)
                return;

            if (cb.IsChecked.HasValue)
            {
                if (cb.IsChecked.Value == true)
                {
                    if(DifficultyCheckBoxMedium!=null)
                        DifficultyCheckBoxMedium.IsChecked = false;
                    if(DifficultyCheckBoxEasy!=null)
                        DifficultyCheckBoxEasy.IsChecked = false;
                    MyMathGame.SetDifficultly(true, false, false);
                }
            }
        }
        private void Med_Checked(object sender, RoutedEventArgs e)
        {
            var cb = sender as CheckBox;
            if (cb == null)
                return;

            if (cb.IsChecked.HasValue)
            {
                if (cb.IsChecked.Value == true)
                {
                    if(DifficultyCheckBoxHard!=null)
                        DifficultyCheckBoxHard.IsChecked = false;

                    if(DifficultyCheckBoxEasy!=null)
                        DifficultyCheckBoxEasy.IsChecked = false;
                    MyMathGame.SetDifficultly(false, true, false);
                }
              
            }
        }
        private void Easy_Checked(object sender, RoutedEventArgs e)
        {
            var cb = sender as CheckBox;
            if (cb == null)
                return;
            if (cb.IsChecked.HasValue)
            {
                if (cb.IsChecked.Value == true)
                {
                    if(DifficultyCheckBoxMedium!=null)
                        DifficultyCheckBoxMedium.IsChecked = false;
                    if (DifficultyCheckBoxHard != null)
                        DifficultyCheckBoxHard.IsChecked = false;
                    MyMathGame.SetDifficultly(false, false, true);
                }
               
            }
        }
        private void SelectAll_Checked(object sender, RoutedEventArgs e)
        {
            CheckBoxAdd.IsChecked = CheckBoxMult.IsChecked = CheckBoxSub.IsChecked = CheckBoxDiv.IsChecked = true;
        }

        private void SelectAll_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBoxAdd.IsChecked = CheckBoxMult.IsChecked = CheckBoxSub.IsChecked = CheckBoxDiv.IsChecked = false;
        }

        private void SelectAll_Indeterminate(object sender, RoutedEventArgs e)
        {
            // If the SelectAll box is checked (all options are selected), 
            // clicking the box will change it to its indeterminate state.
            // Instead, we want to uncheck all the boxes,
            // so we do this programatically. The indeterminate state should
            // only be set programatically, not by the user.

            if (CheckBoxAdd.IsChecked == true &&  CheckBoxMult.IsChecked == true &&   CheckBoxSub.IsChecked == true &&  CheckBoxDiv.IsChecked == true)
            {
                // This will cause SelectAll_Unchecked to be executed, so
                // we don't need to uncheck the other boxes here.
                OptionsAllCheckBox.IsChecked = false;
            }
        }

        private void HideAllControls()
        {
            Questions.Visibility = Visibility.Hidden;
            UserInput.Visibility = Visibility.Hidden;
           // ButtonAns.Visibility = Visibility.Hidden;
            TextBlockAnswer.Visibility = Visibility.Hidden;
            TextBlockGameOver.Visibility = Visibility.Hidden;
        }
        private void SetCheckedState()
        {
            // Controls are null the first time this is called, so we just 
            // need to perform a null check on any one of the controls.
            if (CheckBoxDiv != null)
            {
                if (CheckBoxAdd.IsChecked == true &&  CheckBoxMult.IsChecked == true &&  CheckBoxSub.IsChecked == true && CheckBoxDiv.IsChecked == true)
                {
                    OptionsAllCheckBox.IsChecked = true;
                }
                else if (CheckBoxAdd.IsChecked == false &&  CheckBoxMult.IsChecked == false &&  CheckBoxSub.IsChecked == false &&   CheckBoxDiv.IsChecked == false)
                {
                    OptionsAllCheckBox.IsChecked = false;
                    MyMathGame.Addition = true;
                }
                else
                {
                    // Set third state (indeterminate) by setting IsChecked to null.
                    OptionsAllCheckBox.IsChecked = null;
                }
                //Read all Booleans from Window and store in my MathClass object
                if(CheckBoxAdd.IsChecked.HasValue)
                    MyMathGame.Addition = CheckBoxAdd.IsChecked.Value;
                if (CheckBoxDiv.IsChecked.HasValue)
                    MyMathGame.Division = CheckBoxDiv.IsChecked.Value;
                if (CheckBoxSub.IsChecked.HasValue)
                    MyMathGame.Subtraction = CheckBoxSub.IsChecked.Value;
                if (CheckBoxMult.IsChecked.HasValue)
                    MyMathGame.Multiplication = CheckBoxMult.IsChecked.Value;
            }
        }

        /// <summary>
        /// Button handler for "Start Game"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Game_Start(object sender, RoutedEventArgs e)
        {
            ButtonStart.Content = "Start Game";
            Questions.Visibility = Visibility.Visible;
            UserInput.Visibility = Visibility.Visible;
            ButtonRestartScore.Visibility = Visibility.Visible;
            TextBlockScore.Visibility = Visibility.Visible;
            TextBlockGameOver.Visibility = Visibility.Hidden;
            Questions.Text = MyMathGame.FeedMeTheNextQuestion();
        }

        private void TextBoxUserInputKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                TextBlockAnswer.Visibility = Visibility.Visible;
                ProcessAnswer();
                UserInput.Text = "";
            }
        }
        private void ProcessAnswer()
        {
            var ok = Int32.TryParse(UserInput.Text, out int result);
            if (!ok)
                return;
            var IsCorrect = MyMathGame.CheckAnswer(result);
            TextBlockAnswer.Text = MyMathGame.AnswerSentence;
            TextBlockScore.Text = $"Your Score is {MyMathGame.TotalScore}";
            if (IsCorrect)
                TextBlockAnswer.Background = Brushes.LightGreen;
            else
                TextBlockAnswer.Background = Brushes.LightSalmon;

            if (MyMathGame.GameOver())
            {
                Questions.Visibility = Visibility.Collapsed;
                UserInput.Visibility = Visibility.Collapsed;
                TextBlockAnswer.Visibility = Visibility.Collapsed;
                TextBlockGameOver.Visibility = Visibility.Visible;

                MyMathGame.EndGame();
                TextBlockGameOver.Text = MyMathGame.ScoreReport;
                ButtonStart.Content = "New Game?";
            }
            else //Next Question
            {
                Questions.Text = MyMathGame.FeedMeTheNextQuestion();
            }
        }
        private void Restart_Score(object sender, RoutedEventArgs e)
        {
            MyMathGame.TotalScore = 0;
            TextBlockScore.Text = $"Your Score is {MyMathGame.TotalScore}";
        }
        /// <summary>
        /// Event handler for slider
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SliderRounds_ValueChanged(object sender, RoutedEventArgs e)
        {

            var mySlider = sender as Slider;
            if (mySlider == null)
                return;

            MyMathGame.GameRounds = (int) mySlider.Value;

        }

        private void Option_Checked(object sender, RoutedEventArgs e)
        {
            SetCheckedState();
        }

        private void Option_Unchecked(object sender, RoutedEventArgs e)
        {
            SetCheckedState();
        }

        private void UserInput_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
