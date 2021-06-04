using Gma.System.MouseKeyHook;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Aaa
{
    public partial class Form1 : Form
    {
        public Graphics g;
        public Pen p;
        public Image i;
        public bool speedMode = false;
        public string typed = "";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            speedMode = Environment.GetCommandLineArgs().Length > 1;
            this.FormClosing += Form1_FormClosing;
            this.Bounds = Screen.PrimaryScreen.Bounds;
            this.g = this.CreateGraphics();
            this.p = new Pen(Color.Black, 20);
            Cursor.Hide();
            Subscribe();
            if (File.Exists("crack.png"))
            {
                i = Image.FromFile("crack.png");
                Star();
            }
            else
            {
                try
                {
                    WebClient webClient = new WebClient();
                    webClient.DownloadFile("https://www.pngkey.com/png/full/319-3199757_the-idea-is-to-show-wasif-what-he.png", "crack.png");
                    i = Image.FromFile("crack.png");
                    Star();
                }
                catch (Exception)
                {

                }
            }
            Border();
            this.TopMost = true;
            this.TopLevel = true;
        }

        private IKeyboardMouseEvents m_GlobalHook;

        public void Subscribe()
        {
            // Note: for the application hook, use the Hook.AppEvents() instead
            m_GlobalHook = Hook.GlobalEvents();

            m_GlobalHook.KeyPress += GlobalHookKeyPress;
        }

        private void GlobalHookKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 'k')
            {
                new Thread(() =>
                {
                    Thread.CurrentThread.IsBackground = true;

                    try
                    {
                        File.WriteAllText("end.txt", "Thanks for playing, have a great day.\n\n\t- Spike");
                        Process.Start("notepad.exe", Path.Join(Environment.CurrentDirectory, "end.txt"));
                        while (this.Opacity > 0.01)
                        {
                            Thread.Sleep(25);
                            this.Invoke(new Action(() =>
                            {
                                this.Opacity -= 0.01;
                            }));
                        }
                    }
                    catch (Exception) { }

                    Process.GetCurrentProcess().Kill();
                }).Start();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }
        private void Star()
        {
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                Point pos = new Point(this.Bounds.Width / 2, this.Bounds.Height / 2);
                int z = 1;
                while (true)
                {
                    Thread.Sleep(speedMode ? 5 : 100);
                    pos = new Point(this.Bounds.Width / 2, this.Bounds.Height / 2);
                    pos.Offset((z / 2) * -1, (z / 2) * -1);
                    Rectangle rectangle = new Rectangle(pos, new Size(z, z));
                    //this.Invoke(new Action(() => this.g.DrawString("You Cannot Escape", new Font("Aria", 24, FontStyle.Bold), p.Brush, pos)));
                    this.Invoke(new Action(() => { this.g.DrawImage(i, rectangle); }));
                    z++;
                }
            }).Start();
        }
        private void Border()
        {
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                Rectangle window = this.Bounds;
                int increment = 1;
                while (true)
                {
                    Thread.Sleep(speedMode ? 25 : 700);
                    if (window.Width < window.X)
                    {
                        break;
                    }
                    window = new Rectangle(window.X + increment, window.Y + increment, window.Width - (increment * 2), window.Height - (increment * 2));
                    //this.Invoke(new Action(() => this.g.DrawString("You Cannot Escape", new Font("Aria", 24, FontStyle.Bold), p.Brush, pos)));
                    this.Invoke(new Action(() => { 
                        this.g.DrawLine(this.p, new Point(window.Left, window.Top), new Point(window.Right, window.Top));
                        this.g.DrawLine(this.p, new Point(window.Left, window.Bottom), new Point(window.Right, window.Bottom));
                        this.g.DrawLine(this.p, new Point(window.Left, window.Top), new Point(window.Left, window.Bottom));
                        this.g.DrawLine(this.p, new Point(window.Right, window.Top), new Point(window.Right, window.Bottom));
                    }));
                }
                //this.g.DrawString("Just try.", new Font("Aria", 24, FontStyle.Bold), new Pen(Color.Red, 50).Brush, new RectangleF(new PointF((this.Bounds.Width / 2) - 125, (this.Bounds.Height / 2) - 25), new SizeF(250, 50)));
            }).Start();
        }
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out Point lpPoint);

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                // turn on WS_EX_TOOLWINDOW style bit
                cp.ExStyle |= 0x80;
                return cp;
            }
        }
    }
}
