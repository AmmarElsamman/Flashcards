using ConsoleTableExt;

namespace Flashcards
{
    internal class Stack
    {
        public List<Flashcard> flashcards { get; set; } = new List<Flashcard> { };
        public int stackId { get; set; }
        public string stackName { get; set; }
        public bool changeCurrentStack { get; set; }

        public Stack(int stackId, List<Flashcard> flashCards, string stackName)
        {
            this.stackId = stackId;
            this.flashcards = flashCards;
            this.stackName = stackName;
            this.changeCurrentStack = false;
        }

        public void ViewAllFlashcards()
        {
            Console.Clear();

            List<List<object>> tableOfFlashcards = new List<List<object>> { new List<object> { "Id", "Front", "Back" } };
            int i = 0;
            foreach (var flashcard in flashcards)
            {
                tableOfFlashcards.Add(new List<object> { i + 1, flashcard.question, flashcard.answer });
                i++;
            }
            ConsoleTableBuilder
                .From(tableOfFlashcards)
                .WithTitle(stackName)
                .WithFormat(ConsoleTableBuilderFormat.Alternative)
                .ExportAndWriteLine(TableAligntment.Left);

        }

        public void ChangeCurrentStack()
        {
            this.changeCurrentStack = true;
        }

        public void ViewXFlashcards(int numberInput)
        {
            Console.Clear();

            if (numberInput > flashcards.Capacity)
            {
                numberInput = flashcards.Count;
                Console.Clear();
                Console.WriteLine($"There are only {numberInput} Cards");
            }

            List<List<object>> tableOfFlashcards = new List<List<object>> { new List<object> { "Id", "Front", "Back" } };

            int j = 0;
            for (int i = numberInput - 1; i >= 0; i--)
            {
                tableOfFlashcards.Add(new List<object> { j + 1, flashcards[j].question, flashcards[j].answer });
                j++;
            }
            ConsoleTableBuilder
                .From(tableOfFlashcards)
                .WithTitle(stackName)
                .WithFormat(ConsoleTableBuilderFormat.Alternative)
                .ExportAndWriteLine(TableAligntment.Left);

        }

        public Flashcard CreateFlashcard()
        {
            Console.Clear();
            string q = GetStringInput("Enter the Question on the front of the card: ");

            string a = GetStringInput("Enter the Answer on the back of the card: ");

            Flashcard newCard = new Flashcard(flashcards[flashcards.Count - 1].Id + 1, q, a, stackId);
            flashcards.Add(newCard);

            return (newCard);

        }

        internal Flashcard EditFlashcard(int flashcardId)
        {
            if (flashcardId > flashcards.Count)
            {
                Console.WriteLine($"\n\nFlashcard id {flashcardId} doesn't exists");
                Console.ReadLine();
                return null;
            }
            else
            {
                string q = GetStringInput("Enter the Question on the front of the card: ");

                string a = GetStringInput("Enter the Answer on the back of the card: ");

                flashcards[flashcardId - 1].question = q;

                flashcards[flashcardId - 1].answer = a;

                return flashcards[flashcardId - 1];
            }
        }

        internal Flashcard DeleteFlashcard(int flashcardId)
        {
            if (flashcardId > flashcards.Count)
            {
                Console.WriteLine($"\n\nFlashcard id {flashcardId} doesn't exists");
                Console.ReadLine();
                return null;
            }
            else
            {
                Flashcard removedFlashcard = flashcards[flashcardId - 1];
                flashcards.Remove(removedFlashcard);
                return removedFlashcard;
            }


        }

        private string GetStringInput(string message)
        {
            Console.WriteLine($"\n\n{message}");
            string? stringInput = Console.ReadLine();
            if (string.IsNullOrEmpty(stringInput))
            {
                GetStringInput(message);
            }
            return stringInput;
        }


    }
}
