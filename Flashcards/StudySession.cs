using ConsoleTableExt;

namespace Flashcards
{
    internal class StudySession
    {
        public int score { get; set; }
        public Stack stack { get; set; }
        private int cardsCount { get; }

        public StudySession(Stack stack)
        {
            this.score = 0;
            this.stack = stack;
            this.cardsCount = stack.flashcards.Count;
        }

        internal void Start()
        {
            bool exitSession = false;


            while (stack.flashcards.Any() && exitSession == false)
            {
                Flashcard currentViewedCard = ViewRandomCard();
                string userAnswer = GetUserAnswer();

                if (userAnswer == "0")
                {
                    exitSession = true;
                }
                else if (userAnswer != currentViewedCard.answer)
                {
                    Console.WriteLine("\n\nYour answer was wrong.");
                    Console.WriteLine($"\nYou answered {userAnswer}");
                    Console.WriteLine($"The Correct answer was {currentViewedCard.answer}");
                    Console.WriteLine("\nPress Enter to continue");
                    Console.ReadLine();
                }
                else
                {
                    score++;
                }
            }

            Console.WriteLine("\n\nExiting Study Session");
            CalculateScore();
        }

        private void CalculateScore()
        {
            Console.WriteLine($"You got {score} right out of {cardsCount}");
            Console.WriteLine("Press Enter to continue");
            Console.ReadLine();
        }

        private string GetUserAnswer()
        {
            Console.WriteLine("\n\nInput your answer to this card");
            Console.WriteLine("Or 0 to exit\n");

            string userInput = Console.ReadLine();

            if (string.IsNullOrEmpty(userInput))
            {
                Console.WriteLine("Invalid Input");
                GetUserAnswer();
            }

            return userInput;
        }

        private Flashcard ViewRandomCard()
        {
            Random rnd = new Random();
            int r = rnd.Next(stack.flashcards.Count);

            var cardData = new List<object> { "Front" };

            Flashcard randomCard = stack.flashcards[r];

            cardData.Add(randomCard.question);

            ConsoleTableBuilder
                .From(cardData)
                .WithTitle(stack.stackName)
                .WithFormat(ConsoleTableBuilderFormat.Alternative)
                .ExportAndWriteLine(TableAligntment.Left);


            stack.flashcards.Remove(randomCard);
            return randomCard;

        }
    }
}
