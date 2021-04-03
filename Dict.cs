using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Collections.Specialized;


namespace ZipperPackerWF
{
    
    public partial class Dict : Form
    {
        public NameValueCollection sAll;
        public Dict()
        {
            InitializeComponent();
        }

        private void Dict_Load(object sender, EventArgs e)
        {
            textBox1.Clear();
            label1.Text = "Код ЛПУ";
            label2.Text = "Пароль";
           
            button1.Text = "Сохранить";

            sAll = ConfigurationManager.AppSettings;

            foreach (string s in sAll.AllKeys)
            { if (s.StartsWith("15")){ textBox1.AppendText(s + " " + sAll.Get(s)+"\r\n"); } }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[,] strarr = new string[ textBox1.Lines.Length,2];
            int roww = 0;
            foreach (string s in textBox1.Lines)
            {
                string[] words = s.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                try
                {
                   
                    if (words[0].StartsWith("15") && words[0].Length == 6)
                    {
                        strarr[roww, 0] = words[0];
                        strarr[roww, 1] = words[1];
                        roww += 1;
                    }
                }
                catch
                {

                }
             

            }
            
                for (int i=0;i<roww;i++)
                {
               
                    if (!ConfigurationManager.AppSettings.AllKeys.Contains(strarr[i,0]))
                    {
                        // открываем текущий конфиг специальным обьектом
                        System.Configuration.Configuration currentConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                        // добавляем позицию в раздел AppSettings
                        currentConfig.AppSettings.Settings.Add(strarr[i, 0], strarr[i, 1]);
                        //сохраняем
                        currentConfig.Save(ConfigurationSaveMode.Full);
                        //принудительно перезагружаем соотвествующую секцию
                        ConfigurationManager.RefreshSection("appSettings");
                }
                else
                {
                    System.Configuration.Configuration currentConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    currentConfig.AppSettings.Settings[strarr[i,0]].Value = strarr[i,1];

                    currentConfig.Save(ConfigurationSaveMode.Modified);
                    ConfigurationManager.RefreshSection("appSettings");
                }
                } 
            
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
