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
        [InlineData("WARNING", "_ _ _ _ _ _ _")]
        [InlineData("ERROR", "_ _ _ _ _")]
        public void ReturnUnderscoreForWord(string word, string expectedValue)
        {
            Assert.Equal(expectedValue, Program.MaskWord(word));
        }
        [Theory]
        [InlineData("WARNING", "ASWD", "W A _ _ _ _ _")]
        [InlineData("ERROR", "ASWDR" ,"_ R R _ R")]
        [InlineData("WARNING", "", "_ _ _ _ _ _ _")]
        [InlineData("ERROR", "" ,"_ _ _ _ _")]
        public void ReturnMaskedWordWithMatchedLetters(string word, string collectedUserInput, string expectedValue)
        {
            Assert.Equal(expectedValue, Program.UnmaskByUserInput(word, collectedUserInput));
        }
    }
}
