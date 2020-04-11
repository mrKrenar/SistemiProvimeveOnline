using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;

namespace SistemiProvimeveOnline.Account
{
    public partial class RegisterQuestions : System.Web.UI.Page
    {
        Questions question;
        string professorUsername = "DhurateHyseni";
        string baseDirectory;
        string newFileName = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            idPyetjes.Focus();
            baseDirectory = "D:\\Desktop\\CompileTest\\" + professorUsername + "\\pyetjet";
            question = new Questions();
        }

        protected void register_Click(object sender, EventArgs e)
        {
            int nrPyetjes;
            if (idPyetjes.Text.Length == 0 || emriFajllit.Text.Length == 0 || pyetja.Text.Length == 0 || rezultati.Text.Length == 0)
            {
                errorMsg.Text = "Fushat e shenuara me * jane obligative";
                errorMsg.Visible = true;
                return;
            }
            try{
                nrPyetjes = int.Parse(idPyetjes.Text);
            }
            catch{
                return;
            }

            newFileName = FindNewFileName(nrPyetjes);
            //new file name duhet me kthye veq p1_1
            if (!MakeQuestionObject())
            {
                errorMsg.Text = "Rishiko te dhenat qe i ke shenuar!";
                errorMsg.Visible = true;
            }
            string path = Path.Combine(baseDirectory, newFileName);
            Directory.CreateDirectory(path);
            File.WriteAllText(path +"\\"+emriFajllit.Text+".json",(JsonConvert.SerializeObject(question, Formatting.Indented)));
            File.WriteAllText(path + "\\preCode.txt", preCode.Text);
            File.WriteAllText(path + "\\postCode.txt", postCode.Text);
            successMsg.Text = "Detyra me id " + newFileName + " u regjistrua!";
            ClearForm();
        }

        void ClearForm()
        {
            idPyetjes.Text = string.Empty;
            emriFajllit.Text = string.Empty;
            pyetja.Text = string.Empty;
            parametrat.Text = string.Empty;
            rezultati.Text = string.Empty;
            preCode.Text = string.Empty;
            postCode.Text = string.Empty;

            errorMsg.Visible = false;
            successMsg.Visible = true;
        }

        bool MakeQuestionObject()
        {
            try
            {
                question.FileName = emriFajllit.Text.Trim();
                question.QuestionId = newFileName.Trim();
                question.QuestionText = pyetja.Text.Trim();
                question.FinalResults = new List<dynamic>();

                var param = parametrat.Text.Trim().Split(',');
                foreach (var item in param)
                    question.Parameters.Add(item.Trim());

                var rezultatet = rezultati.Text.Split(',');
                foreach (var item in rezultatet)
                    question.ExpectedResults.Add(item.Trim());
            }
            catch (Exception e)
            {
                Debug.WriteLine("EXCEPTION\n"+e.StackTrace);
                return false;
            }
            return true;
        }

        string FindNewFileName(int questionNo)
        {
            string lastFileName="";
            string[] files = Directory.GetFileSystemEntries(baseDirectory);
            if (files.Length == 0)
                return "p" + questionNo + "_1";

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
                    lastFileName = files[i];
            }
            if (lastFileName.Trim().Length == 0)
                return "p" + questionNo + "_" + (1);

            int nextNr = int.Parse(lastFileName.Substring(lastFileName.LastIndexOf('_')+1));

            return "p" + questionNo + "_" + (nextNr + 1);
        }

    }
}