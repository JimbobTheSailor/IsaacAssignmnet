using System;
using System.Collections.Generic;

namespace WpfApp1
{
    public class MathGame
    {
        //public bool asmd = false;
        public bool Addition = false;
        public bool Subtraction = false;
        public bool Multiplication = false;
        public bool Division = false;
        public int Score = 0;
        public int TotalScore = 0;
        public int QuestionNumber;
        public string AnswerSentence="";
        public string ScoreReport="";
        private Random rnd;
        private int MaxMDRange; //the maximum value for mulitply and divide
        private int MaxASRange; //the maximum value for adding and subtration
        public int GameRounds=5;
        public string Question="";
        public int CorrectAnswer;
        private List<string> ArrayOfPositiveFeedback; //Array
        //Constructor, called automatically
        public MathGame()
        {
            rnd = new Random();
            ArrayOfPositiveFeedback = new List<string>
            {
                "Correct",
                "Well Done",
                "You Rock",
                "Excellent",
                "Great Work"
            };
        }

        /// <summary>
        /// Sets the Difficulty Level of the Game
        /// </summary>
        /// <param name="Hard"></param>
        /// <param name="Med"></param>
        /// <param name="Easy"></param>
        public void SetDifficultly(bool Hard, bool Med, bool Easy)
        {
            if(Hard)
            {
                MaxMDRange = 20;
                MaxASRange = 100;
            }
            else if(Med)
            {
                MaxMDRange = 15;
                MaxASRange = 50;
            }
            else
            {
                MaxMDRange = 10;
                MaxASRange = 20;
            }

        }

        public string FeedMeTheNextQuestion()
        {
            int Operand = NextQuestionType();

            if (Operand == 1)
                AdditionGame();
            else if (Operand == 2)
                SubtractionGame();
            else if (Operand == 3)
                MultiplyGame();
            else if (Operand == 4)
                DivideGame();
            else
            {
                //do nothing, error 
            }

            return Question;
           
        }

        /// <summary>
        /// Determines the next question to ask (1,2,3 or 4) based on 
        /// 1) random choice And 
        /// 2) whether the question is allowed
        /// </summary>
        /// <returns>Iteger between 1 and 4</returns>
        public int NextQuestionType()
        {
            bool cont = true;
            int x = 0;
            while (cont)
            {
                x = rnd.Next(1, 5);
                if (x == 1 && Addition)
                    cont = false;
                else if (x == 2 && Subtraction)
                    cont = false;
                else if (x == 3 && Multiplication)
                    cont = false;
                else if (x == 4 && Division)
                    cont = false;
                else
                {
                    //The combination was not valid, need to continue in the loop 
                }
            }
            return x;
        }
        /// <summary>
        /// Mulitplication Question, and checks/score answer
        /// </summary>
        public void MultiplyGame()
        {
            var x = rnd.Next(1, MaxMDRange);
            var y = rnd.Next(1, MaxMDRange);
            CorrectAnswer = x * y;
            Question=$"What is {x} x {y}";
        }
        public void DivideGame()
        {
            var x = rnd.Next(1, MaxMDRange);
            var y = rnd.Next(1, MaxMDRange);
            var z = x * y;
            CorrectAnswer = x;
            Question= $"What is {z} / {y}";
        }
        public void AdditionGame()
        {
            var x = rnd.Next(1, MaxASRange);
            var y = rnd.Next(1, MaxASRange);
            CorrectAnswer = x + y;
            Question= $"What is {x} + {y}";
        }
        public void SubtractionGame()
        {
            var x = rnd.Next(1, MaxASRange);
            var y = rnd.Next(1, MaxASRange);
            var z = x + y;
            CorrectAnswer = x;
            Question = $"What is {z} - {y}";
        }
        /// <summary>
        /// Gets user answer, check it with real answer, and score it
        /// </summary>
        /// <param name="UserAnswer"></param>
        public bool CheckAnswer(int UserAnswer)
        {
            QuestionNumber++;
            if (CorrectAnswer == UserAnswer)
            {
                Score++;
                TotalScore++;
                var FeedbackArrayIndex = rnd.Next(0, ArrayOfPositiveFeedback.Count-1); //Randomly chose the index of the array of positive feedback
                AnswerSentence = $"{ArrayOfPositiveFeedback[FeedbackArrayIndex]}";
                return true;
            }
            else
            {
                AnswerSentence =$"{UserAnswer} is Incorrect\r\nThe answer is {CorrectAnswer}";
                return false;
            }
        }

        /// <summary>
        /// Check for game over
        /// </summary>
        /// <returns>True if game over</returns>
        public bool GameOver()
        {
            if (QuestionNumber >= GameRounds)
                return true;
            else
                return false;
        }
        /// <summary>
        /// Calculates final score, crates report, resets game variables
        /// </summary>
        public void EndGame()
        {
            double Percentage = ((double)Score / (double)QuestionNumber) * 100;
            ScoreReport = $"You scored {Score} out of {QuestionNumber}\r\nYour Accuracy is {Percentage:f2}%";
            Score = 0;
            QuestionNumber = 0;
        }
    }//end of class
}
