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

using System.Diagnostics;



namespace Airplane
{
    public delegate void DCollisionEvent(DenseGameObject obj);

    public class DenseGameObject : GameObject
    {
        //respect to the object position
        public DCollisionEvent CollisionEvent { get; set; }
        public Rectangle CollisionRect { set; get; }

        DenseGameObject() : base()
        {

        }
        public DenseGameObject(Vector2 position)
            : base(position)
        {
            Initialize();
        }

        public DenseGameObject(Rectangle rect)
            : base(new Vector2(rect.X, rect.Y))
        {
            Initialize();
            CollisionRect = rect;
        }

        public DenseGameObject(Vector2 position, Texture2D texture)
            : base(position, texture)
        {
            Initialize();
        }

        public DenseGameObject(Rectangle rect, Texture2D texture)
            : base(rect, texture)
        {
            Initialize();
            //CollisionRect = rect;
        }

        protected new void Initialize() 
        {
            CollisionRect = new Rectangle(0, 0, (int)base.Size.X, (int)base.Size.Y);
        }

        public void Collided(DenseGameObject collObject)
        {
            
            Console.WriteLine("Collision detected");
        }
    }
}
