using System;
using System.IO;

namespace ConsoleWordGuessGame
{
    public class Program
    {
        static void Main(string[] args)
        {
            string[] data = default(string[]);
            string path = "data.txt";
            while (true) {
                if (File.Exists(path))
                {
                    data = ReadFrom(path);
                    DisplayInitialScreen();
                    switch (GetInput("numeric"))
                    {
                        case "1":
                            HandlePlay(data);
                            break;
                        case "2":
                            DisplayAdminScreen();
                            switch (GetInput("numeric"))
                            {
                                case "1":
                                    DisplayShowAllWordsScreen(data);
                                    break;
                                case "2":
                                    Console.Clear();
                                    DisplayAddWordScreen(path);
                                    break;
                                case "3":
                                    DisplayDeleteWordScreen(data, path);
                                    break;
                            };
                            break;
                        case "3":
                            Environment.Exit(0);
                            break;
                    };
                }
                else
                {
                    Console.Write("There is no words to choose from. ");
                    DisplayAddWordScreen(path);
                }
            }
        }
        // ================= IO OPERATIONS ====================
        /// <summary>
        /// Creates new file and/or append given string to it.
        /// </summary>
        /// <param name="path">Path to file</param>
        /// <param name="content">String to add to file</param>
        /// <returns>return code (0 - Ok, 1 - Error)</returns>
        public static int CreateAppendTo(string path, string content)
        {
            int retCode = 1;
            StreamWriter sw = File.AppendText(path);
            try
            {
                sw.WriteLine(content);
                retCode = 0;
            }
            catch (Exception ex)
            {
                ExceptionMessageToConsole("Can't write to file", ex);
                throw ex;
            }
            finally
            {
                if (sw != null) sw.Close();
            }
            return retCode;
        }
        /// <summary>
        /// Read all strings from file
        /// </summary>
        /// <param name="path">Path to file</param>
        /// <returns>Array of strings in the file</returns>
        public static string[] ReadFrom(string path)
        {
            string[] result = default(string[]);
            try
            {
                result = File.ReadAllLines(path);
            }
            catch (Exception ex)
            {
                ExceptionMessageToConsole("Can't read file", ex);
                throw ex;
            }
            return result;
        }
        /// <summary>
        /// Delete file from path
        /// </summary>
        /// <param name="path">Path to file</param>
        /// <param name="lineNumber">Number of string to delete</param>
        /// <returns>return code (0 - Ok, 1 - Error)</returns>
        public static int DeleteFrom(string path, int lineNumber)
        {
            int result = default(int);
            try
            {
                string[] words = ReadFrom(path);
                File.Delete(path);
                for (int i = 0; i < words.Length; i++)
                {
                    if (i + 1 != lineNumber) CreateAppendTo(path, words[i]);
                }
                result = 0;
            }
            catch (Exception ex)
            {
                ExceptionMessageToConsole("Can't delete from file", ex);
                throw ex;
            }
            return result;
        }
        /// <summary>
        /// Handle addition/deletion of word in a file
        /// </summary>
        /// <param name="dataToHandle">Data we want be added/deleted from file</param>
        /// <param name="action">Action we want to undertake</param>
        /// <param name="path">Path to file</param>
        public static void ManageFileContent(string dataToHandle, string action, string path)
        {
            if (dataToHandle== "ctrlx") return;
            switch (action)
            {
                case "addWord":
                    CreateAppendTo(path, dataToHandle);
                    break;
                case "deleteWord":
                    // empty input destroys it
                    DeleteFrom(path, int.Parse(dataToHandle));
                    break;
            }
        }
        /// <summary>
        /// Print custom error message and internal error message to console
        /// </summary>
        /// <param name="message">Custom error message</param>
        /// <param name="ex">Original exception type</param>
        public static void ExceptionMessageToConsole(string message, Exception ex)
        {
            Console.WriteLine(message);
            Console.WriteLine(ex.Message);
        }


        // ================= APP ====================
        /// <summary>
        /// Mask with underscores the letters which are not in the word
        /// </summary>
        /// <param name="word">Word to mask</param>
        /// <param name="keepUnmasked">Words to keep unmasked</param>
        /// <returns>The word masked with underscores</returns>
        public static string MaskWithFilter(string word, string keepUnmasked)
        {
            char[] arr = word.ToCharArray();
            for (int i = 0; i < arr.Length; i++)
            {
                if (!keepUnmasked.Contains(arr[i]))
                {
                    arr[i] = '_';
                }
            }
            return string.Join(' ', arr);
        }
        /// <summary>
        /// Check if underscores are in the word
        /// </summary>
        /// <param name="word">Word to mask</param>
        /// <param name="keepUnmasked">Words to keep unmasked</param>
        /// <returns>True - the word has underscores, false - doesn't</returns>
        public  static bool WithUnderscores(string word, string keepUnmasked)
        {
            bool withUnderscores = false;
            char[] arr = word.ToCharArray();
            for (int i = 0; i < arr.Length; i++)
            {
                if (!keepUnmasked.Contains(arr[i]))
                {
                    withUnderscores = true;
                }
            }
            return withUnderscores;
        }
        /// <summary>
        /// Given an array of strings choose random
        /// </summary>
        /// <param name="words">Array of strings</param>
        /// <returns>One random word</returns>
        public static string GetRandomWordFrom(string[] words)
        {
            Random random = new Random();
            return words[random.Next(0, words.Length)];
        }
        /// <summary>
        /// Helper to manage game flow
        /// </summary>
        /// <param name="words">Array of strings for the game</param>
        public static void HandlePlay(string[] words)
        {
            string word = GetRandomWordFrom(words);
            string maskedWord = string.Empty;
            string userInput = string.Empty;
            string userInputCollector = string.Empty;

            while (WithUnderscores(word, userInputCollector.ToUpper()))
            {
                maskedWord = MaskWithFilter(word, userInputCollector.ToUpper());
                DisplayGamePlayScreen(maskedWord, userInputCollector);
                userInput = GetInput("alphabetic");
                if (userInput == "ctrlx") return;
                userInputCollector += userInput;
            };
            maskedWord = MaskWithFilter(word, userInputCollector.ToUpper());
            DisplayGamePlayScreen(maskedWord, userInputCollector);
            Console.WriteLine($"Guessed correctly in {userInputCollector.Length} turns!");
            Console.WriteLine("Play Again?");
            Console.Write("Y/N: ");
            if (GetInput("alphabetic").ToUpper() == "Y") HandlePlay(words);
        }

        // ================= UI ====================
        /// <summary>
        /// Show main menu to the screen
        /// </summary>
        public static void DisplayInitialScreen()
        {
            Console.Clear();
            Console.WriteLine("Word Guess Game");
            Console.WriteLine("Choose operation");
            Console.WriteLine("1. Play");
            Console.WriteLine("2. Admin");
            Console.WriteLine("3. Exit");
        }
        /// <summary>
        /// Show admin menu to the screen
        /// </summary>
        public static void DisplayAdminScreen()
        {
            Console.Clear();
            Console.WriteLine("Admin menu:");
            Console.WriteLine("1. Show All Words");
            Console.WriteLine("2. Add New Word");
            Console.WriteLine("3. Delete Word");
            Console.WriteLine("4. Main Menu");
        }
        /// <summary>
        /// Show add word menu to the screen
        /// </summary>
        public static void DisplayAddWordScreen(string path)
        {
            Console.WriteLine("Add New Word (Press Enter to save, Ctrl-X to get back to Main Menu):");
            ManageFileContent(GetInput("alphabetic").ToUpper(), "addWord", path);
            Console.WriteLine("Word was added");
            Console.ReadLine();
        }
        /// <summary>
        /// Show delete word menu to the screen
        /// </summary>
        public static void DisplayDeleteWordScreen(string[] words, string path)
        {
            Console.Clear();
            Console.WriteLine("Choose # of Word to delete (Press Enter to save, Ctrl-X to get back to Main Menu):");
            for (int i = 0; i < words.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {words[i]}");
            }
            ManageFileContent(GetInput("alphabetic"), "deleteWord", path);
            Console.WriteLine("Word was deleted");
            Console.ReadLine();
        }
        /// <summary>
        /// Show all words in the game storage
        /// </summary>
        /// <param name="words">Words to show</param>
        public static void DisplayShowAllWordsScreen(string[] words)
        {
            Console.Clear();
            Console.WriteLine("All Available Words");
            foreach (string word in words)
            {
                Console.WriteLine(word);
            }
            Console.ReadLine();
        }
        /// <summary>
        /// Show main game screen
        /// </summary>
        /// <param name="wordToShow">The word the user guesses</param>
        /// <param name="alreadyUsedSymbols">The characters already used by the user</param>
        /// <returns></returns>
        public static void DisplayGamePlayScreen(string wordToShow, string alreadyUsedSymbols)
        {
            Console.Clear();
            Console.WriteLine("Choose a letter");
            Console.WriteLine();
            Console.WriteLine(wordToShow);
            Console.WriteLine(alreadyUsedSymbols);
        }
        /// <summary>
        /// Acquire user input depending
        /// </summary>
        /// <param name="expectedInputType">Expected input type (numeric, alphabetic)</param>
        /// <returns>User input</returns>
        public static string GetInput(string expectedInputType)
        {
            // storage for input result 
            string userInput = string.Empty;
            ConsoleKeyInfo pressed;
            // while not pressed Enter
            do
            {
                // don't show key char on the screen
                pressed = Console.ReadKey(true);
                // check whether the key pressed  is a number or not
                if (pressed.Key != ConsoleKey.Backspace && expectedInputType == "numeric")
                {
                    int value = default(int);
                    if (int.TryParse(pressed.KeyChar.ToString(), out value))
                    {
                        userInput += pressed.KeyChar;
                        Console.Write(pressed.KeyChar);
                    }
                    if ((pressed.Modifiers & ConsoleModifiers.Control) != 0 && pressed.Key == ConsoleKey.X)
                    {
                        userInput = "ctrlx";
                        break;
                    }
                }
                // check whether the key pressed is an alphabetic char or not
                else if (pressed.Key != ConsoleKey.Backspace && expectedInputType == "alphabetic")
                {
                    char value = default(char);
                    if (char.TryParse(pressed.KeyChar.ToString(), out value))
                    {
                        if(pressed.Key != ConsoleKey.Enter)
                        {
                            userInput += pressed.KeyChar;
                        }
                        Console.Write(pressed.KeyChar);
                    }
                    if ((pressed.Modifiers & ConsoleModifiers.Control) != 0 && pressed.Key == ConsoleKey.X)
                    {
                        userInput = "ctrlx";
                        break;
                    }
                }
                else
                {
                    // the user want do delete the last char
                    if (pressed.Key == ConsoleKey.Backspace && userInput.Length != 0)
                    {
                        // sync storage and screen 
                        userInput = userInput.Substring(0, userInput.Length - 1);
                        Console.Write("\b \b");
                    }
                }
            } while (pressed.Key != ConsoleKey.Enter);
            return userInput;
        }
    }
}
