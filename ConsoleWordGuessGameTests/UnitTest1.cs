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
        [InlineData("WARNING", "ASWD", true, "W A _ _ _ _ _")]
        [InlineData("ERROR", "ASWDR", true, "_ R R _ R")]
        [InlineData("WARNING", "", true, "_ _ _ _ _ _ _")]
        [InlineData("ERROR", "", true, "_ _ _ _ _")]
        [InlineData("WARNING", "ARGINSWD", false, "W A R N I N G")]
        [InlineData("ERROR", "EOASWDR", false, "E R R O R")]
        public void ReturnMaskedWordWithMatchedLetters(string word, string collectedUserInput, bool withUnderscores, string expectedValue)
        {
            string returnedValue = string.Empty;
            Program.MaskWithFilter(word, collectedUserInput, out returnedValue);
            Assert.Equal(expectedValue, returnedValue);
        }
        [Theory]
        [InlineData("WARNING", "ASWD", true, "W A _ _ _ _ _")]
        [InlineData("ERROR", "ASWDR", true, "_ R R _ R")]
        [InlineData("WARNING", "", true, "_ _ _ _ _ _ _")]
        [InlineData("ERROR", "", true, "_ _ _ _ _")]
        [InlineData("WARNING", "ARGINSWD", false, "W A R N I N G")]
        [InlineData("ERROR", "EOASWDR", false, "E R R O R")]
        public void ReturnCorrectBoolIfWithUnderscores(string word, string collectedUserInput, bool withUnderscores, string expectedValue)
        {
            string returnedValue = string.Empty;
            Assert.Equal(withUnderscores, Program.MaskWithFilter(word, collectedUserInput, out returnedValue));
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
