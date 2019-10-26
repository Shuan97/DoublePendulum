using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows.Forms;

namespace DoublePendulum
{
    public partial class Form1 : Form
    {
        private readonly List<Pendulum> _pendulums;
        private Thread _thread;      
        private Bitmap _frame;
        private Graphics g;
        private GraphicsPath _path;
        private PathGradientBrush _br;
        private Rectangle _rect;
        private int _pendulumThickness = 0;
        private float _centerX;
        private float _centerY;

        private const int Interval = 5;
        private const float AnimationSpeed = 50f;
        private const float PI = (float) Math.PI;
        private const int EllipseDiameter = 10;       
        private const int CurveThickness = 3;

        public Form1()
        {
            InitializeComponent();
            _pendulums = new List<Pendulum>
            {
                new Pendulum(200, 110, 250, 10, 10f),
                new Pendulum(200, 120, 250, 11, 10f),
                new Pendulum(200, 130, 250, 12, 10f),
                new Pendulum(200, 140, 250, 13, 10f),
                new Pendulum(200, 150, 250, 14, 10f),
                new Pendulum(200, 160, 250, 15, 10f),
                new Pendulum(200, 170, 250, 16, 10f),
                new Pendulum(200, 180, 250, 17, 10f),
                new Pendulum(200, 190, 250, 17, 10f),
                new Pendulum(200, 200, 250, 19, 10f),
            };
            
            Setup();
            Start();
        }

        private void Draw()
        {
            _centerX = display.Width / 2f;
            _centerY = display.Height / 2f;
            _frame = new Bitmap(display.Width, display.Height);
            g = Graphics.FromImage(_frame);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TranslateTransform(_centerX, _centerY);

            foreach (Pendulum pendulum in _pendulums)
            {
                if (pendulum.Trail.Count > 2)
                {

                    //first curve design (standard)
                    //if (Math.Abs(pendulum.Trail[0].X - pendulum.Trail[pendulum.Trail.Count - 1].X) > 0.05 ||
                    //    Math.Abs(pendulum.Trail[0].Y - pendulum.Trail[pendulum.Trail.Count - 1].Y) > 0.05)
                    //{
                    //    try
                    //    {
                    //        g.DrawCurve(new Pen(Color.MediumSeaGreen, CurveThickness), pendulum.Trail.ToArray());
                    //    }
                    //    catch (Exception)
                    //    {
                    //        Console.WriteLine(@"[{0}] First and last point were too close (distance < 0.1f) !", DateTime.Now);
                    //        //Stop();
                    //    }
                    //}

                    //second curve design(fading)
                    for (int i = 1; i < pendulum.Trail.Count; i++)
                    {
                        List<PointF> list = new List<PointF> { pendulum.Trail[i - 1], pendulum.Trail[i] };
                        try
                        {
                            if (Math.Abs(list[0].X - list[1].X) > 0.1 || Math.Abs(list[0].Y - list[1].Y) > 0.1)
                                g.DrawCurve(new Pen(Color.FromArgb(i * 255 / pendulum.Trail.Count, 0, i * 200 / (pendulum.Trail.Count - 1), 150), CurveThickness), list.ToArray());
                        }
                        catch (Exception)
                        {
                            Console.WriteLine(@"[{0}] !!! Something goes wrong !!!", DateTime.Now);
                            Console.WriteLine(pendulum.Trail[i-1] + @" || " + list[0]);
                            Console.WriteLine(pendulum.Trail[i] + @" || " + list[1]);
                            Console.WriteLine(pendulum.Trail.Count);
                            Console.WriteLine(list.Count);
                            Console.WriteLine(Math.Abs(list[0].X - list[1].X));
                            Console.WriteLine(Math.Abs(list[0].Y - list[1].Y));
                            Console.WriteLine(pendulum.Trail.Count);
                            Console.WriteLine(list.Count);
                        }
                    }
                }                   
            }
            g.ResetTransform();            

            foreach (Pendulum pendulum in _pendulums)
            {
                try
                {                   
                    g.TranslateTransform(_centerX, _centerY);                    
                    g.RotateTransform(-pendulum.Angle1 * 180 / PI);
                    g.FillRectangle(pendulum.Brush1, new Rectangle(-_pendulumThickness / 2, 0 / 2, _pendulumThickness, pendulum.Length1));
                    g.TranslateTransform(0, pendulum.Length1);
                    //g.FillEllipse(pendulum.Brush1, new Rectangle(-EllipseDiameter / 2, -EllipseDiameter / 2, EllipseDiameter, EllipseDiameter));
                    g.RotateTransform((pendulum.Angle1 - pendulum.Angle2) * 180 / PI);                    
                    g.FillRectangle(pendulum.Brush2, new Rectangle(-_pendulumThickness / 2, 0 / 2, _pendulumThickness, pendulum.Length2));
                    g.TranslateTransform(0, pendulum.Length2);                    
                    g.FillEllipse(_br, _rect);
                    g.ResetTransform();
                }
                catch (Exception)
                {
                    Console.WriteLine(@"[{0}] Some graphics translations goes wrong!", DateTime.Now);
                    Console.WriteLine(pendulum.Angle1 + @"  ||  " + pendulum.Angle2);
                }               
            }
            g.Flush();

            try
            {
                display.Invoke(new Action(() => display.Image = _frame));                
            }
            catch (Exception)
            {               
                Stop();
            }
        }

        private void Setup()
        {
            //variables, objects setup          
            _rect = new Rectangle(-EllipseDiameter / 2, -EllipseDiameter / 2,EllipseDiameter,EllipseDiameter);
            _path = new GraphicsPath();
            _path.AddEllipse(_rect);
            _br = new PathGradientBrush(_path)
            {
                CenterColor = Color.LawnGreen,
                SurroundColors = new Color[] { Color.Transparent, }
            };
            
            //running pendulum thread
            _thread = new Thread(() =>
            {
                while (true)
                {
                    UpdateSimulation(AnimationSpeed / 1000f);
                    Draw();                   
                    Thread.Sleep(Interval);
                }
            });
        }

        private void UpdateSimulation(float dt)
        {
            foreach (Pendulum pendulum in _pendulums)
            {
                pendulum.Update(dt);
            }
        }

        private void Start()
        {
            _thread.Start();
        }

        private void Stop()
        {
            _thread.Abort();
        }

        private void btnArms_Click(object sender, EventArgs e)
        {
            this._pendulumThickness = this._pendulumThickness == 0
                ? this._pendulumThickness = 1
                : this._pendulumThickness = 0;
        }
    }
}
