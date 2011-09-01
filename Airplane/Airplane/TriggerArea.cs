using System;
using System.Collections.Generic;
using System.Collections;
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
    public delegate void AreaTriggerDelegate(GameObject obj);

    /// <summary>
    /// Sensitive area, that reacts, when specified objects get inside or out. Doesn't work as it should work now;
    /// </summary>
    public class TriggerArea: System.Collections.DictionaryBase
    {
        public Rectangle AreaRectangle { set; get; }
        public AreaTriggerDelegate OnObjectLeft { set; get; }
        public AreaTriggerDelegate OnObjectCame { set; get; }

        enum ObjectState { OUT, IN, MID }

        public TriggerArea()
        {
            AreaRectangle = new Rectangle(0, 0, 0, 0);
        }

        public TriggerArea(Rectangle rect)
        {
            AreaRectangle = rect;
        }

        public void addObject(GameObject obj)
        {
            Dictionary.Add(obj, isObjectInsideArea(obj));
        }
        public void addObjects(GameObject[] objs)
        {
            foreach(GameObject obj in objs)
                Dictionary.Add(obj, isObjectInsideArea(obj));
        }

        public void checkAreaObjects()
        {
            foreach (DictionaryEntry entry in Dictionary)
            {
                bool isInside = isObjectInsideArea((GameObject)entry.Key);
                if (isInside != (bool)entry.Value)
                {
                    if (isInside == true)
                    {
                        if (OnObjectCame != (AreaTriggerDelegate)null)
                            OnObjectCame((GameObject)entry.Key);
                    }
                    else
                    {
                        if (OnObjectLeft != (AreaTriggerDelegate)null)
                            OnObjectLeft((GameObject)entry.Key);
                    }

                }
            }
        }

        protected bool isObjectInsideArea(GameObject obj)
        {
            if (obj.Position.X > AreaRectangle.X && obj.Position.Y > AreaRectangle.Y &&
                (obj.Position.X + obj.Size.X) < AreaRectangle.X + AreaRectangle.Width &&
                (obj.Position.Y + obj.Size.Y) < AreaRectangle.Y + AreaRectangle.Height)
            {
                return true;
            }

            return false;
        }

    }
}
