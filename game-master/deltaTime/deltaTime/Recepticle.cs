using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace deltaTime
{
    public class Recepticle : GameObject
    {
        private Level currentLevel;
        private Interactable heldItem;
        private String nameAndState;
        private AnimatedTexture onTexture;
        private AnimatedTexture offTexture;
        private AnimatedTexture requiredItemTexture;
        private TimeState timeExists;

        private bool on;

        /// <summary>
        /// Gets whether the recepticle is on or off
        /// </summary>
        public bool On
        {
            get
            {
                return on;
            }
        }

        public bool Exists
        {
            get
            {
                return timeExists == Game1.timeState;
            }
        }
        /// <summary>
        /// Gets the coordinates of the recepticle relative to the grid
        /// </summary>
        public Vector2 GridCoords
        {
            get
            {
                return new Vector2(position.X / 64, position.Y / 64);
            }
        }

        public Recepticle(TimeState ts, string nameAndState, Rectangle position):base(position, true, 1)
        {
            heldItem = null;
            this.nameAndState = nameAndState;

            timeExists = ts;

            CheckValidity();
            onTexture = new AnimatedTexture(0, 16, 16, Game1.staticTextureManager["OnRecepticle"]);
            offTexture = new AnimatedTexture(0, 16, 16, Game1.staticTextureManager["OffRecepticle"]);
            requiredItemTexture = new AnimatedTexture(0, 16, 16, Game1.staticTextureManager[nameAndState]);
        }
         
        public Level CurrentLevel
        {
            set
            {
                currentLevel = value;
            }
        }

        public Interactable Item
        {
            get
            {
                return heldItem;
            }
            set
            {
                heldItem = value;
                if(heldItem != null)
                {
                    CheckValidity();
                }
                
            }
        }

        public override void Update(GameTime gameTime)
        {
            

        }

        public override void Draw(GameTime gameTime, SpriteBatch sb)
        {
            //timeExists decides what time a receptIcle exists
            //we only want one
            if (Game1.timeState == timeExists)
            {
                if (on)
                {
                    onTexture.Draw(position, sb, gameTime, SpriteEffects.None);
                }
                else
                {
                    offTexture.Draw(position, sb, gameTime, SpriteEffects.None);
                }

                requiredItemTexture.Draw(GetItemIndicatorRect(), sb, gameTime, SpriteEffects.None);
                if (heldItem != null)
                {
                    heldItem.DrawAt(position, gameTime, sb);
                }
            }
        }

        private Rectangle GetItemIndicatorRect()
        {
            Rectangle returnRect = new Rectangle(position.X + 16, position.Y + 16, 32, 32);
            return returnRect;
        }
        /// <summary>
        /// Accepts a new item into the recepticle, then checks to see if the door should open or close
        /// </summary>
        /// <param name="i">The new item to accept into the recepticle</param>
        /// <returns>True if an item was successfully put into the recepticle, false otherwise</returns>
        public bool AcceptNewItem(Interactable i)
        {
            if(heldItem != null)
            {
                return false;
            }
            heldItem = i;

            CheckValidity();
            return true;
        }

        /// <summary>
        /// Removes the current item from the recepticle, then checks to see if the door should open or close
        /// </summary>
        public void RemoveItem()
        {
            heldItem = null;
            CheckValidity();
        }

        /// <summary>
        /// Checks whether the current held item is the correct one
        /// </summary>
        private void CheckValidity()
        {
            if(heldItem == null)
            {
                on = false;
            } else
            {
                on = (heldItem.NameAndState == nameAndState && Game1.timeState == timeExists);
            }
        }
    }
}
