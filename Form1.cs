using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace graphics_lab_eight
{
    public partial class Form1 : Form
    {
        Timer bowDrawTimer;
        Timer targetTimer;
        int targetX = 30;
        float bowPower = 0.0f;
        Graphics g_bar;
        Graphics g_canvas;
        int mousePos;
        public Form1()
        {
            InitializeComponent();
            bowDrawTimer = new Timer();
            bowDrawTimer.Interval = 5;
            bowDrawTimer.Tick += new EventHandler(DrawBow);
            targetTimer = new Timer();
            targetTimer.Interval = 1;
            targetTimer.Tick += new EventHandler(targetTimer_Tick);
            targetTimer.Enabled = true;
            g_bar = pictureBoxDrawBar.CreateGraphics();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void DrawBow(object sender, EventArgs e)
        {
            if (bowPower <= 1)
            {
                Brush b = new SolidBrush(getFromHue(bowPower));
                float height = pictureBoxDrawBar.Height-pictureBoxDrawBar.Height*bowPower;
                g_bar.FillRectangle(b, 0, height, pictureBoxDrawBar.Width, pictureBoxDrawBar.Height/100+10);
                bowPower+=0.02f;
                
            }
        }
        private void shootBow()
        {
            label2.Text = (Convert.ToInt32(label2.Text)-1).ToString();
            int d = Math.Abs(targetX - mousePos + 30);
            if (d <= 30)
            {
                MessageBox.Show("Есть попадание!");
                label4.Text = (Convert.ToInt32(label4.Text)+((d <= 15)?10:5)).ToString();
            }
        }
        int direction = 1;
        private void targetTimer_Tick(object sender, EventArgs e)
        {
            int radius = 30;
            
            targetX += (5 * direction);
            if (targetX >= (pictureBox1.Width - radius- 200))
            {
                direction = -1;
            }

            else if (targetX == radius) direction = 1;
            pictureBox1.Refresh();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (label2.Text.Trim() == "0") MessageBox.Show("У вас закончились стрелы!");
            else bowDrawTimer.Enabled = true;
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            mousePos = e.X;
            if (bowPower >= 1) shootBow();
            bowPower = 0;
            bowDrawTimer.Enabled = false;
            g_bar.Clear(Color.White);
        }

        private Color getFromHue(float hue)
        {
            return Color.FromArgb(255, 250-(int)(250*hue), 0+(int)(200*hue), 20);
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Point bowStart = new Point(pictureBox1.Width-10, pictureBox1.Height-350);
            Point bowEnd = new Point(pictureBox1.Width - 10, pictureBox1.Height - 50);
            g_canvas = e.Graphics;
            g_canvas.DrawBezier(Pens.Black, bowStart.X, bowStart.Y, bowStart.X-100, (bowEnd.Y+bowStart.Y)*1/3, bowStart.X - 100, (bowEnd.Y + bowStart.Y) * 2 / 3, bowEnd.X, bowEnd.Y);
            g_canvas.DrawLine(Pens.Black, bowStart, bowEnd);

            g_canvas.FillEllipse(Brushes.Orange, new Rectangle(targetX, pictureBox1.Height / 2, 60, 60));
            g_canvas.FillEllipse(Brushes.Red, new Rectangle(targetX+15, pictureBox1.Height/2+15, 30, 30));
            g_canvas.DrawEllipse(Pens.Black, new Rectangle(targetX, pictureBox1.Height / 2, 60, 60));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label2.Text = "10";
            label4.Text = "0";
        }
    }
}
