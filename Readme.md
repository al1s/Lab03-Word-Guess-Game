# Word Guess Game 
## Description
The idea of the game is the user must guess what a mystery word is by inputting 1 letter at a time.
The game should save all of guesses (both correct and incorrect) throughout each session of 
the game, along with the ability to show how many letters out of the word was guessed 
correctly. All "mystery" words are kept in external file with ability for the user to add/remove
words.

## Getting started

* Clone the repo;
* Build with `dotnet build --configuration Release`;
* Find the dll in `bin/Release` folder;
* Start with `dotnet ConsoleWordGuessGame.dll`;

## Specs
The program (should) contain the following

* Methods for each action (suggestions: Home navigation, View words in the external file, add a word to the external file, Remove words from a text file, exit the game, start a new game)
* When playing a game, randomly select one of the words to output to the console for the user to guess (Use the Random class)
* You should have a record of the letters they have attempted so far
* If they guess a correct letter, display that letter in the console for them to refer back to when making guesses (i.e. C _ T S )
* Your program does not need to be case sensitive.
* Errors should be handled through Exception handling
* Do not create external classes to accomplish this task.
* Stay within scope, you may use the methods/classes listed below if desired.
* Once the game is completed, the user should be presented with the option to “Play again” (a new random word is generated), or “Exit” (the program terminates)
* the user should only be allowed to guess only 1 letter at a time. Do not make it so that they can input the whole alphabet and get the answer.

## Screenshots

![image](https://raw.githubusercontent.com/al1s/Lab03-Word-Guess-Game/master/screenshot1.PNG)
![image](https://raw.githubusercontent.com/al1s/Lab03-Word-Guess-Game/master/screenshot2.PNG)
![image](https://raw.githubusercontent.com/al1s/Lab03-Word-Guess-Game/master/screenshot3.PNG)
![image](https://raw.githubusercontent.com/al1s/Lab03-Word-Guess-Game/master/screenshot4.PNG)
![image](https://raw.githubusercontent.com/al1s/Lab03-Word-Guess-Game/master/screenshot5.PNG)
![image](https://raw.githubusercontent.com/al1s/Lab03-Word-Guess-Game/master/screenshot6.PNG)
![image](https://raw.githubusercontent.com/al1s/Lab03-Word-Guess-Game/master/screenshot7.PNG)
![image](https://raw.githubusercontent.com/al1s/Lab03-Word-Guess-Game/master/screenshot8.PNG)
