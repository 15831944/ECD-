using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ECD安装包
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int height = SystemInformation.WorkingArea.Height;
            int width = SystemInformation.WorkingArea.Width;
            int formheight = this.Size.Height;
            int formwidth = this.Size.Width;
            int newformx = width / 2 - formwidth / 2;
            int newformy = height / 2 - formheight / 2;
            this.SetDesktopLocation(newformx, newformy);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                Cad2012();
                this.Hide();

            }

            if (radioButton2.Checked)

            {
                Cad2018();
                this.Hide();
            }
            }
        void Cad2012()
        {
            DirectoryInfo path_exe = new DirectoryInfo(Application.StartupPath); //exe目录
            String path = path_exe.Parent.Parent.FullName; //上两级的目录
            string p1 = path + "\\dll";
            string aimpth = @"C:\Program Files\Autodesk\AutoCAD 2012 - Simplified Chinese";

            //CopyDirectory(p1, aimpth);

            TestCopyDirectory(p1, aimpth);//复制dll到CAD目录
            string lsppath = @"C:\Program Files\Autodesk\AutoCAD 2012 - Simplified Chinese\Support";
            string getlsppath = path + "\\lsp2012";

            //ecdlogin存放在exe启动目录
            string ecdlogin = path + "\\bin\\debug\\Ecdlogin2012.dll";
            CopyToFile(ecdlogin, aimpth);//复制ECDLOGIN2012.dll到CAD目录中


            TestCopyDirectory(getlsppath, lsppath);//复制lisp文件到CAD中support文件夹中
            MessageBox.Show("插件加载成功请启动CAD！");
            
        }

        void Cad2018()
        {

            DirectoryInfo path_exe = new DirectoryInfo(Application.StartupPath); //exe目录
            String path = path_exe.Parent.Parent.FullName; //上两级的目录
            string p1 = path + "\\dll";
            // string[] dir = Directory.GetFiles(p1);
            //  string filepath

            string aimpth = @"C:\Program Files\Autodesk\AutoCAD 2018";

            //CopyDirectory(p1, aimpth);

            TestCopyDirectory(p1, aimpth);
            string lsppath = @"C:\Program Files\Autodesk\AutoCAD 2018\Support";
            string getlsppath = path + "\\lsp2018";
            TestCopyDirectory(getlsppath, lsppath);
            string ecdlogin = path + "\\bin\\debug\\Ecdlogin2012.dll";
            CopyToFile(ecdlogin, aimpth);//复制ECDLOGIN2012.dll到CAD目录中
            MessageBox.Show("插件加载成功请启动CAD！");

        }
        private static void AutoLoad(string dname, string desc, string dpath)
        {
            RegistryKey LocaIMachine = Registry.LocalMachine;
            RegistryKey MyPrograrm = LocaIMachine.OpenSubKey(@"SOFTWARE\Autodesk\AutoCAD\R18.2\ACAD-A001:804\Applications\", true);
            //RegistryKey MyPrograrm = Applications.CreateSubKey(dname);
            MyPrograrm.SetValue("DESCRIPTION", desc, RegistryValueKind.String);
            MyPrograrm.SetValue("LOADCTRLS", 14, RegistryValueKind.DWord);
            MyPrograrm.SetValue("LOADER", dpath, RegistryValueKind.Binary);
            MyPrograrm.SetValue("MANAGED", 1, RegistryValueKind.DWord);

        }

        private static void CopyDirectory(string srcdir, string desdir)//复制文件夹到文件夹
        {
            string folderName = srcdir.Substring(srcdir.LastIndexOf("\\") + 1);

            string desfolderdir = desdir + "\\" + folderName;

            if (desdir.LastIndexOf("\\") == ( desdir.Length - 1 ))
            {
                desfolderdir = desdir + folderName;
            }
            string[] filenames = Directory.GetFileSystemEntries(srcdir);

            foreach (string file in filenames)// 遍历所有的文件和目录
            {
                if (Directory.Exists(file))// 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件
                {

                    string currentdir = desfolderdir + "\\" + file.Substring(file.LastIndexOf("\\") + 1);
                    if (!Directory.Exists(currentdir))
                    {
                        Directory.CreateDirectory(currentdir);
                    }

                    CopyDirectory(file, desfolderdir);
                }

                else // 否则直接copy文件
                {
                    string srcfileName = file.Substring(file.LastIndexOf("\\") + 1);

                    srcfileName = desfolderdir + "\\" + srcfileName;


                    if (!Directory.Exists(desfolderdir))
                    {
                        Directory.CreateDirectory(desfolderdir);
                    }


                    File.Copy(file, srcfileName,true);
                }
            }//foreach 
        }
        public static void CopyToFile(string srcidr ,string desdir)//复制单个文件到文件夹
        {
            //源文件路径
            string sourceName = srcidr;

            //目标路径:项目下的NewTest文件夹,(如果没有就创建该文件夹)
           // string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NewTest");
            if (!Directory.Exists(desdir))
            {
                Directory.CreateDirectory(desdir);
            }

            //文件不用新的文件名，就用原文件文件名
            string fileName = Path.GetFileName(sourceName);
            ////可以选择给文件换个新名字
            //string fileName = string.Format("{0}.{1}", "newFileText", "txt");

            //目标整体路径
            string targetPath = Path.Combine(desdir, fileName);

            //Copy到新文件下
            FileInfo file = new FileInfo(sourceName);
            if (file.Exists)
            {
                //true 覆盖已存在的同名文件，false不覆盖
                file.CopyTo(targetPath, true);
            }
        }
        private static void TestCopyDirectory(string srcdir, string desdir)//复制文件到文件夹
        {

            string[] filenames = Directory.GetFileSystemEntries(srcdir);

            foreach (string file in filenames)// 遍历所有的文件和目录
            {
                if (Directory.Exists(file))// 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件
                {


                    string currentdir = desdir + "\\" + file.Substring(file.LastIndexOf("\\") + 1);
                    if (!Directory.Exists(currentdir))
                    {
                        Directory.CreateDirectory(currentdir);
                    }

                    CopyDirectory(file, desdir);
                }

                else // 否则直接copy文件
                {
                    string srcfileName = file.Substring(file.LastIndexOf("\\") + 1);

                    srcfileName = desdir + "\\" + srcfileName;



                   
                    File.Copy(file, srcfileName,true);
                }
            }//foreach 
        }


    }
}
