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
        public bool isVisible { get; set; }
        public Vector2 Position {get;set;}  //the default object position
        public Texture2D Image {get; set;}

        public float Scale { set; get; }
        public float Rotation { set; get; }
        public Vector2 Speed { get; set; }

        public GameObject()
        {
            Initialize();
        }

        public GameObject(Vector2 position)
        {
            Initialize();
            Position = position;
        }

        public GameObject(Vector2 position, Texture2D texture)
        {
            Initialize();
            Position = position;
            Image = texture;
        }

        protected virtual void Initialize()
        {
            isVisible = true;
            Position = new Vector2(0, 0);
            Scale = 1.0f;
            Rotation = 0.0f;
            Speed = new Vector2(0,0);
        }

        //

    }
}
