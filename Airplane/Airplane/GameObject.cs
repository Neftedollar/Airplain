using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Airplane
{
    public class GameObject
    {
        public bool IsVisible { get; set; }
        public Vector2 Position {get;set;}  //the default object position
        public Texture2D Image {get; set;}

        public float Scale { set; get; }
        public float Rotation { set; get; }
        public Vector2 Speed { get; set; }

        public string Tag { get; set; }

        Vector2 size_;

        public Vector2 Size { 
            get
            {
                if (size_ == Vector2.Zero)
                    return new Vector2(Image.Width, Image.Height);
                else
                    return size_;
            }
            set
            {
                size_ = value;
            }
        }

        public GameObject()
        {
            Initialize();
        }

        public GameObject(Vector2 position)
        {
            Initialize();
            Position = position;
        }

        public GameObject(Rectangle rect)
        {
            Initialize();
            Position = new Vector2(rect.X,rect.Y);
            Size = new Vector2(rect.Width, rect.Height);
        }

        public GameObject(Vector2 position, Texture2D texture)
        {
            Initialize();
            Position = position;
            Image = texture;
        }

        public GameObject(Rectangle rect, Texture2D texture)
        {
            Initialize();
            Position = new Vector2(rect.X, rect.Y);
            Size = new Vector2(rect.Width, rect.Height);
            Image = texture;
        }

        protected void Initialize()
        {
            IsVisible = true;
            
            Scale = 1.0f;
            Rotation = 0.0f;

            Position = Vector2.Zero;
            Speed = Vector2.Zero;
            size_ = Vector2.Zero;
        }

        //

    }
}
