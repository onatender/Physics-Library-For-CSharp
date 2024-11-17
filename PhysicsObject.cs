using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace phy.Entities
{
    public class PhysicsObject
    {
        public static Timer Time { get; set; }
        public const float GRAVITY_ACCELERATION = 10f;
        public static float Weight { get; set; }
        public Panel FormObject { get; set; }
        public static bool noGravity { get; set; }
        public static Velocity Velocity { get; set; }
        public static List<Force> Forces { get; set; }
        public static Acceleration Acceleration { get; set; }
        public enum Direction { Up, Down, Left, Right }
        public float userPixelHeight { get; set; }
        public PhysicsObject(int weight, Panel formObject, bool noGravity,int fps)
        {
            userPixelHeight = pixelHeight();
            Weight = weight;
            FormObject = formObject;
            Velocity = new Velocity(0, 0);
            Acceleration = new Acceleration(0, 0);
            Forces = new List<Force>();
            noGravity = noGravity;
            if (!noGravity)
                Forces.Add(new Force(Weight * GRAVITY_ACCELERATION, Direction.Down, "GRAVITY", int.MaxValue));
            Time = new Timer();
            Time.Interval = 1000/fps;
            Time.Tick += Time_Tick;
            Time.Start();
        }

        public void setGravity(bool boolean)
        {
            noGravity = boolean;
        }   

        public void Stop()
        {
            Velocity = new Velocity(0, 0);
        }



        public void AddForce(Force force)
        {
            Forces.Add(force);
        }



        public int cmToPixel(float cm)
        {
            return (int)(cm/userPixelHeight);
        }

        public float pixelHeight()
        {
            using (Graphics g = Graphics.FromHwnd(IntPtr.Zero))
            {
                // Ekranın DPI değerlerini al
                float dpiX = g.DpiX; // Yatay DPI
                float dpiY = g.DpiY; // Dikey DPI

                // Piksel başına yüksekliği hesapla (inch cinsinden)
                float pixelHeightInInches = 1 / dpiY;

                // Piksel başına yüksekliği santimetreye çevir
                float pixelHeightInCm = pixelHeightInInches * 2.54f;
                return pixelHeightInCm;
            }
        }

        public void Time_Tick(object sender, EventArgs e)
        {
            float timePassed = (float)Time.Interval/1000;
            CalculateAccelerations(timePassed);
            FormObject.Location = CalculateNewLocation(timePassed);
            CalculateVelocities(timePassed);
        }
        public void CalculateVelocities(float timePassed)
        {
            Velocity.X += Acceleration.X*timePassed;
            Velocity.Y += Acceleration.Y*timePassed;

        }
        private Point CalculateNewLocation(float timePassed) //
        {
            float xChange = Velocity.X*timePassed + 0.5f*Acceleration.X*timePassed*timePassed;
            float yChange = Velocity.Y*timePassed + 0.5f*Acceleration.Y*timePassed*timePassed;
            return new Point(FormObject.Location.X + cmToPixel(xChange), FormObject.Location.Y + cmToPixel(yChange));
        }


        private void CalculateVelocities()
        {
            float timePassed = Time.Interval/1000;
            Velocity.X += Acceleration.X * timePassed;
            Velocity.Y += Acceleration.Y * timePassed;
        }

        public static void CalculateAccelerations(float timePassed) //doğru
        {
            Acceleration = new Acceleration(0, 0);
            foreach (Force force in Forces)
            {
                if (force.ForceName == "GRAVITY" && noGravity)
                {
                    continue;
                }
                if (force.Direction == Direction.Down)
                {
                    Acceleration.Y += (force.Value / Weight);
                }
                else if (force.Direction == Direction.Up)
                {
                    Acceleration.Y -= (force.Value / Weight);
                }
                else if (force.Direction == Direction.Left)
                {
                    Acceleration.X -= (force.Value / Weight);
                }
                else if (force.Direction == Direction.Right)
                {
                    Acceleration.X += (force.Value / Weight);
                }
                force.TimeRemained -= timePassed;
            }

            for (int i = 0; i < Forces.Count; i++)
            {
                if (Forces[i].TimeRemained <= 0)
                {
                    Forces.RemoveAt(i);
                    i-=1;
                }
            }

        }



        internal object PixelToCm(int v)
        {
            return v*userPixelHeight;
        }
    }
}
