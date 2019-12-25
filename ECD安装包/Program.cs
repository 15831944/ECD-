using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using AutoUpdater;

namespace ECD安装包
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            AutoUpdater.AutoUpdater au = new AutoUpdater.AutoUpdater();
            try
            {
                au.Update();
            }
            catch (WebException exp)
            {
                MessageBox.Show(String.Format("无法找到指定资源/n/n{0}", exp.Message), "自动升级", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (XmlException exp)
            {
                MessageBox.Show(String.Format("下载的升级文件有错误/n/n{0}", exp.Message), "自动升级", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (NotSupportedException exp)
            {
                MessageBox.Show(String.Format("升级地址配置错误/n/n{0}", exp.Message), "自动升级", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (ArgumentException exp)
            {
                MessageBox.Show(String.Format("下载的升级文件有错误/n/n{0}", exp.Message), "自动升级", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception exp)
            {
                MessageBox.Show(String.Format("升级过程中发生错误/n/n{0}", exp.Message), "自动升级", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Application.Run(new Form1());
        }
    }
}
