using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.Synthesis;
using System.IO;
using GemBox.Pdf;
using Microsoft.Office.Interop.Word;
using System.Runtime.InteropServices;
using System.Xml;




namespace Text_to_Speech
{
    public partial class Form1 : Form
    {
       
        SpeechSynthesizer synthVoice;
        bool isStopped;
       
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            synthVoice = new SpeechSynthesizer();

            foreach (InstalledVoice voice in synthVoice.GetInstalledVoices())
            {
                VoiceInfo infoVoice = voice.VoiceInfo;
                cmbVoice.Items.Add(infoVoice.Name);
                
            }
            synthVoice.Dispose();

        }

        private void BtnSpeech_Click(object sender, EventArgs e)
        {
            string voice = cmbVoice.Text;
            string theText = TxtSpeech.Text;
            if (voice == "Select Voice")
            {

                MessageBox.Show("Select a Voice");
                return;

            }

            if (theText == "")
            {

                MessageBox.Show("Type some text");
                return;

            }

            synthVoice = new SpeechSynthesizer();
            synthVoice.SetOutputToDefaultAudioDevice();
            synthVoice.SelectVoice(voice);
            synthVoice.Rate = trackBar1.Value;
            synthVoice.Volume = trackBar2.Value;
            synthVoice.SpeakAsync(theText);
            isStopped = false;
        }
        private void Form1_FormClosing(object sender, EventArgs e)
        {
            synthVoice.Dispose();

        }

        private void BtnPause_Click(object sender, EventArgs e)
        {
            if (isStopped == false)
            {

                if (synthVoice.State == SynthesizerState.Speaking)
                {

                    synthVoice.Pause();

                }

            }
        }

        private void BtnResume_Click(object sender, EventArgs e)
        {
            if (isStopped == false)
            {
                if (synthVoice.State == SynthesizerState.Paused)
                {

                    synthVoice.Resume();

                }
            }
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            if (synthVoice != null)
            {

                synthVoice.Dispose();
                isStopped = true;
            }
        }

        private void BtnOpenFile_Click(object sender, EventArgs e)
        {

            ofd1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            ofd1.Title = "Open File";
            ofd1.Filter = "Text Files|*.txt|Word Files|*.docx|PDF Files|*.pdf";

            if (ofd1.ShowDialog() != DialogResult.Cancel)
            {
                string fileName = ofd1.FileName;
                string ending = Path.GetExtension(fileName);

                if (ending == ".txt")
                {

                    GetTextFile(fileName);

                }
                else if (ending == ".docx")
                {

                    GetWordFile(fileName);


                }
                else if (ending == ".pdf")
                {

                    GetPdfFile(fileName);

                }
                else
                {

                    MessageBox.Show("Error");

                }
            }
        }
        private void GetTextFile(string filePath)
        {
            StreamReader objReader = new StreamReader(filePath);
            TxtSpeech.Text = objReader.ReadToEnd();
            objReader.Close();

        }

        private void GetPdfFile(string filePath)
        {
            TxtSpeech.Text = "";
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
            PdfDocument doc = PdfDocument.Load(filePath);
            int numOfPages = doc.Pages.Count;

            if (numOfPages <= 2)
            {
                foreach (var page in doc.Pages)
                {

                  TxtSpeech.Text += page.Content.ToString();

                }
                doc.Close();
            }
            else
            {

                MessageBox.Show("Too many pages");

            }

        }
        private void GetWordFile(string filePath)
        {
            TxtSpeech.Text = "";
            Microsoft.Office.Interop.Word.Application word = new Microsoft.Office.Interop.Word.Application();
            Document doc = word.Documents.Open(filePath);
            TxtSpeech.Text = doc.Content.Text.ToString();
            int paras = doc.Paragraphs.Count;
            for (int i = 1; i <= paras; i++)
            {
                string temp = doc.Paragraphs[i].Range.Text.Trim();
                TxtSpeech.Text += temp + "\r\n";
            }
            if (doc != null)
            {

                doc.Close();
                Marshal.ReleaseComObject(doc);

            }

            if (word != null)
            {

                word.Quit();
                Marshal.ReleaseComObject(word);

            }
        }

        private void BtnPronounce_Click(object sender, EventArgs e)
        {
            string voice = cmbVoice.Text;

            if (voice == "Select Voice")
            {

                MessageBox.Show("Select a Voice");
                return;

            }

            if (TxtSpeech.Text.Trim() == "")
            {

                MessageBox.Show("Type some text");
                return;

            }

            if (TxtSpeech.SelectionLength > 0)
            {

                string sel = TxtSpeech.SelectedText;
                SpeechSynthesizer syn = new SpeechSynthesizer();
                syn.SetOutputToDefaultAudioDevice();
                syn.SelectVoice(voice);
                syn.Speak(sel.Trim());
                syn.Dispose();

            }
        }

        private void btnURL_Click(object sender, EventArgs e)
        {
            string webPage = txtUrl.Text;

            WebBrowser wb = new WebBrowser();
            wb.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(DisplayText);

            wb.Url = new Uri(webPage);
        }

        private void DisplayText(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            WebBrowser wb = (WebBrowser)sender;
            wb.Document.ExecCommand("SelectAll", false, null);
            wb.Document.ExecCommand("Copy", false, null);
            TxtSpeech.Text = Clipboard.GetText();

            wb.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                trackBar1.Value = -10;

            }
            if (comboBox1.SelectedIndex == 1)
            {
                trackBar1.Value = 0;

            }
            if (comboBox1.SelectedIndex == 2)
            {
                trackBar1.Value = 10;

            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex == 0)
            {
                trackBar2.Value = 0;

            }
            if (comboBox2.SelectedIndex == 1)
            {
                trackBar2.Value = 50;

            }
            if (comboBox2.SelectedIndex == 2)
            {
                trackBar2.Value = 100;

            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            ofd1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            ofd1.Title = "Open File";
            ofd1.Filter = "Text Files|*.txt|Word Files|*.docx|PDF Files|*.pdf";

            if (ofd1.ShowDialog() != DialogResult.Cancel)
            {
                string fileName = ofd1.FileName;
                string ending = Path.GetExtension(fileName);


                if (comboBox3.SelectedIndex == 0)
                {
                    if (ending == ".txt")
                    {

                        GetTextFile(fileName);

                    }

                }
                if (comboBox3.SelectedIndex == 1)
                {
                    if (ending == ".pdf")
                    {

                        GetPdfFile(fileName);

                    }

                }
                if (comboBox3.SelectedIndex == 2)
                {
                    if (ending == ".docx")
                    {

                        GetWordFile(fileName);


                    }

                }
            }
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            string webPage = txtUrl.Text;

            WebBrowser wb = new WebBrowser();
            wb.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(DisplayText);

           



            if (comboBox4.SelectedIndex == 0)
            {
                txtUrl.Text = "https://www.nsuok.edu";
             

            }
            if (comboBox4.SelectedIndex == 1)
            {
                txtUrl.Text = "https://www.newson6.com";
                
            }
            if (comboBox4.SelectedIndex == 2)
            {
                txtUrl.Text = "https://www.usatoday.com";
                

            }
        }

        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ofd1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            ofd1.Title = "Open File";
            ofd1.Filter = "Text Files|*.txt|Word Files|*.docx|PDF Files|*.pdf";

            if (ofd1.ShowDialog() != DialogResult.Cancel)
            {
                string fileName = ofd1.FileName;
                string ending = Path.GetExtension(fileName);

                if (ending == ".txt")
                {

                    GetTextFile(fileName);

                }
                else if (ending == ".docx")
                {

                    GetWordFile(fileName);


                }
                else if (ending == ".pdf")
                {

                    GetPdfFile(fileName);

                }
                else
                {

                    MessageBox.Show("Error");

                }
            }
        }

        private void getUrlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string webPage = txtUrl.Text;

            WebBrowser wb = new WebBrowser();
            wb.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(DisplayText);

            wb.Url = new Uri(webPage);
        }

        private void davidToolStripMenuItem_Click(object sender, EventArgs e)
        {



            cmbVoice.SelectedIndex = 0;
            
                
               
        }

        private void ziraToolStripMenuItem_Click(object sender, EventArgs e)
        {

            cmbVoice.SelectedIndex = 1;
        }

        private void speakToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string voice = cmbVoice.Text;
            string theText = TxtSpeech.Text;
            if (voice == "Select Voice")
            {

                MessageBox.Show("Select a Voice");
                return;

            }

            if (theText == "")
            {

                MessageBox.Show("Type some text");
                return;

            }

            synthVoice = new SpeechSynthesizer();
            synthVoice.SetOutputToDefaultAudioDevice();
            synthVoice.SelectVoice(voice);
            synthVoice.Rate = trackBar1.Value;
            synthVoice.Volume = trackBar2.Value;
            synthVoice.SpeakAsync(theText);
            isStopped = false;
        }

        private void pauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isStopped == false)
            {

                if (synthVoice.State == SynthesizerState.Speaking)
                {

                    synthVoice.Pause();

                }

            }
        }

        private void resumeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isStopped == false)
            {
                if (synthVoice.State == SynthesizerState.Paused)
                {

                    synthVoice.Resume();

                }
            }
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (synthVoice != null)
            {

                synthVoice.Dispose();
                isStopped = true;
            }
        }

        private void slowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            trackBar1.Value = -10;
        }

        private void normalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            trackBar1.Value = 0;

        }

        private void fastToolStripMenuItem_Click(object sender, EventArgs e)
        {
            trackBar1.Value = 10;
        }

        private void lowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            trackBar2.Value = 0;
        }

        private void normalToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            trackBar2.Value = 50;
        }

        private void maximumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            trackBar2.Value = 100;
        }

        private void instructionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 help = new Form2();
            help.ShowDialog();
        }

        private void licenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 help = new Form3();
            help.ShowDialog();
        }
    }
}
