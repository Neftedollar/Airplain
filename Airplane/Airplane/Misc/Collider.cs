using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            foreach (DenseGameObject checkObject in List)
            {
                //check collisions
            }

        }
    }
}
