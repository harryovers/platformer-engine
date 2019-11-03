using Platform.Physics.Forces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Platform.Physics
{
    public class World
    {
        public List<Body> _bodies = new List<Body>();
        private DateTime _lastUpdate = new DateTime();
        private List<Force> Forces = new List<Force>();

        //public double GravityX { get; set; }
        //public double GravityY { get; set; }

        public World(double gravityX, double gravityY)
        {
            //GravityX = gravityX;
            //GravityY = gravityY;

            Forces = new List<Force>();
            Forces.Add(new Impulse());
            Forces.Add(new Gravity(gravityX, gravityY));
            Forces.Add(new Drag());
        }

        public void AddBody(Body body)
        {
            _bodies.Add(body);
        }

        public void RemoveBody(Body body)
        {
            _bodies.Remove(body);
        }

        //public void Start()
        //{
        //    ThreadStart ts = new ThreadStart(Run);
        //    Thread t = new Thread(ts);
        //    t.Start();
        //}

        //private void Run()
        //{
        //    //AutoResetEvent autoResetEvent = new AutoResetEvent(true);
        //    //TimerCallback timerCallback = new TimerCallback(Step);
        //    //Timer timer = new Timer(timerCallback, autoResetEvent, 10, 1000 / 60);
        //    _lastUpdate = DateTime.Now;
        //    while (true)
        //    {
        //        TimeSpan delta = DateTime.Now - _lastUpdate;
        //        _lastUpdate = DateTime.Now;

        //        Step(delta);
        //        System.Threading.Thread.Sleep(1000 / 60);
        //    }
        //}

        public void Step(TimeSpan delta)
        {
            foreach (var body in _bodies)
            {
                body.CollisionList.Clear();
            }

            foreach (var body in _bodies)
            {
                if ((!body.IsStatic || body.CollisionOnlyUpdate) && !body.Dead)
                {
                    Update(body, delta);

                    //Console.WriteLine(body.ToString());
                    //Console.CursorLeft = (int)body.X * 6;
                    //Console.Write(".");
                    //Console.Write("\r\n");
                }
            }

            for (int i = _bodies.Count - 1, n = -1; i > n; --i)
            {
                if (_bodies[i].Dead)
                {
                    _bodies.Remove(_bodies[i]);
                }
            }
        }

        private void Update(Body body, TimeSpan delta)
        {
            var msDelta = 1000 / delta.TotalMilliseconds;

            if (msDelta != 0)
            {
                if (!body.CollisionOnlyUpdate)
                {
                    // impulse
                    //body.VelX += body.ImpX / body.Mass;
                    //body.ImpX = 0;
                    //body.VelY += body.ImpY / body.Mass;
                    //body.ImpY = 0;

                    //// calc gravity effect
                    //body.VelX += GravityX / body.Mass;
                    //body.VelY += GravityY / body.Mass;

                    //// drag
                    //body.VelX -= CalcDragCoef(body.VelX, body.Drag);
                    //body.VelY -= CalcDragCoef(body.VelY, body.Drag);

                    foreach (var force in Forces)
                    {
                        force.ApplyForce(body);
                    }

                    // move body by velocity
                    body.X += body.VelX / msDelta;
                    body.Y += body.VelY / msDelta;
                }

                foreach (var otherBody in _bodies.Where(b => b.ID != body.ID && !b.Dead))
                {
                    if (body.Colide(otherBody))
                    {
                        if (!body.DontList && !otherBody.DontList)
                        {
                            if ((otherBody.ID.StartsWith("br") || body.ID.StartsWith("k")))
                            {
                                Debug.WriteLine("break?");
                            }

                            if (!body.CollisionList.Contains(otherBody))
                            {
                                body.CollisionList.Add(otherBody);
                            }
                            if (!otherBody.CollisionList.Contains(body))
                            {
                                otherBody.CollisionList.Add(body);
                            }

                        }

                        if (!ShouldIgnoreCollisionResponse(body, otherBody))
                        {
                            var restitution = (body.Restitution + otherBody.Restitution) / 2;
                            var angleOfColision = body.Center - otherBody.Center;

                            if (angleOfColision.X < 0)
                            {
                                angleOfColision.X *= -1;
                            }
                            if (angleOfColision.Y < 0)
                            {
                                angleOfColision.Y *= -1;
                            }

                            if (angleOfColision.Y >= angleOfColision.X)
                            {
                                var collisionVelocity = Math.Abs(Math.Max(body.VelY, otherBody.VelY) - Math.Min(body.VelY, otherBody.VelY)) * (body.Mass + otherBody.Mass);

                                if (otherBody.IsStatic)
                                {
                                    body.VelY *= -1 * (restitution);
                                }
                                else
                                {
                                    body.ApplyImpulse(0, (collisionVelocity) * restitution * ImpulseReactionDirection(body.Y, otherBody.Y));
                                    otherBody.ApplyImpulse(0, (collisionVelocity) * restitution * ImpulseReactionDirection(otherBody.Y, body.Y));
                                }

                                if (body.Y > otherBody.Y)
                                {
                                    body.Y = otherBody.Y + otherBody.Height;
                                }
                                else
                                {
                                    body.Y = otherBody.Y - body.Height;
                                }
                            }
                            else
                            {
                                var collisionVelocity = Math.Abs(Math.Max(body.VelX, otherBody.VelX) - Math.Min(body.VelX, otherBody.VelX)) * (body.Mass + otherBody.Mass);

                                if (otherBody.IsStatic)
                                {
                                    body.VelX *= -1 * (restitution);
                                }
                                else
                                {
                                    body.ApplyImpulse((collisionVelocity) * restitution * ImpulseReactionDirection(body.X, otherBody.X), 0);
                                    otherBody.ApplyImpulse((collisionVelocity) * restitution * ImpulseReactionDirection(otherBody.X, body.X), 0);
                                }

                                if (body.X > otherBody.X)
                                {
                                    body.X = otherBody.X + otherBody.Width;
                                }
                                else
                                {
                                    body.X = otherBody.X - body.Width;
                                }
                            }
                        }

                        //if (true)
                        //{
                        //    if (otherBody.Type == BodyType.KillBox && body.Type == BodyType.Character)
                        //    {
                        //        body.Dead = true;
                        //    }
                        //}
                    }
                }
            }
        }

        private int ImpulseReactionDirection(double c1, double c2)
        {
            if (c1 < c2)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }

        private double CalcDragCoef(double speed, double drag)
        {
            if (drag > 1.0)
            {
                drag = 1.0;
            }

            var multip = speed * drag;

            return multip;
        }

        private bool ShouldIgnoreCollisionResponse(Body body1, Body body2)
        {
            if (body1.CollisionGroup < 0 || body2.CollisionGroup < 0)
            {
                return true;
            }

            if (body1.CollisionGroup == 0 || body2.CollisionGroup == 0)
            {
                return false;
            }

            if (body1.CollisionGroup == body2.CollisionGroup)
            {
                return false;
            }

            return true;
        }
        //    if (body1.Type == BodyType.KillBox || body2.Type == BodyType.KillBox)
        //    {
        //        return true;
        //    }

        //    if (body1.Type == BodyType.Character && body2.Type == BodyType.Character)
        //    {
        //        return true;
        //    }

        //    if (body1.Type == BodyType.Character && body2.Type == BodyType.DeathBox)
        //    {
        //        return true;
        //    }

        //    if (body1.Type == BodyType.DeathBox && body2.Type == BodyType.Character)
        //    {
        //        return true;
        //    }

        //    if (body1.Type == BodyType.PickUp && body2.Type == BodyType.Character)
        //    {
        //        return true;
        //    }

        //    if (body1.Type == BodyType.Character && body2.Type == BodyType.PickUp)
        //    {
        //        return true;
        //    }

        //    if (body1.Type == BodyType.EndPoint || body2.Type == BodyType.EndPoint)
        //    {
        //        return true;
        //    }

        //    return false;
        //}
    }
}
