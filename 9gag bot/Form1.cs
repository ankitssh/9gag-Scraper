using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
namespace _9gag_bot
{
    public partial class Form1 : Form
    {
        BackgroundWorker wb = new BackgroundWorker();
                       
        string sourceCode;
        
        public Form1()
        {
            InitializeComponent();
            progressBar1.Minimum = 0;
            progressBar1.Maximum =int.Parse( textBox2.Text);
            total.Text="0/"+textBox2.Text;
            textBox2.TextChanged += textBox2_TextChanged;
           // wb.DoWork += wb_DoWork;
        }

        void textBox2_TextChanged(object sender, EventArgs e)
        {
            total.Text = "0/" + textBox2.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fb = new FolderBrowserDialog();
           
            if (fb.ShowDialog() == DialogResult.OK)
            {

                for (int i = 0; i <int.Parse( textBox2.Text); i++)
                {


                    string url = textBox1.Text;



                    WebClient wc = new WebClient();
                    try
                    {
                        Parallel.Invoke(() =>   sourceCode = wc.DownloadString(url));
                      
                    }
                    catch (Exception ex) { }

                    int headerIndexStart = sourceCode.IndexOf("<h2 class=\"badge-item-title\">")+29;
                    int headerEndIndex = sourceCode.IndexOf("<",headerIndexStart);
                    string header = sourceCode.Substring(headerIndexStart,headerEndIndex-headerIndexStart);
                    if (header.Contains("&#039;"))
                    header = header.Replace("&#039;","'");
                    

                    int startIndex = sourceCode.IndexOf("<img class=\"badge-item-img\" src=\"") + 33;
                    int endIndex = sourceCode.IndexOf("\"", startIndex);

                    string picUrl = sourceCode.Substring(startIndex, endIndex - startIndex);
                    string extension = picUrl.Substring(picUrl.Length - 4);
                    picUrl = picUrl.Substring(0, picUrl.Length - 4);
                    try
                    {
                        wc.DownloadFileAsync(new Uri(picUrl + extension), fb.SelectedPath+"//" + header + extension);
                     
                        
                      
                    }
                        
                    catch (Exception ex) { }
                    finally
                    {
                        Parallel.Invoke(()=>   total.Text = i + "/" + textBox2.Text);
                        total.Refresh();
                     

                        progressBar1.Value = progressBar1.Value + 1;   
              
                        startIndex = sourceCode.IndexOf("badge-fast-entry badge-next-post-entry next");
                        startIndex = sourceCode.IndexOf("href=", startIndex) + 6;

                        endIndex = sourceCode.IndexOf("\"", startIndex);

                        string nextUrl = sourceCode.Substring(startIndex, endIndex - startIndex);
                       textBox1.Text= "https://9gag.com" + nextUrl;
                    }
                }

                MessageBox.Show("Download Completed!");
            
            }



            progressBar1.Value = 0;
            
        }

       
     

        private void textBox1_Click(object sender, EventArgs e)
        {
            textBox1.SelectAll();
        }
    }
}
