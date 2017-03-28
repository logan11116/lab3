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
using System.IO;
using System.Text.RegularExpressions;

namespace lab3
{
    public partial class Form1 : Form

    {
        string Url = "http://facebook.com";
        string postUrl = "http://www.contoso.com/PostAccepter.aspx ";
        string postData = "This is a test that posts this string to a Web server.";


        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 1;
            textBox1.Text = "http://facebook.md/";
          //  textBox3.Visible = false;
            textBox3.Text = "10";
          //  label3.Visible = false;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            if (comboBox1.SelectedIndex == 2)
            {
                Url = textBox1.Text;
                textBox1.Text = postUrl;
                textBox2.Text = postData;
                label1.Visible = true;
                textBox2.Visible = true;
                textBox3.Visible = false;
                label3.Visible = false;
            }
            else if (comboBox1.SelectedIndex == 0)
            {
               
                    textBox1.Text = Url;
                    label2.Visible = false;
                    textBox2.Visible = false;
                    textBox3.Visible = false;
                    label3.Visible = false;
                
            }
            else if (comboBox1.SelectedIndex == 4)
            {
               
                    textBox1.Text = Url;
                    textBox3.Visible = true;
                    label3.Visible = true;
                label2.Visible = false;
                textBox2.Visible = false;
                     
                
            }
            else if (comboBox1.SelectedIndex == 3)
            {

                textBox1.Text = Url;
                textBox3.Visible = false;
                label3.Visible = false;
                textBox2.Visible = false;
                label2.Visible = false;

            }
            else if (comboBox1.SelectedIndex == 1)
            {

                textBox1.Text = Url;
                textBox3.Visible = false;
                label3.Visible = false;
                textBox2.Visible = false;
                label2.Visible = false;

            }

        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!textBox1.Text.StartsWith("http://") && !textBox1.Text.StartsWith("ftp:/") && !textBox1.Text.StartsWith("https://")) textBox1.Text = "http://" + textBox1.Text;

            if (comboBox1.SelectedIndex == 0)
            {
                try
                {
                    WebRequest request = WebRequest.Create(textBox1.Text);
                    request.Credentials = CredentialCache.DefaultCredentials;
                    WebResponse response = request.GetResponse();
                    richTextBox1.AppendText("Status " + ((HttpWebResponse)response).StatusDescription + "\n\n");
                    Stream dataStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(dataStream);
                    richTextBox1.AppendText(reader.ReadToEnd());
                    reader.Close();
                    response.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }

            }
            else if (comboBox1.SelectedIndex == 1)
            {
                try
                {
                    WebRequest request = WebRequest.Create(textBox1.Text);
                    request.Credentials = CredentialCache.DefaultCredentials;
                    request.Method = "HEAD";
                    WebResponse response = request.GetResponse();
                    richTextBox1.AppendText("Status " + ((HttpWebResponse)response).StatusDescription + "\n\n");
                    richTextBox1.AppendText(response.Headers.ToString());
                    response.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }

            }
            else if (comboBox1.SelectedIndex == 2)
            {
                try
                {
                    postData = textBox2.Text;
                    WebRequest request = WebRequest.Create(postUrl);
                    request.Method = "POST";
                    byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                    request.ContentType = "application/x-www-form-urlencoded"; //multipart/form-data  
                    request.ContentLength = byteArray.Length;
                    Stream dataStream = request.GetRequestStream();
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Close();
                    WebResponse response = request.GetResponse();
                    richTextBox1.AppendText("Status " + ((HttpWebResponse)response).StatusDescription + "\n\n");
                    dataStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(dataStream);
                    richTextBox1.AppendText(reader.ReadToEnd());
                    reader.Close();
                    dataStream.Close();
                    response.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }

            else if (comboBox1.SelectedIndex == 3)
            {
                try
                {
                    if (richTextBox1.Text != "") richTextBox1.Text = "";
                    if (!textBox1.Text.StartsWith("http://") && !textBox1.Text.StartsWith("ftp:/") && !textBox1.Text.StartsWith("https://")) textBox1.Text = "http://" + textBox1.Text;
                    if (label2.Text != "Result:") label2.Text = "Result:";
                    WebRequest request = WebRequest.Create(textBox1.Text);
                    WebResponse response = request.GetResponse();
                    Stream stream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(stream);
                    string status = ((HttpWebResponse)response).StatusDescription;
                    richTextBox1.Text = "Checking>.. " + textBox1.Text + " Status " + status + "\n";

                    string pattern = @"(((http|https|ftp)+\:/\/)[&#95;a-z0-9\/&#95;:@=.+?,##%&~-]*[^.|\'|""|\# |!|\(|?|,| |>|<|;|\)])";

                    MatchCollection numberoflinks = Regex.Matches(reader.ReadToEnd(), pattern, RegexOptions.Singleline);
                    label2.Text = "Results found " + numberoflinks;

                    int counter = 0;
                    foreach (Match m in numberoflinks)
                    {
                        string link = m.Groups[1].Value;
                        try
                        {
                            WebRequest newrequest = WebRequest.Create(link);
                            WebResponse newresponse = newrequest.GetResponse();
                            richTextBox1.AppendText("\n" + link + "  Status " + ((HttpWebResponse)newresponse).StatusCode);
                            newresponse.Close();
                            counter++;
                            label2.Text = "Results found " + counter;
                        }
                        catch (Exception ex)
                        {
                            richTextBox1.AppendText("\n" + link + " " + ex.Message);
                            counter++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }


            }

            else if (comboBox1.SelectedIndex == 4)
            {
              //  textBox3.Visible = true;
               // label3.Visible = true;
                if (!textBox1.Text.StartsWith("http://") && !textBox1.Text.StartsWith("ftp://") && !textBox1.Text.StartsWith("https://")) textBox1.Text = "http://" + textBox1.Text;
                for (int i = 0; i < Convert.ToInt32(textBox3.Text); i++)
                {
                    WebRequest newRequest = WebRequest.Create(Url);
                    WebResponse newResponse = newRequest.GetResponse();
                    Stream newStream = newResponse.GetResponseStream();
                    StreamReader newReader = new StreamReader(newStream);
                    richTextBox1.AppendText("\n" + Url + "  Status " + ((HttpWebResponse)newResponse).StatusCode);

                    string pattern = @"(((http|https|ftp)+\:/\/)[&#95;a-z0-9\/&#95;:@=.+?,##%&~-]*[^.|\'|""|\# |!|\(|?|,| |>|<|;|\)])";
                    MatchCollection links = Regex.Matches(newReader.ReadToEnd(), pattern, RegexOptions.Singleline);

                index:
                    Random rand = new Random();
                    Url = links[rand.Next(links.Count)].Value;
                    if (((HttpWebResponse)newResponse).StatusCode.ToString() != "OK") goto index;

                    newResponse.Close();
                    newReader.Close();



                }
            }
        }




        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
