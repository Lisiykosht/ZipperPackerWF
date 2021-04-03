using System;

using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ionic.Zip;
using System.Threading;
using System.Configuration;
using System.Collections.Specialized;


namespace ZipperPackerWF
{

    public partial class Form1 : Form
    {
        private NameValueCollection sAll;
        public int time_1 = 1;
        public Form1()
        {
            InitializeComponent();

            sAll = ConfigurationManager.AppSettings;
            if (sAll.Get("delete_orig") == "1")
            { checkBox1.Checked = true; }
            else { checkBox1.Checked = false; };
            checkBox1.Text = "Удалять исходные файлы";
            button1.Text = "Пароли";

            label2.Text = time_1.ToString();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (time_1 > 0)
            { time_1 -= 1; }
            else
            {
                Doer();
                time_1 = 10;
            }

            label2.Text = time_1.ToString();
        }



        private void Doer()
        {
            ConfigurationManager.RefreshSection("appSettings");
            DirectoryInfo di_pack = new DirectoryInfo(@".\Incoming");
            DirectoryInfo di_packed = new DirectoryInfo(@".\Outgoing\");
            if (!di_pack.Exists)
            { di_pack.Create(); }

            if (!di_packed.Exists)
            { di_packed.Create(); }


            foreach (FileInfo findedFile in di_pack.GetFiles())
            {
                listBox1.Items.Add(DateTime.Now + "  ||    Пакуем :" + findedFile.Name);
                //  listBox1.Items.Add();
                string pwd = sAll.Get(findedFile.Name.Substring(0, 6));
                if (pwd!=null)
                {
                    using (ZipFile zip = new ZipFile())
                    {
                        try
                        {
                            zip.Password = pwd;
                            zip.AddFile(di_pack + @"\"+findedFile.Name).FileName = findedFile.Name;
                           
                            zip.Save(di_packed + @"\" + Path.GetFileNameWithoutExtension(findedFile.Name) + ".zip");
                           

                            if (checkBox1.Checked) { findedFile.Delete(); }

                            listBox1.Items.Add(DateTime.Now + "  ||    Файл " + findedFile.Name + " упакован c паролем "+pwd+" !");
                        }
                       catch
                        {
                            
                            listBox1.Items.Add("Сбой! Секундочку...");
                            Thread.Sleep(1000);
                        }
                    }


                }
                else 
                {
                    listBox1.Items.Add("Сбой! Пароль не задан или неправилльное имя файла!");
                }





            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Dict dictForm = new Dict();
            
            dictForm.Show();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            string attr;
            if (checkBox1.Checked) { attr = "1"; } else { attr = "0"; };

            System.Configuration.Configuration currentConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            currentConfig.AppSettings.Settings["delete_orig"].Value = attr;

            currentConfig.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        
    }
    }
}
