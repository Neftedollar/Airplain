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
    class Collider : System.Collections.CollectionBase
    {
        public Collider()
        {

        }

        public void addObject(DenseGameObject obj)
        {
            List.Add(obj);
        }

        public void addObject(DenseGameObject[] objs)
        {
            foreach(DenseGameObject obj in objs)
                List.Add(obj);
        }

        public void CheckCollisions()
        {
            //check each with each objects not more than one time
            for (int i = 0; i < List.Count-1; i++)
            {
                for (int j = i + 1; j < List.Count; j++)
                {
                    checkCollisionBetween((DenseGameObject)List[i], (DenseGameObject)List[j]); //? is it good to perform this conversion
                }
            }
        }

        /// <summary>
        /// Check if object1 and object2 intersect each other by its collision rectangles. If true calls its CollisionEvent if defined.
        /// </summary>
        protected void checkCollisionBetween(DenseGameObject obj1, DenseGameObject obj2)
        {
            //Warning! Width and Height params are coordinates of the right top rectangle corner, not width and height of the rectangle
            //the scale may cause troubles in future
            Rectangle obj1_rect = new Rectangle(
                (int)(obj1.Position.X + obj1.CollisionRect.X), 
                (int)(obj1.Position.Y + obj1.CollisionRect.Y),
                (int)(obj1.Position.X + obj1.CollisionRect.X + obj1.CollisionRect.Width * obj1.Scale),
                (int)(obj1.Position.Y + obj1.CollisionRect.Y + obj1.CollisionRect.Height * obj1.Scale));

           Rectangle obj2_rect = new Rectangle(
                (int)(obj2.Position.X + obj2.CollisionRect.X), 
                (int)(obj2.Position.Y + obj2.CollisionRect.Y),
                (int)(obj2.Position.X + obj2.CollisionRect.X + obj2.CollisionRect.Width * obj2.Scale),
                (int)(obj2.Position.Y + obj2.CollisionRect.Y + obj2.CollisionRect.Height * obj2.Scale));

            if (checkRectanglesCollision(obj1_rect, obj2_rect) == true)
            {
                if (obj1.CollisionEvent != (CollisionEventDelegate)null)
                    obj1.CollisionEvent(obj1, obj2);
                if (obj2.CollisionEvent != (CollisionEventDelegate)null)   //???
                    obj2.CollisionEvent(obj2, obj1);
            }
        }

        bool checkRectanglesCollision(Rectangle rect1, Rectangle rect2)
        {
            ////Warning! Width and Height params are coordinates of the right top rectangle corner, not width and height of the rectangle
            //recheck this method later
            //is recr1 intersects rect2
            if (isPointInRectangle(new Vector2(rect1.X, rect1.Y), rect2))
                return true;
            if (isPointInRectangle(new Vector2(rect1.Width, rect1.Y), rect2))
                return true;
            if (isPointInRectangle(new Vector2(rect1.X, rect1.Height), rect2))
                return true;
            if (isPointInRectangle(new Vector2(rect1.Width, rect1.Height), rect2))
                return true;
            //is recr2 intersects rect1
            if (isPointInRectangle(new Vector2(rect2.X, rect2.Y), rect1))
                return true;
            if (isPointInRectangle(new Vector2(rect2.Width, rect2.Y), rect1))
                return true;
            if (isPointInRectangle(new Vector2(rect2.X, rect2.Height), rect1))
                return true;
            if (isPointInRectangle(new Vector2(rect2.Width, rect2.Height), rect1))
                return true;
            return false;
        }
        bool isPointInRectangle(Vector2 point, Rectangle rect)
        {
            //Warning! Width and Height params are coordinates of the right top rectangle corner, not width and height of the rectangle 
            if (point.X >= rect.X && point.X <= rect.Width && point.Y >= rect.Y && point.Y <= rect.Height)
                return true;
            return false;
        }
    }
}
