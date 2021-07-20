using System;
using System.Windows.Forms;
using System.Speech;
using System.Speech.Synthesis;
using System.IO;
using System.Globalization;

namespace TextToSpeechConverter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //SpeechSynthesizer Class Provides access to the functionality of an installed a speech synthesis engine.
        SpeechSynthesizer speechSynthesizerObj;
        private void Form1_Load(object sender, EventArgs e)
        {
            speechSynthesizerObj = new SpeechSynthesizer();
            btn_Resume.Enabled = false;
            btn_Pause.Enabled = false;
            btn_Stop.Enabled = false;
        }
        

        private void btn_Speak_Click(object sender, EventArgs e)
        {
            //Disposes the SpeechSynthesizer object 
            speechSynthesizerObj.Dispose();
            if(richTextBox1.Text!="")
            {
                speechSynthesizerObj = new SpeechSynthesizer();
                //Asynchronously speaks the contents present in RichTextBox1
                speechSynthesizerObj.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Teen, 0, CultureInfo.GetCultureInfo("en-GB"));
                speechSynthesizerObj.SpeakAsync(richTextBox1.Text);
     
                btn_Pause.Enabled = true;
                btn_Stop.Enabled = true;              
            }
        }
       
        private void btn_Pause_Click(object sender, EventArgs e)
        {
            if(speechSynthesizerObj!=null)
            {
                //Gets the current speaking state of the SpeechSynthesizer object.
                if(speechSynthesizerObj.State==SynthesizerState.Speaking)
                {
                    //Pauses the SpeechSynthesizer object.
                    speechSynthesizerObj.Pause();
                    btn_Resume.Enabled = true;
                    btn_Speak.Enabled = false;
                }
            }
        }

        private void btn_Resume_Click(object sender, EventArgs e)
        {
            if (speechSynthesizerObj != null)
            {
                if (speechSynthesizerObj.State == SynthesizerState.Paused)
                {
                    //Resumes the SpeechSynthesizer object after it has been paused.
                    speechSynthesizerObj.Resume();
                    btn_Resume.Enabled = false;
                    btn_Speak.Enabled = true;
                }
            }
        }

        private void btn_Stop_Click(object sender, EventArgs e)
        {
            if(speechSynthesizerObj!=null)
            {
                //Disposes the SpeechSynthesizer object 
                speechSynthesizerObj.Dispose();
                btn_Speak.Enabled = true;
                btn_Resume.Enabled = false;
                btn_Pause.Enabled = false;
                btn_Stop.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            speechSynthesizerObj.Dispose();
            if (richTextBox1.Text != "")
            {
                string result = TextToSpeech(richTextBox1.Text);
                string base64Text = "data:audio/wav;base64,"+ result;
                Clipboard.SetText(base64Text);
                MessageBox.Show("Text Converted To Audio and Copied Stream");

            }
               
        }
        public string TextToSpeech(string text)
        {
            MemoryStream audioStream = new MemoryStream();
            string result;
            using (var synthesizer = new SpeechSynthesizer())
            {
                synthesizer.SetOutputToWaveStream(audioStream);
                synthesizer.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Teen, 0, CultureInfo.GetCultureInfo("en-GB"));
                synthesizer.Speak(text);
                byte[] bytes = audioStream.ToArray();
                result = Convert.ToBase64String(bytes);
            }
            return result;
         }
    }
}
