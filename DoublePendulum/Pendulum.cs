using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DoublePendulum
{
    class Pendulum
    {
        //first arm
        public int Length1 { get; set; }
        public float Mass1 { get; set; }

        public float Angle1 { get; set; }
        public float Velocity1 { get; set; }
        public float Acceleration1 { get; set; }


        //second arm
        public int Length2 { get; set; }
        public float Mass2 { get; set; }

        public float Angle2 { get; set; }
        public float Velocity2 { get; set; }
        public float Acceleration2 { get; set; }

        public Brush Brush1 { get; set; }
        public Brush Brush2 { get; set; }
        public Brush Brush3 { get; set; }
        public LinearGradientBrush GradientBrush1 { get; set; }
        public PathGradientBrush GradientBrush2 { get; set; }

        public int TrailLength { get; set; }

        private const float G = 9.8f; //Earth gravity
        private const float Pi = (float)(Math.PI * 2); //2 radians

        public List<PointF> Trail { get; private set; }


        public Pendulum(int length1, int length2, float mass1, float mass2, float angle)
        {
            this.Length1 = length1;
            this.Length2 = length2;
            this.Mass1 = mass1;
            this.Mass2 = mass2;

            this.Angle1 = angle*Pi/4;
            this.Velocity1 = 0f;
            this.Acceleration1 = 0f;

            this.Angle2 = Pi/8;
            this.Velocity2 = 0f;
            this.Acceleration2 = 0f;

            this.Brush1 = Brushes.LawnGreen;
            this.Brush2 = Brushes.ForestGreen;
            this.Brush3 = Brushes.Aqua;
            this.GradientBrush1 = new LinearGradientBrush(
                new Point(-320, -320),
                new Point(320, 320),
                Color.FromArgb(255, 255, 0, 0),
                Color.FromArgb(255, 0, 0, 255));

            this.TrailLength = 50;
            this.Trail = new List<PointF>(this.TrailLength + 1);

        }

        public void Update(float dt)
        {
            //first arm acceleration
            float t1 = -G * (2 * Mass1 + Mass2) * (float)Math.Sin(Angle1);
            float t2 = -Mass2 * G * (float)Math.Sin(Angle1 - 2 * Angle2);
            float t3 = -2 * (float)Math.Sin(Angle1 - Angle2) * Mass2;
            float t4 = Velocity2 * Velocity2 * Length2 + Velocity1 * Velocity1 * Length1 * (float)Math.Cos(Angle1 - Angle2);
            float den1 = Length1 * (2 * Mass1 + Mass2 - Mass2 * (float)Math.Cos(2 * Angle1 - 2 * Angle2));
            Acceleration1 = (t1 + t2 + t3 * t4) / den1;

            //second arm acceleration
            float t5 = 2 * (float)Math.Sin(Angle1 - Angle2);
            float t6 = Velocity1 * Velocity1 * Length1 * (Mass1 + Mass2);
            float t7 = G * (Mass1 + Mass2) * (float)Math.Cos(Angle1);
            float t8 = Velocity2 * Velocity2 * Length2 * Mass2 * (float)Math.Cos(Angle1 - Angle2);
            float den2 = Length2 * (2 * Mass1 + Mass2 - Mass2 * (float)Math.Cos(2 * Angle1 - 2 * Angle2));
            Acceleration2 = (t5 * (t6 + t7 + t8)) / den2;
        
            Velocity1 += Acceleration1 * dt;
            Velocity2 += Acceleration2 * dt;
            Angle1 += Velocity1 * dt;
            Angle2 += Velocity2 * dt;

            Angle1 = Angle1 % Pi; // range (-PI;PI) 2 radians
            Angle2 = Angle2 % Pi; // range (-PI;PI) 2 radians

            PointF tip1 = new PointF((float)Math.Sin(Angle1) * Length1, -(float)Math.Cos(Angle1) * Length1);
            PointF tip2 = new PointF((float)Math.Sin(Angle2) * Length2, -(float)Math.Cos(Angle2) * Length2);
            Console.WriteLine("X1:" + tip1.X + " Y1:" + tip1.Y + " X2:" + tip2.X + " Y2:" + tip2.Y + " A1:" + Angle1 + " A2:"+ Angle2);

            if (tip1.X > 10000f || tip2.Y > 10000f || tip2.X > 10000f || tip2.Y > 10000f)
            {
                Console.WriteLine("Poza zakresem!");
                Console.WriteLine("Length1: " + Length1);
                Console.WriteLine("Length2: " + Length1);
                Console.WriteLine((float)Math.Sin(Angle1));
                Console.WriteLine((float)Math.Sin(Angle2));
                Console.WriteLine(Angle1);
                Console.WriteLine(Angle2);
            }
            else
            {
                this.Add(new PointF(tip1.X + tip2.X, -tip1.Y - tip2.Y));
            }
            
        }

        private void Add(PointF point)
        {            
 
            Trail.Add(point);
            if (TrailLength > 0 && Trail.Count > TrailLength)
            {
                Trail.RemoveAt(0);
            }
        }
    }
}
