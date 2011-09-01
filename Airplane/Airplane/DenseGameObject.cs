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

    public delegate void CollisionEventDelegate(DenseGameObject obj);

    /// <summary>
    /// An object that can interact with other "dense" objects. 
    /// </summary>
    /// <param name="CollisionRect">Rectangle that sets "sensitive" region. Is relative to the object position.</param>
    /// <param name="CollisionEvent">Delegate method that will be called if collision happenes</param>
    public class DenseGameObject : GameObject
    {
        public Rectangle CollisionRect { set; get; }
        public CollisionEventDelegate CollisionEvent { get; set; }

        DenseGameObject() : base()
        {

        }
        public DenseGameObject(Vector2 position)
            : base(position)
        {
            Initialize();
        }

        public DenseGameObject(Rectangle rect)
            : base(rect)
        {
            Initialize();
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
        }

        protected new void Initialize() 
        {
            // By default collision rectangle is the same as sprite region
            CollisionRect = new Rectangle(0, 0, (int)base.Size.X, (int)base.Size.Y);
        }

        public void Collided(DenseGameObject collObject)
        {
            
            Console.WriteLine("Collision detected");
        }
    }
}
