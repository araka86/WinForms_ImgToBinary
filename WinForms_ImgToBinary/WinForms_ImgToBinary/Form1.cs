using System;
using System.Diagnostics;
using System.Net;
using System.Security.Policy;
using System.Windows.Forms;

using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace WinForms_ImgToBinary
{
    public partial class Form1 : Form
    {
        Bitmap image;
        string base64Text;
        FileStream fs;
        string path = @"D:\tes\";


        public Form1()
        {
            InitializeComponent();
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private async void button1_Click(object sender, EventArgs e)
        {


            if (txtLoad.Text == "" || txtLoad.Text == null || txtLoad.Text == String.Empty)
            {
                label1.Text = "Input the link Image!!! Or Click button upload All";
            }
            else
            {

                WebRequest request = WebRequest.Create(txtLoad.Text);

                using (var response = request.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        pictureBox2.Image = Image.FromStream(stream);   /////////////////output URL

                        Random random = new Random();
                        //using (HttpClient clienttt = new HttpClient())
                        //{
                        //    using (HttpResponseMessage responseee = await client.GetAsync("https://altstar.com.ua/image/category_image/000000134.jpg"))
                        //    using (Stream streamToReadFrom = await responseee.Content.ReadAsStreamAsync())
                        //    {
                        //        byte[] buffer = new byte[streamToReadFrom.Length];
                        //        FileStream fs;
                        //        using (fs = new FileStream(@$"D:\tes\{Guid.NewGuid().ToString()}.jpg", FileMode.Append)) // path
                        //        {
                        //            var ResponseTask = responseee.Content.CopyToAsync(fs); //save to path
                        //            ResponseTask.Wait(500);
                        //        }
                        //        buffer = File.ReadAllBytes(fs.Name); //convert to byte
                        //    }
                        //}
                        //////////////////////////V2.0/////////////////////////////////
                        using (HttpClient clienttt = new HttpClient())
                        {
                            using (HttpResponseMessage responseee = await clienttt.GetAsync(txtLoad.Text)) //Url


                            using (fs = new FileStream($"{path}{Guid.NewGuid()}.jpg", FileMode.Append)) // path
                            {
                                var ResponseTask = responseee.Content.CopyToAsync(fs); //save to path
                                                                                       // ResponseTask.Wait(500);
                            }
                        }

                        byte[] buffer = File.ReadAllBytes(fs.Name); //convert to byte
                        string[] bit = new string[buffer.Length];
                        for (int i = 0; i < buffer.Length; i++)
                            bit[i] = buffer[i].ToString();


                        foreach (var item in bit)
                            listBox1.Items.Add(item);



                        string convertbuffer = Convert.ToBase64String(buffer);
                        string imgDataURL = string.Format("data:image/png;base64,{0}", convertbuffer);

                        //var webClient = new WebClient();
                        //byte[] imageBytes = webClient.DownloadData("https://altstar.com.ua/image/category_image/000000134.jpg"); //dounload to byte
                        //string imreBase64Data = Convert.ToBase64String(imageBytes);
                        //string imgDataURL = string.Format("data:image/png;base64,{0}", imreBase64Data);

                        richTextBox1.Text = imgDataURL;
                        pictureBox1.Image = ConvertBase64ToImage(convertbuffer);
                        stream.Dispose();
                    }

                    response.Dispose();
                }
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {

            richTextBox1.Text = "";
            pictureBox1.Image = null;
            pictureBox2.Image = null;
            listBox1.Items.Clear();


            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image Files(*.BMP;*.JPG;*.PNG)|*.BMP;*.JPG;*.PNG" +
            "|All files(*.*)|*.*";
            dialog.CheckFileExists = true;
            dialog.Multiselect = false;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                image = new Bitmap(dialog.FileName);
                pictureBox1.Image = image;

                byte[] imageArray = File.ReadAllBytes(dialog.FileName);
                base64Text = Convert.ToBase64String(imageArray); //base64Text must be global but I'll use  richtext
                richTextBox1.Text = base64Text;
            }
        }


        public Image ConvertBase64ToImage(string base64String)
        {
            byte[] imageBytes = Convert.FromBase64String(base64String);
            using (MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
            {
                ms.Write(imageBytes, 0, imageBytes.Length);
                return Image.FromStream(ms, true);
            }
        }


    }
}