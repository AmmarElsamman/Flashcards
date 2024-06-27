namespace Flashcards
{
    internal class User
    {
        private DatabaseManager DB;
        public User()
        {
            this.DB = new DatabaseManager();
            this.MainMenu();
        }

        private void MainMenu()
        {
            Console.Clear();
            bool exitCode = false;

            while (!exitCode)
            {
                Console.WriteLine("\n\nMain Menu\n");
                Console.WriteLine("--------------------------------");
                Console.WriteLine("0 | Exit");
                Console.WriteLine("1 | Manage Stacks");
                Console.WriteLine("2 | Manage Flashcards");
                Console.WriteLine("3 | Study");
                Console.WriteLine("4 | View Study Session Data");
                Console.WriteLine("--------------------------------");

                string? userInput = Console.ReadLine();

                switch (userInput)
                {
                    case "0":
                        Console.WriteLine("\nGoodbye\n");
                        exitCode = true;
                        Environment.Exit(0);
                        break;
                    case "1":
                        Stacks();
                        break;
                    case "2":
                        Flashcards();
                        break;
                    case "3":
                        Study();
                        break;
                    case "4":
                        ViewSessions();
                        break;
                    default:
                        Console.WriteLine("\nNot Valid Option. Please Enter a number from 0 to 4\n");
                        break;
                }

            }


        }

        private void Study()
        {
            Stack currentStack = PickCurrentStack();
            while (currentStack == null)
            {
                currentStack = PickCurrentStack();
            }

            StudySession newSession = new StudySession(currentStack);

            newSession.Start();

            DB.InsertNewSession(newSession);

        }

        private void Stacks()
        {
            StackHandler();
        }

        private void Flashcards()
        {
            Stack currentStack = PickCurrentStack();
            while (currentStack == null)
            {
                currentStack = PickCurrentStack();
            }


            FlashcardsHandler(currentStack);


            if (currentStack.changeCurrentStack)
            {
                Flashcards();
            }

        }

        private string GetStackName()
        {
            Console.WriteLine("--------------------------------------");
            Console.WriteLine("Input a current stack name");
            Console.WriteLine("Or input 0 to exit input");
            Console.WriteLine("--------------------------------------");

            string stringInput = Console.ReadLine();

            if (stringInput == "0") MainMenu();

            return stringInput;

        }

        private void ViewSessions()
        {
            Console.Clear();
            DB.ViewStudySessions();
        }

        private Stack PickCurrentStack()
        {
            Console.Clear();
            DB.ViewStacks();

            Console.WriteLine("\nChoose a stack of flashcards to interact with:\n");

            string stackName = GetStackName();
            string parsedStackName = string.Concat(stackName[0].ToString().ToUpper(), stackName.AsSpan(1));

            if (DB.CheckStackExists(parsedStackName))
            {
                int stackId = DB.GetStackId(parsedStackName);
                List<Flashcard> flashcards = DB.GetListOfFlashcards(stackId);

                Stack currentStack = new Stack(stackId, flashcards, parsedStackName);
                return currentStack;

            }
            else
            {
                Console.WriteLine($"\nStack {stackName} doesn't exist");
                Console.ReadLine();
                return null;
            }

        }

        private int GetNumberInput(string message)
        {
            Console.WriteLine("\n-----------------------");
            Console.WriteLine(message);
            Console.WriteLine("Or 0 to exit");
            Console.WriteLine("------------------------\n");

            string numberInput = Console.ReadLine();

            if (numberInput == "0") MainMenu();

            while (!Int32.TryParse(numberInput, out _) || Convert.ToInt32(numberInput) < 0)
            {
                Console.WriteLine("\n\nInvalid number.\n\n");
                numberInput = Console.ReadLine();
            }

            int result = Convert.ToInt32(numberInput);

            return result;

        }

        private void FlashcardsHandler(Stack currentStack)
        {
            Console.Clear();
            bool exitFlashcardMenu = false;



            while (!exitFlashcardMenu)
            {
                Console.WriteLine("\n--------------------------------------------");
                Console.WriteLine($"Current working stack: {currentStack.stackName}\n");

                Console.WriteLine("0 | Return to Main Menu");
                Console.WriteLine("1 | Change current stack");
                Console.WriteLine("2 | View all Flashcards in stack");
                Console.WriteLine("3 | View X amount of cards in stack");
                Console.WriteLine("4 | Create a Flashcard in current stack");
                Console.WriteLine("5 | Edit a Flashcard");
                Console.WriteLine("6 | Delete a Flashcard");
                Console.WriteLine("--------------------------------------------");

                string? userInput = Console.ReadLine();


                switch (userInput)
                {
                    case "0":
                        exitFlashcardMenu = true;
                        break;
                    case "1":
                        currentStack.ChangeCurrentStack();
                        exitFlashcardMenu = true;
                        break;
                    case "2":
                        currentStack.ViewAllFlashcards();
                        break;
                    case "3":
                        int numberInput = GetNumberInput("Enter number of cards you need:");
                        currentStack.ViewXFlashcards(numberInput);
                        break;
                    case "4":
                        Flashcard createdFC = currentStack.CreateFlashcard();
                        DB.CreateFlashcard(createdFC);
                        break;
                    case "5":
                        currentStack.ViewAllFlashcards();
                        int fcIdEdit = GetNumberInput("Input an ID of a flashcard");
                        if (fcIdEdit == 0)
                            break;
                        Flashcard editedFc = currentStack.EditFlashcard(fcIdEdit);
                        if (editedFc != null)
                            DB.EditFlashcard(editedFc);
                        break;
                    case "6":
                        currentStack.ViewAllFlashcards();
                        int fcIdDelete = GetNumberInput("Input an ID of a flashcard");
                        if (fcIdDelete == 0)
                            break;
                        Flashcard deletedFC = currentStack.DeleteFlashcard(fcIdDelete);
                        if (deletedFC != null)
                            DB.DeleteFlashcard(deletedFC);
                        break;
                    default:
                        Console.WriteLine("\nNot Valid Option. Please Enter a number from 0 to 6\n");
                        break;


                }
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

        private void StackHandler()
        {
            Console.Clear();
            bool exitStackMenu = false;



            while (!exitStackMenu)
            {
                Console.WriteLine("\n\n--------------------------------------------");
                Console.WriteLine("0 | Return to Main Menu");
                Console.WriteLine("1 | Create A Stack");
                Console.WriteLine("2 | Delete A Stack");
                Console.WriteLine("3 | Edit A Stack");
                Console.WriteLine("--------------------------------------------");

                string? userInput = Console.ReadLine();

                switch (userInput)
                {
                    case "0":
                        exitStackMenu = true;
                        break;

                    case "1":
                        CreateStack();
                        break;

                    case "2":
                        DeleteStack();
                        break;

                    case "3":
                        EditStack();
                        break;

                    default:
                        Console.WriteLine("\nNot Valid Option. Please Enter a number from 0 to 3\n");
                        break;



                }
            }
        }

        private void EditStack()
        {
            string stackName = GetStringInput("Enter name of Stack you want to edit:");
            stackName = string.Concat(stackName[0].ToString().ToUpper(), stackName.AsSpan(1));
            if (DB.CheckStackExists(stackName))
            {
                string newStackName = GetStringInput("Insert new Stack name:");
                DB.EditStack(newStackName, stackName);
            }
            else
            {
                Console.WriteLine($"\n Stack {stackName} doesn't exists");
                Console.ReadLine();
            }

        }

        private void DeleteStack()
        {
            string stackName = GetStringInput("Enter name of Stack you want to delete:");
            stackName = string.Concat(stackName[0].ToString().ToUpper(), stackName.AsSpan(1));
            if (DB.CheckStackExists(stackName))
            {
                DB.DeleteStack(stackName);
            }
            else
            {
                Console.WriteLine($"\n Stack {stackName} doesn't exists");
                Console.ReadLine();
            }

        }

        private void CreateStack()
        {
            string stackName = GetStringInput("Enter name of new Stack:");
            stackName = string.Concat(stackName[0].ToString().ToUpper(), stackName.AsSpan(1));
            if (DB.CheckStackExists(stackName))
            {
                Console.WriteLine($"\n Stack {stackName} already exists");
                Console.ReadLine();
            }
            else
            {

                DB.CreateStack(stackName);
            }
        }
    }
}
