using System;
using Xunit;
using ConsoleWordGuessGame;
using System.IO;

namespace ConsoleWordGuessGameTests
{
    public class UnitTest1
    {
        [Fact]
        public void CanCreateFile()
        {
            string path = "data.txt";
            if (File.Exists(path)) File.Delete(path);
            Program.CreateAppendTo(path, "");
            Assert.True(File.Exists(path));
            if (File.Exists(path)) File.Delete(path);
        }
        [Fact]
        public void CanReadFromFile()
        {
            string path = "data.txt";
            if (File.Exists(path)) File.Delete(path);
            string testString = "check we can read a string from file";
            Program.CreateAppendTo(path, testString);
            string[] dataContent = Program.ReadFrom(path);
            Assert.Contains(testString, dataContent);
            if (File.Exists(path)) File.Delete(path);
        }
        [Fact]
        public void CanDeleteFromFile()
        {
            string path = "anotherData.txt";
            if (File.Exists(path)) File.Delete(path);
            string[] wordsInFile = "check we can read a string from file".Split(" ");
            int wordNumberToDelete = 3;
            Array.ForEach(wordsInFile, elm => Program.CreateAppendTo(path, elm));
            Program.DeleteFrom(path, 3);
            string[] dataContent = Program.ReadFrom(path);
            Assert.DoesNotContain(wordsInFile[wordNumberToDelete], dataContent);
            if (File.Exists(path)) File.Delete(path);
        }
        [Theory]
        [InlineData("WARNING", "ASWD",  "W A _ _ _ _ _")]
        [InlineData("ERROR", "ASWDR",  "_ R R _ R")]
        [InlineData("WARNING", "",  "_ _ _ _ _ _ _")]
        [InlineData("ERROR", "",  "_ _ _ _ _")]
        [InlineData("WARNING", "ARGINSWD", "W A R N I N G")]
        [InlineData("ERROR", "EOASWDR",  "E R R O R")]
        public void ReturnMaskedWordWithMatchedLetters(string word, string collectedUserInput, string expectedValue)
        {
            string returnedValue = string.Empty;
            returnedValue = Program.MaskWithFilter(word, collectedUserInput);
            Assert.Equal(expectedValue, returnedValue);
        }
        [Theory]
        [InlineData("WARNING", "ASWD", true)]
        [InlineData("ERROR", "ASWDR", true)]
        [InlineData("WARNING", "", true)]
        [InlineData("ERROR", "", true)]
        [InlineData("WARNING", "ARGINSWD", false)]
        [InlineData("ERROR", "EOASWDR", false)]
        public void ReturnCorrectBoolIfWithUnderscores(string word, string collectedUserInput, bool withUnderscores)
        {
            Assert.Equal(withUnderscores, Program.WithUnderscores(word, collectedUserInput));
        }
        //[Fact]
        //public void ReturnRandomWord()
        //{
        //    int seed = 4;
        //    string[] words = new string[] { "WARNING", "ERROR", "SOLUTION", "STRUCT" };
        //    Random rnd = new Random(seed);
        //    Assert.Equal(words[rnd.Next(0, words.Length)], Program.GetRandomWordFrom(words, seed));
        //}
    }
}
