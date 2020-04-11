using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace SistemiProvimeveOnline.Account
{
    public partial class ExamPage : System.Web.UI.Page
    {
        ExamDatas exam = new ExamDatas();
        Questions question = new Questions();

        int questionsForThisExam = 5, questionsAskedSoFar = 0;

        Process cmd;
        string username;
        string professorUsername;//behet update varesisht prej profesorit
        DirectoryInfo studentFilesDirectory, baseFilesDirectory;
        string fileName, fileLocation;
        #region buttonClicksAndPageLoad
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
                implementBox.Focus();
                SetGlobalVariables();
                Submit.Visible = false;
                exam.ExamDate = DateTime.Now;
                LoadQuestion(++questionsAskedSoFar);
                UpdateFormWithQuestionsData();
            //}
        }

        void UpdateFormWithQuestionsData()
        {
            detyra.Text = question.QuestionText;
        }

        protected void Submit_Click(object sender, EventArgs e)
        {
            dynamic param = question.Parameters;
            SubmitCode(param);
        }

        protected void Compile_Click(object sender, EventArgs e)
        {
            Compile.Enabled = false;
            CompileCode();
        }
        #endregion
        protected bool CompileCode()
        {
            errorMsg.Visible = false;
            compSuccessMsg.Visible = false;
            
            //fileName = "HelloWorld";
            //string question = GetRandomQuestionID(1);
            fileLocation = studentFilesDirectory.FullName;
            WriteProgramFile(baseFilesDirectory.FullName + "\\" + question.QuestionId,fileLocation + "\\" + question.QuestionId, fileName);

            string cmdCompileCommand = string.Format("/c cd {0} && javac {1}.java", fileLocation + "\\" + question.QuestionId, fileName).Trim();

            cmd = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.UseShellExecute = false;
            startInfo.WorkingDirectory = fileLocation;
            startInfo.FileName = "C:\\WINDOWS\\system32\\cmd.exe";
            startInfo.Arguments = cmdCompileCommand;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;

            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;

            cmd.StartInfo = startInfo;
            cmd.Start();

            //string output = cmd.StandardOutput.ReadToEnd();
            string outputError = cmd.StandardError.ReadToEnd();

            if (!cmd.WaitForExit(milliseconds: 10000))
            {
                cmd.Kill();
                Debug.WriteLine("CMD took too long to finish. Just killed it!");
            }

            //if (output.Length != 0)
            //    Debug.WriteLine(output);
            Compile.Enabled = true;
            if (outputError.Length != 0)
            {
                StringBuilder formatedError = new StringBuilder();
                for (int i = 0; i < outputError.Length; i++)
                {
                    if (outputError[i].CompareTo('\n') == 0)
                        formatedError.Append("<br />");
                    else if (char.IsWhiteSpace(outputError[i]))
                        formatedError.Append("&nbsp");
                    else formatedError.Append(outputError[i]);
                }
                errorMsg.Text = "Kompajlimi deshtoi. Gabimi eshte:<br />------------------<br />" + formatedError;
                errorMsg.Visible = true;
                Submit.Visible = false;
                Debug.WriteLine(outputError);
                return false;
            }
            else
            {
                compSuccessMsg.Visible = true;
                Submit.Visible = true;
                return true;
            }
        }
        protected void SubmitCode(dynamic param)
        {
            fileLocation = studentFilesDirectory.FullName;
            string runArguments = "";
            int paramId = 0;
            do
            {
                string outputError = string.Empty, output = string.Empty;

                if (param != null && param.Count > paramId)
                {
                    runArguments = param[paramId].ToString();
                    //question.Parameters.Add(param[paramId]);
                }

                string cmdCompileCommand = paramId == 0 ?
                    string.Format("/c cd {0} && javac {1}.java && java {1} {2}", fileLocation, fileName, runArguments).Trim() :
                    string.Format("/c cd {0} && java {1} {2}", fileLocation, fileName, runArguments).Trim();

                Debug.WriteLine("cmd command " + cmdCompileCommand);

                cmd = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.UseShellExecute = false;
                startInfo.WorkingDirectory = fileLocation;
                startInfo.FileName = "C:\\WINDOWS\\system32\\cmd.exe";
                //startInfo.Verb = "runas";
                startInfo.Arguments = cmdCompileCommand;
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;

                startInfo.RedirectStandardOutput = true;
                startInfo.RedirectStandardError = true;

                cmd.StartInfo = startInfo;
                cmd.Start();

                output = cmd.StandardOutput.ReadToEnd();
                outputError = cmd.StandardError.ReadToEnd();

                //wait 10 seconds for cmd to exit, otherwise kill it
                if (!cmd.WaitForExit(milliseconds: 10000))
                {
                    cmd.Kill();
                    Debug.WriteLine("CMD took too long to finish. Just killed it!");
                }

                if(outputError.Length == 0)
                    question.FinalResults.Add(output.Length != 0 ? output : null);
                else
                {
                    question.FinalResults.Add("Exception");
                    Debug.WriteLine("output error: " + outputError);
                }
            } while (param.Count - 1 > paramId++);

            //question.FinalResults.Add(/*final reults*/);
            exam.Questions.Add(this.question);
            this.question = null;

            if (++questionsAskedSoFar <= questionsForThisExam)
            {
                LoadQuestion(questionsAskedSoFar);
                UpdateFormWithQuestionsData();
            }
            else
            {
                File.WriteAllText(fileLocation, JsonConvert.SerializeObject(exam));
            }
        }

        void WriteProgramFile(string sourceFilesDirectory, string programFileDirectory, string fileName)
        {
            StringBuilder programFileContent = new StringBuilder();
            programFileContent.Append(File.ReadAllText(sourceFilesDirectory + "\\preCode.txt"));
            programFileContent.Append(implementBox.Text);
            programFileContent.Append( File.ReadAllText(sourceFilesDirectory + "\\postCode.txt"));

            Directory.CreateDirectory(programFileDirectory);
            File.WriteAllText(programFileDirectory+"\\"+fileName+".java", programFileContent.ToString());
        }

        void SetGlobalVariables()
        {
            try{
                username = Session["email"].ToString();
            }
            catch{
                Response.Redirect("Login.aspx", true);
            }
            exam.StudentId = username;
            professorUsername = "DhurateHyseni";
            studentFilesDirectory = Directory.CreateDirectory("D:\\Desktop\\CompileTest\\" + professorUsername + "\\studentet\\" + username);
            baseFilesDirectory = Directory.CreateDirectory("D:\\Desktop\\CompileTest\\" + professorUsername + "\\pyetjet");
        }

        void LoadQuestion(int questionNo)
        {
            string question = GetRandomQuestionID(questionNo);
            this.question = JsonConvert.DeserializeObject<Questions>(File.ReadAllText(baseFilesDirectory.FullName + "\\" + question+"\\HelloWorld.json"));
            fileName = this.question.FileName;
        }

        string GetRandomQuestionID(int questionNo)
        {
            string[] files = Directory.GetFileSystemEntries(baseFilesDirectory.FullName);
            if (files.Length == 0) throw new Exception("No files were found");

            List<string> questions = new List<string>();
            for (int i = 0; i < files.Length; i++)
            {
                files[i] = Path.GetFileNameWithoutExtension(files[i]);
                int questionSifres = 0;

                // prevents getting a number that starts with value of questionNo
                //ex if we want question with nnumber 1, 
                //this prevents to also take number 10
                for (int j = 1; ; j++)
                    if (files[i][j].CompareTo('_') == 0)
                        break;
                    else questionSifres++;
                if (questionSifres > questionNo.ToString().Length)
                    continue;
                //end

                if (string.Compare(files[i], 1, questionNo.ToString(), 0, questionNo.ToString().Length) == 0)
                    questions.Add(files[i]);
            }
            return questions.Count==0 ? throw new Exception("No files were found with your specified criteria.") : questions[new Random().Next(0,questions.Count)];
        }
    }
}