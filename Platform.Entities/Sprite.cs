using OpenTK;
using OpenTK.Input;
using Platform.Core;
using Platform.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TK.Graphics;

namespace Platform.Entities
{
    public abstract class Sprite : Body
    {
        public SpriteType SpriteType { get; set; }
        //public bool Dead { get; set; }
        //public BitmapImage Texture { get; set; }
        public Texture2D Texture { get; set; }
        //public Brush Brush { get; set; }
        public int Damageful { get; set; }
        public Vector2 PositiveTextureOffset { get; set; }
        public Vector2 NegativeTextureOffset { get; set; }
        public double Z { get; set; }
        //public virtual Transform Transform
        //{
        //    get
        //    {
        //        return new ScaleTransform();
        //    }
        //}

        public Sprite(SpriteType spriteType, int x, int y)
        {
            this.X = x;
            this.Y = y;
            this.Z = 1;
            this.NegativeTextureOffset = new Vector2(0f, 0f);
            this.PositiveTextureOffset = new Vector2(0f, 0f);
            SpriteType = spriteType;
        }

        public virtual void Update(TimeSpan delta, KeyboardState keyboardStatel, KeyboardState previousKeyboardState)
        {
            // default does nothing
        }

        public virtual void Draw()
        {
            if (this.Dead || this.Texture == null)
            {
                return;
            }

            SpriteRender.Draw(
                this.X - this.NegativeTextureOffset.X,
                this.Y - this.NegativeTextureOffset.Y,
                this.Z,
                this.Width + this.PositiveTextureOffset.X,
                this.Height + this.PositiveTextureOffset.Y,
                this.Texture);

            //UIElement element = new UIElement();
            //if (Texture != null)
            //{
            //    element = new Image();
            //    (element as Image).Source = Texture;
            //    (element as Image).RenderTransform = Transform;
            //}
            //else
            //{
            //    element = new Rectangle();

            //    (element as Rectangle).StrokeThickness = 2;
            //    (element as Rectangle).Width = this.Width;
            //    (element as Rectangle).Height = this.Height;
            //    (element as Rectangle).Stroke = Brush;
            //}

            //Canvas.SetLeft(element, this.X);
            //Canvas.SetTop(element, this.Y);

            //canvas.Children.Add(element);
        }
    }
}
