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

    public delegate void ObjectEventVoidDelegate();
    /// <summary>
    /// An ingame object.
    /// </summary>
    /// <param name="Position">A 2D vector.</param>
    /// <param name="Size">Gets or sets a 2D vector that stores width and height of an object's image. By default it is the same as the loaded image size.</param>
    /// <param name="Image">A </param>
    public class GameObject
    {
        public bool IsVisible { get; set; }
        public Vector2 Position {get;set;} 
        public Texture2D Image {get; set;}

        public float Scale { set; get; }
        public float Rotation { set; get; }
        public Vector2 Speed { get; set; }

        public string Tag { get; set; }

        public Vector2 SizeScaled { private set{}
            get
            {
                return Size * Scale;
            }
        }

        Vector2 size_;

        //GameObject events
        /*public ObjectEventVoidDelegate onScreenLeftLeft{ get; set; }
        public ObjectEventVoidDelegate onScreenLeftRight { get; set; }
        public ObjectEventVoidDelegate onScreenLeftTop { get; set; }
        public ObjectEventVoidDelegate onScreenLeftBottom { get; set; }*/

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
    }
}
