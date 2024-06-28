# Flashcards

Console based CRUD application For learning using Flashcards.
Developed using C# and Postgres.
Also Applied DTOs to transfer data.

# Given Requirements:

- [x] This is an application where the users will create Stacks of Flashcards.
- [x] We need two different tables for stacks and flashcards. The tables should be linked by a foreign key. 
- [x] Stacks should have an unique name. 
- [x] Every flashcard needs to be part of a stack. If a stack is deleted, the same should happen with the flashcard. 
- [x] You should use DTOs to show the flashcards to the user without the Id of the stack it belongs to.
- [x] Create a "Study Session" area, where the users will study the stacks. All study sessions should be stored, with date and score.
- [x] The study and stack tables should be linked. If a stack is deleted, it's study sessions should be deleted.
- [x] The project should contain a call to the study table so the users can see all their study sessions. This table receives insert calls upon each study session, but there shouldn't be update and delete calls to it.

# Features

* Postgres database connection

	- The program uses a local Postgres db connection to store and read information. 
	- If the correct tables does not exist they will be created on program start.

* A console based UI where users can navigate by key presses
  - ![image](https://github.com/AmmarElsamman/Flashcards/assets/53655392/73e39f38-e6a7-4c2d-8bae-81558bec1a81)

* Different menu to handle Stack management
  - Each Option calls independent method which handles the database requests 
  - ![image](https://github.com/AmmarElsamman/Flashcards/assets/53655392/4a98ac21-833d-4eaf-832d-d8f1942f8a4d)

* Managing flashcards
  - First we let the user choose which stack to interact with
  - Another menu to handle flashcards requests
  - ![image](https://github.com/AmmarElsamman/Flashcards/assets/53655392/9d8bccd9-a9f4-4b03-85b6-f05ee54b2bb8)
  - ![image](https://github.com/AmmarElsamman/Flashcards/assets/53655392/6c81ff62-1c44-428d-a479-27b0d66f83bc)

* Study Session
  - Implement the same method which select the stack.
  - Then we keep picking random cards from the stack and check the user inputs and keep track of user score
  - ![image](https://github.com/AmmarElsamman/Flashcards/assets/53655392/d0ccca2c-eb02-480e-a8ba-d78caae0aff6)
  - ![image](https://github.com/AmmarElsamman/Flashcards/assets/53655392/39caa57c-629f-428b-9074-e0e3127cb3f4)

 



