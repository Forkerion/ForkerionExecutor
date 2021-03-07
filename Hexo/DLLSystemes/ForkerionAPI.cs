using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;

namespace ForkerionAPI
{
    public class Client
    {
        public static string pipe;

        public static string name;

        public static string downloadLink;

        private Pipe.BasicInject Injector = new Pipe.BasicInject();

        static Client()
        {
            Client.pipe = "qa6uPWYHm1LeUFsLosFF";
            Client.name = "./bin/forkerion.dll";
        }
        Forkerion.ErrorMessage Err = new Forkerion.ErrorMessage();
        public Client()
        {
        }

        public void Attach()
        {
            if ((int)Process.GetProcessesByName("RobloxPlayerBeta").Length != 1)
            {
                Forkerion.Properties.Settings.Default.ErrorMessage = "Roblox is not open!";
                Forkerion.Properties.Settings.Default.Save();
                
                Err.Show();
            }
            else if (this.isOXygenUAttached())
            {
                Forkerion.Properties.Settings.Default.ErrorMessage = "Already attached!";
                Forkerion.Properties.Settings.Default.Save();

                Err.Show();
            }
            else
            {
                if (!File.Exists(Client.name))
                {
                    Forkerion.Properties.Settings.Default.ErrorMessage = "Dll not found!";
                    Forkerion.Properties.Settings.Default.Save();

                    Err.Show();
                }
                this.Injector.InjectDLL();
                try
                {
                    MessageBox.Show("Injected. Please consider joining our Discord!","Forkerion");
                }
                catch (Exception exception)
                {
                    return;
                }
            }
        }

        public void Execute(string script)
        {
            Pipe.MainPipeClient(Client.pipe, script);
        }

        public void IntializeUpdate()
        {
            if (!File.Exists(Client.name))
            {
                try
                {
                    (new WebClient()).DownloadFile(Client.downloadLink, Client.name);
                }
                catch (Exception exception1)
                {
                    Exception exception = exception1;
                }
            }
            else if (this.isOXygenUAttached())
            {
                this.KillRoblox();
                if (File.Exists(Client.name))
                {
                    try
                    {
                        File.Delete(Client.name);
                        (new WebClient()).DownloadFile(Client.downloadLink, Client.name);
                    }
                    catch (Exception exception3)
                    {
                        Exception exception2 = exception3;
                    }
                }
            }
            else
            {
                try
                {
                    File.Delete(Client.name);
                    (new WebClient()).DownloadFile(Client.downloadLink, Client.name);
                }
                catch (Exception exception5)
                {
                    Exception exception4 = exception5;
                }
            }
        }

        public bool isOXygenUAttached()
        {
            bool flag;
            flag = Pipe.NamedPipeExist(pipe) ? true : false;
            return flag;
        }

        public bool isRobloxOn()
        {
            bool flag;
            flag = ((int)Process.GetProcessesByName("RobloxPlayerBeta").Length != 1 ? false : true);
            return flag;
        }

        public void KillRoblox()
        {
            Process[] processesByName = Process.GetProcessesByName("RobloxPlayerBeta");
            for (int i = 0; i < (int)processesByName.Length; i++)
            {
                processesByName[i].Kill();
            }
        }
    }
}
