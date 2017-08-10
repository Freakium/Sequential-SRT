using System;
using System.IO;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace SequentialSRT
{
    public partial class frmSequentialSRT : Form
    {
        public frmSequentialSRT()
        {
            InitializeComponent();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = @"C:\";
            openFileDialog1.Title = "Open SRT";

            openFileDialog1.CheckFileExists = true;
            openFileDialog1.CheckPathExists = true;

            openFileDialog1.DefaultExt = "srt";
            openFileDialog1.Filter = "SRT Files (*.srt)|*.srt|All Files (*.*)|*.*";
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                TBOpen.Text = openFileDialog1.FileName;
            }
        }

        private void btnBegin_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "Working...";

            lblStatus.Text = numberAssigner(TBOpen.Text, chkBoxlToI.Checked);
        }

        private string numberAssigner(string filename, bool lReplace)
        {
            string readIn;
            int number;
            int count = 1;
            bool nextSub = true;
            string directoryPath = Path.GetDirectoryName(filename);

            try
            {
                using (StreamReader reader = new StreamReader(filename))
                {
                    StreamWriter writer = new StreamWriter(directoryPath + "\\NEWmerated.srt");

                    while (reader.Peek() >= 0)
                    {
                        readIn = reader.ReadLine();

                        if (nextSub && Int32.TryParse(readIn, out number))
                        {
                            nextSub = false;
                            writer.WriteLine(count);
                            count++;
                        }
                        else if (readIn == "")
                        {
                            nextSub = true;
                            writer.WriteLine();
                        }
                        else
                        {
                            if (lReplace)
                                writer.WriteLine(lToIReplace(readIn));
                            else
                                writer.WriteLine(readIn);
                        }
                    }

                    writer.Close();
                }

                if( lReplace )
                    return "SRT Numeration / l -> I Replace Complete";
                else
                    return "SRT Numeration Complete";
            }
            catch (Exception)
            {
                return "File Does Not Exist";
            }
        }

        private string lToIReplace(string input)
        {
            string pattern = "\\bl\\b";
            string replacement = "I";

            Regex rgx = new Regex(pattern);

            return rgx.Replace(input, replacement);
        }
    }
}
