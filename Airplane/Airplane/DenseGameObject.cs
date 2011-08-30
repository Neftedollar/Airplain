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
    class DenseGameObject : GameObject
    {
        Rectangle collisionRect;

        DenseGameObject() : base()
        {

        }
        public DenseGameObject(Vector2 position)
            : base(position)
        {
            Initialize();
        }
        public DenseGameObject(Vector2 position, Texture2D texture)
            : base(position, texture)
        {
            Initialize();
        }

        protected override void Initialize()
        {
            collisionRect = new Rectangle(0, 0, 0, 0);
            base.Initialize();
        }

        public void Collided(DenseGameObject collObject)
        {
            
            Console.WriteLine("Collision detected");
        }
    }
}
