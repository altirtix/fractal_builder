using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PPLV
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            disableControls(this.Controls);
            osToolStripMenuItem.Text = SystemInfo.getOS();
            lANIPToolStripMenuItem.Text = SystemInfo.getLANIP();
            wANIPToolStripMenuItem.Text = SystemInfo.getWANIP();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                if (ofd.ShowDialog() == DialogResult.Cancel)
                {
                    return;
                }

                ofd.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";
                pictureBox1.Image = new Bitmap(ofd.FileName);
                textBox2.Text = ofd.FileName;

                MessageBox.Show(this, "File is opened!",
                    "Message",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Something went wrong!" + "\r\n" + ex.ToString(),
                    "Message",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();

                sfd.FileName = "Fractal";
                sfd.Filter = "JPEG Image (.jpeg)|*.jpeg";
                sfd.Title = "Save Fractal image";

                if (sfd.ShowDialog() == DialogResult.Cancel)
                {
                    return;
                }

                int width = Convert.ToInt32(pictureBox1.Width);
                int height = Convert.ToInt32(pictureBox1.Height);
                Bitmap bmp = new Bitmap(width, height);
                pictureBox1.DrawToBitmap(bmp, new Rectangle(0, 0, width, height));
                bmp.Save(sfd.FileName, ImageFormat.Jpeg);

                MessageBox.Show(this, "File is saved!",
                    "Message",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Something went wrong!" + "\r\n" + ex.ToString(),
                    "Message",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        public void clearControls(Control.ControlCollection ctrlCollection)
        {
            foreach (Control ctrl in ctrlCollection)
            {
                if (ctrl is TextBoxBase)
                {
                    ctrl.Text = String.Empty;
                }
                else
                {
                    clearControls(ctrl.Controls);
                }
            }
        }

        public void enableControls(Control.ControlCollection ctrlCollection)
        {
            foreach (Control ctrl in ctrlCollection)
            {
                if (ctrl is TextBoxBase)
                {
                    ctrl.Enabled = true;
                }
                else if (ctrl is ButtonBase)
                {
                    ctrl.Enabled = true;
                }
                else
                {
                    enableControls(ctrl.Controls);
                }
            }
        }

        public void disableControls(Control.ControlCollection ctrlCollection)
        {
            foreach (Control ctrl in ctrlCollection)
            {
                if (ctrl is TextBoxBase)
                {
                    ctrl.Enabled = false;
                }
                else if (ctrl is ButtonBase)
                {
                    ctrl.Enabled = false;
                }
                else
                {
                    disableControls(ctrl.Controls);
                }
            }
        }

        private void unlockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            enableControls(this.Controls);
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clearControls(this.Controls);
            pictureBox1.Image = null;
            cnt = 0;
        }

        private void restartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this, "Fractal Generator\r\n"
                        + "Artrur Zhadan\r\n"
                        + "2020\r\n",
                        "Message",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            dateToolStripMenuItem.Text = SystemInfo.getDate();
            timeToolStripMenuItem.Text = SystemInfo.getTime();
            stopwatchToolStripMenuItem.Text = SystemInfo.getStopwatch();
        }

        private List<PointF> Initiator;

        private float ScaleFactor;
        private List<float> GeneratorDTheta;

        private Random rnd = new Random();

        private void DrawSnowflakeEdge(Graphics gr, int depth, ref PointF p1, float theta, float dist)
        {
            if (depth == 0)
            {
                PointF p2 = new PointF(
                    (float)(p1.X + dist * Math.Cos(theta)),
                    (float)(p1.Y + dist * Math.Sin(theta)));

                gr.DrawLine(Pens.Black, p1, p2);
                p1 = p2;
                return;
            }

            dist *= ScaleFactor;
            for (int i = 0; i < GeneratorDTheta.Count; i++)
            {
                theta += GeneratorDTheta[i];
                DrawSnowflakeEdge(gr, depth - 1, ref p1, theta, dist);
            }
        }

        private void DrawSnowflake(Graphics gr, int depth)
        {
            gr.Clear(pictureBox1.BackColor);

            for (int i = 1; i < Initiator.Count; i++)
            {
                PointF p1 = Initiator[i - 1];
                PointF p2 = Initiator[i];

                float dx = p2.X - p1.X;
                float dy = p2.Y - p1.Y;
                float length = (float)Math.Sqrt(dx * dx + dy * dy);
                float theta = (float)Math.Atan2(dy, dx);
                DrawSnowflakeEdge(gr, depth, ref p1, theta, length);
            }
        }

        public static int cnt = 1;

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                textBox2.Text = Convert.ToString(cnt++);

                Initiator = new List<PointF>();

                float height = 0.75f * (Math.Min(
                    pictureBox1.ClientSize.Width,
                    pictureBox1.ClientSize.Height) - 20);
                float width = (float)(height / Math.Sqrt(3.0) * 2);
                
                float y3 = pictureBox1.ClientSize.Height - 10;
                float y1 = y3 - height;
                float x3 = pictureBox1.ClientSize.Height / 2;
                float x1 = x3 - width / 2;
                float x2 = x1 + width;

                Initiator.Add(new PointF(x1, y1));
                Initiator.Add(new PointF(x2, y1));
                Initiator.Add(new PointF(x3, y3));
                Initiator.Add(new PointF(x1, y1));

                ScaleFactor = 1 / 3f; 

                GeneratorDTheta = new List<float>();

                float pi_over_3 = (float)(Math.PI / 3f);

                GeneratorDTheta.Add(0);                 
                GeneratorDTheta.Add(-pi_over_3); 
                GeneratorDTheta.Add(2 * pi_over_3); 
                GeneratorDTheta.Add(-pi_over_3);

                int depth = int.Parse(textBox1.Text);

                Bitmap bm = new Bitmap(pictureBox1.ClientSize.Width, pictureBox1.ClientSize.Height);
                pictureBox1.Image = bm;

                using (Graphics gr = Graphics.FromImage(bm))
                {
                    gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    DrawSnowflake(gr, depth);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Something went wrong!" + "\r\n" + ex.ToString(),
                    "Message",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }
    }
}
