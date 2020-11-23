using Microsoft.VisualStudio.TestTools.UnitTesting;
using NapierBankingApp.Models;
using NapierBankingApp.Services;
using NapierBankingApp.Services.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace NPMTest
{
    [TestClass]
    public class ProcessorTest: Processor
    {
        [TestMethod]
        public void SobstituteAbbreviations_CorrectlyProcessed_ShouldPass()
        {
            List<string> fields = new List<string>() { "+1234567", "LOL" };
            Assert.AreEqual("LOL <Laughing out loud>", SobstituteAbbreviations(fields[1]));
        }

        [TestMethod]
        public void SobstituteAbbreviations_IncorrectlyProcessed_ShouldFail()
        {
            List<string> fields = new List<string>() { "+1234567", "lol" };
            Assert.AreEqual(fields[1], SobstituteAbbreviations(fields[1]));
        }

        // Sobstitute URL
        [TestMethod]
        public void SobstituteURL_CorrectlyProcessed_ShouldPass()
        {
            List<string> fields = new List<string>() { "david@gmai.com", "Hello World", "http:\\\\mywebsite.com" };
            Assert.AreEqual("<URL quarantined>", SobstituteURL(fields[2]));
        }

        [TestMethod]
        public void SobstituteURL_InvalidURL_ShouldNotSobstitute()
        {
            List<string> fields = new List<string>() { "david@gmai.com", "Hello World", "Hello" };
            Assert.AreEqual("Hello", SobstituteURL(fields[2]));
        }

        // Adding to lists
        // Sobstitute URL
        [TestMethod]
        public void SobstituteURL_AddingToURLList_ShouldPass()
        {
            Processor processor = new Processor();
            SEM message = new SEM("E000000000", "david@gmail.com", "Hello World", "http:\\\\mywebsite.com");
            Message processedMessage = processor.ProcessMessage(message);
            // Should return 1 as it is the count of the existing key
            Assert.AreEqual(1, processor.QuarantinedLinks["http:\\\\mywebsite.com"]);
        }

        [TestMethod]
        public void SobstituteURL_AddingToURLList_ShouldNotAdd()
        {
            Processor processor = new Processor();
            SEM message = new SEM("E000000000", "david@gmail.com", "Hello World", "Hello");
            Message processedMessage = processor.ProcessMessage(message);
            Assert.AreEqual(0, processor.QuarantinedLinks.Count);
        }

        [TestMethod]
        public void AddToMentionList_AddingCorrectMention_ShouldPass()
        {
            Processor processor = new Processor();
            Tweet message = new Tweet("T000000000", "@david", "@david");
            Message processedMessage = processor.ProcessMessage(message);
            Assert.AreEqual(1, processor.MentionsList["@david"]);
        }

        [TestMethod]
        public void AddToMentionList_AddingWrongMention_ShouldNotAdd()
        {
            Processor processor = new Processor();
            Tweet message = new Tweet("T000000000", "@david", "david");
            Message processedMessage = processor.ProcessMessage(message);
            Assert.AreEqual(0, processor.MentionsList.Count);
        }

        [TestMethod]
        public void AddToTrendingList_AddingCorrectTrend_ShouldPass()
        {
            Processor processor = new Processor();
            Tweet message = new Tweet("T000000000", "@david", "#david");
            Message processedMessage = processor.ProcessMessage(message);
            Assert.AreEqual(1, processor.TrendingList["#david"]);
        }

        [TestMethod]
        public void AddToTrendingList_AddingWrongTrend_ShouldNotAdd()
        {
            Processor processor = new Processor();
            Tweet message = new Tweet("T000000000", "@david", "david");
            Message processedMessage = processor.ProcessMessage(message);
            Assert.AreEqual(0, processor.TrendingList.Count);
        }

        [TestMethod]
        public void AddToSirList_AddingCorrectSir_ShouldAdd()
        {
            Processor processor = new Processor();
            SIR message = new SIR("E000000000", "david@gmail.com", "SIR 02/07/1995","99-99-99", "Theft","http:\\\\mywebsite.com");
            Message processedMessage = processor.ProcessMessage(message);
            Assert.AreEqual(1, processor.SirList.Count);
        }
    }
}
