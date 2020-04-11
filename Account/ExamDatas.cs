using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemiProvimeveOnline.Account
{
    //TODO swap class names with questions, and fill ncessary data type inside classes
    public class ExamDatas
    {
        public DateTime ExamDate { get; set; }
        public string StudentId { get; set; }
        public int ImplementationsWithoutErrors { get; set; }
        public List<Questions> Questions { get; set; }

        public ExamDatas()
        {
            Questions = new List<Questions>();
        }
        public override string ToString()
        {
            //string questionsDetails="";
            //foreach (var item in Questions)
            //    questionsDetails += item.ToString();

            return "Exam date: " + ExamDate.ToString() + "\nStudent: " + StudentId + "\nCorrect Implementations: " + ImplementationsWithoutErrors + "\nQuestions details:\n---------------------" + Questions.ToString();
        }
    }
    public class Questions
    {
        public string QuestionId { get; set; }
        public string FileName { get; set; }
        public string QuestionText { get; set; }
        public List<dynamic> Parameters { get; set; }
        public List<dynamic> ExpectedResults { get; set; }
        public List<dynamic> FinalResults { get; set; }

        public Questions()
        {
            Parameters = new List<dynamic>();
            ExpectedResults = new List<dynamic>();
            FinalResults = new List<dynamic>();
        }
        public override string ToString()
        {
            //string parameters="", expectedResults="", finalResults="";
            //foreach (var item in Parameters)
            //    parameters += item.ToString();
            //foreach (var item in ExpectedResults)
            //    expectedResults += item.ToString();
            //foreach (var item in FinalResults)
            //    finalResults += item.ToString();

            return string.Format("\nQuestion ID: {0}\nFileName: {1}\nQuestion Text: {2}\nParameters: {3}\nExpected Results {4}\nFinal Results: {5}",QuestionId,FileName,QuestionText,Parameters.ToString(),ExpectedResults.ToString(),FinalResults.ToString());
        }
    }
}