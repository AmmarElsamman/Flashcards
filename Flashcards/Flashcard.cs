namespace Flashcards
{
    internal class Flashcard
    {
        public int Id { get; set; }
        public string question { get; set; }
        public string answer { get; set; }
        public int stackId { get; set; }

        public Flashcard(int id, string q, string a, int stackId)
        {
            this.Id = id;
            this.question = q;
            this.answer = a;
            this.stackId = stackId;
        }
    }
}
