using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Physics
{
    public class Vector
    {
        public double X { get; set; }
        public double Y { get; set; }

        public static Vector operator -(Vector v1, Vector v2)
        {
            return new Vector()
            {
                X = v1.X - v2.X,
                Y = v1.Y - v2.Y
            };
        }

        public Vector Normalize()
        {
            var length = Length();

            return new Vector()
            {
                X = X / length,
                Y = Y / length
            };
        }

        public double Length()
        {
            return Math.Sqrt((X * X) + (Y * Y));
        }
    }

    public enum DrawOrder
    {
        Background,
        PickUp,
        Door,
        Player,
        Enemy,
        Boss,
        Scenery
    }

    public abstract class Body
    {
        public object Tag { get; set; }
        public string ID { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double VelX { get; set; }
        public double VelY { get; set; }
        public double AccX { get; set; }
        public double AccY { get; set; }
        public double Mass { get; set; }
        public double Drag { get; set; }
        public double ImpX { get; set; }
        public double ImpY { get; set; }
        public double Restitution { get; set; }
        public bool IsStatic { get; set; }
        public List<Body> CollisionList { get; set; }
        //public double Friction { get; set; }
        //public BodyType Type { get; set; }
        public bool Dead { get; set; }
        public int CollisionGroup { get; set; }
        public bool CollisionOnlyUpdate { get; set; }
        public bool DontList { get; set; }
        public DrawOrder DrawOrder { get; set; }

        public Vector Center
        {
            get
            {
                return new Vector()
                {
                    X = X + (Width / 2),
                    Y = Y + (Height / 2)
                };
            }
        }

        public double Bottom
        {
            get
            {
                return Y + Height;
            }
        }

        public double Top
        {
            get
            {
                return Y;
            }
        }

        public double Left
        {
            get
            {
                return X;
            }
        }

        public double Right
        {
            get
            {
                return X + Width;
            }
        }

        public Body()
        {
            CollisionList = new List<Body>();
        }

        public void ApplyImpulse(double x, double y)
        {
            ImpX += x;
            ImpY += y;
        }

        public override string ToString()
        {
            return string.Format("{0}: {1},{2} - {3},{4}", ID, X, Y, VelX, VelY);
        }

        public bool Colide(Body body)
        {
            return (BottomColide(body) || TopColide(body) || TopBottomColide(body)) && (RightColide(body) || LeftColide(body) || LeftRightColide(body));
        }

        public bool LeftColide(Body body)
        {
            return Left < body.Right && Left > body.Left;
        }

        public bool RightColide(Body body)
        {
            return Right > body.Left && Right <= body.Right;
        }

        public bool TopColide(Body body)
        {
            return Top < body.Bottom && Top > body.Top;
        }

        public bool BottomColide(Body body)
        {
            return Bottom >= body.Top && Bottom <= body.Bottom;
        }

        public bool TopBottomColide(Body body)
        {
            return Bottom > body.Top && Top < body.Bottom;
        }

        public bool LeftRightColide(Body body)
        {
            return Right > body.Left && Left < body.Right;
        }
    }
}
