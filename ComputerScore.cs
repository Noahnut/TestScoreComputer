
using System.Xml;
using System;
using System.Threading.Tasks;
using System.Diagnostics;
namespace ComputerScore
{
    public class ScoreComputer
    {
        public int SendTestPaper(string testPaperName)
        {
            int score = 0;
            XmlDocument TesterPaper = new XmlDocument();
            XmlDocument AnswerPaper = new XmlDocument();
            try
            {
                var testPaperFilePosition = "test/" + testPaperName + ".xml";
                var testNumber = _testerNumber(testPaperName);
                TesterPaper.Load(testPaperFilePosition);
                AnswerPaper = _getAnswerDocument(testPaperName.Substring(0, testPaperName.LastIndexOf('_')));
                score = _scoreComputer(TesterPaper, AnswerPaper);
            }
            catch (System.IO.FileNotFoundException)
            {
                Console.WriteLine("Your test paper not found Please Check again");
            }

            catch (System.Exception e)
            {
                Console.WriteLine(e);
            }
            return score;
        }

        private int _scoreComputer(XmlDocument testerPaper, XmlDocument answerPaper)
        {
            int score = 0;
            var questionNodeNumber = testerPaper.SelectNodes("/quiz/question").Count;
            var answerNodeNumber = testerPaper.SelectNodes("/quiz/question").Count;
            XmlNodeList answerNode = answerPaper.ChildNodes;
            if (!questionNodeNumber.Equals(answerNodeNumber)) throw new Exception("the AnswerPaper is not suit for the testerPaper");
            Stopwatch sw = new Stopwatch();
            sw.Start();
            /* 
            Parallel.For(0, questionNodeNumber, i => {
                 XmlElement testerItemNodeList = testerPaper.SelectNodes("/quiz/question")[i] as XmlElement;
                XmlElement answerItemNodeList = answerPaper.SelectNodes("/quiz/question")[i] as XmlElement;
                score += _questionScoreCount(testerItemNodeList, answerItemNodeList);
            });
            */
            for (int i = 0; i < questionNodeNumber; i++)
            {
                XmlElement testerItemNodeList = testerPaper.SelectNodes("/quiz/question")[i] as XmlElement;
                XmlElement answerItemNodeList = answerPaper.SelectNodes("/quiz/question")[i] as XmlElement;
                score += _questionScoreCount(testerItemNodeList, answerItemNodeList);
            }
            sw.Stop();
            TimeSpan s2 = sw.Elapsed;
            Console.WriteLine("Total waste: {0} ", s2.TotalMilliseconds);
            if (score < 0) score = 0;
            return score;
        }

        private int _questionScoreCount(XmlElement testerNodeList, XmlElement answerNodeList)
        {
            int totalScore = 0;
            int questionScore = int.Parse(answerNodeList.GetAttribute("score"));
            int itemCount = testerNodeList.SelectNodes("item").Count;
            int eachScore = questionScore / itemCount;
            int correct = 0, wrong = 0;
            Parallel.For(0, itemCount, i => {
                XmlElement TesterItem = testerNodeList.SelectNodes("item")[i] as XmlElement;
                XmlElement AnswerItem = answerNodeList.SelectNodes("item")[i] as XmlElement;
                
                if (TesterItem.GetAttribute("checked") == AnswerItem.GetAttribute("correct")) {
                     correct += 1;
                }
                else {
                    wrong += 1;
                }
            });
            /* 
            for (int i = 0; i < itemCount; i++) {
                XmlElement TesterItem = testerNodeList.SelectNodes("item")[i] as XmlElement;
                XmlElement AnswerItem = answerNodeList.SelectNodes("item")[i] as XmlElement;
                
                if (TesterItem.GetAttribute("checked") == AnswerItem.GetAttribute("correct")) {
                     correct += 1;
                }
                else {
                    wrong += 1;
                }
            }
            */
            totalScore += (correct * eachScore);
            totalScore -= (wrong * eachScore);
            if (totalScore < 0) totalScore = 0;

            return totalScore;
        }

        private string _testerNumber(string testPaperName)
        {
            var testPosition = testPaperName.LastIndexOf('_');
            var testNumberLength = testPaperName.Length - testPosition - 1;
            var testNumber = testPaperName.Substring(testPosition + 1, testNumberLength);
            return testNumber;
        }

        private XmlDocument _getAnswerDocument(string answerPaper)
        {
            XmlDocument AnswerPaper = new XmlDocument();
            var AnswerPaperFilePosition = "test/" + answerPaper + "Answer.xml";
            try
            {
                AnswerPaper.Load(AnswerPaperFilePosition);
            }
            catch (System.IO.FileNotFoundException)
            {
                Console.WriteLine("Your Answer Paper not found Please Check again");
            }
            return AnswerPaper;
        }
    }
}