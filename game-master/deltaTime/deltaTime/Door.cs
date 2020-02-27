using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace deltaTime
{
    class Door : Tile
    {
        private AnimatedTexture pastOpen;
        private AnimatedTexture futureOpen;
        private AnimatedTexture pastClosed;
        private AnimatedTexture futureClosed;
        private bool open;

        private List<Recepticle> recepticles;

        public Door(Rectangle position, AnimatedTexture past, int pFlip, bool pastCollidable, AnimatedTexture future, int fFlip, bool futureCollidable, double scale)
            : base(position, past, pFlip, pastCollidable, future, fFlip, futureCollidable, scale)
        {
            //four types of doors:
            //vertical or horizontal, interior or exterior
            //This finds which combination of those two variables it is and sets all four possible states to the correct texture
            //those states being open or closed, in the past or future
            if(past.SpriteSheet.Name == "PastLockedDoorExtHoriz" || past.SpriteSheet.Name == "PastOpenDoorExtHoriz"
                || future.SpriteSheet.Name.Contains("FutureLockedDoorExtHoriz") || future.SpriteSheet.Name.Contains("FutureOpenDoorExtHoriz"))
            {
                pastOpen = new AnimatedTexture(1, 32, 32, Game1.tileTextures["PastOpenDoorExtHoriz"]);
                futureOpen = new AnimatedTexture(1, 32, 32, Game1.tileTextures["FutureOpenDoorExtHoriz0"]);
                pastClosed = new AnimatedTexture(1, 32, 32, Game1.tileTextures["PastLockedDoorExtHoriz"]);
                futureClosed = new AnimatedTexture(1, 32, 32, Game1.tileTextures["FutureLockedDoorExtHoriz0"]);
            }
            else if (past.SpriteSheet.Name == "PastLockedDoorExtVert" || past.SpriteSheet.Name == "PastOpenDoorExtVert"
                || future.SpriteSheet.Name.Contains("FutureLockedDoorExtVert") || future.SpriteSheet.Name.Contains("FutureOpenDoorExtVert"))
            {
                pastOpen = new AnimatedTexture(1, 32, 32, Game1.tileTextures["PastOpenDoorExtVert"]);
                futureOpen = new AnimatedTexture(1, 32, 32, Game1.tileTextures["FutureOpenDoorExtVert0"]);
                pastClosed = new AnimatedTexture(1, 32, 32, Game1.tileTextures["PastLockedDoorExtVert"]);
                futureClosed = new AnimatedTexture(1, 32, 32, Game1.tileTextures["FutureLockedDoorExtVert0"]);
            }
            else if (past.SpriteSheet.Name == "PastLockedDoorIntHoriz" || past.SpriteSheet.Name == "PastOpenDoorIntHoriz"
                || future.SpriteSheet.Name.Contains("FutureLockedDoorIntHoriz") || future.SpriteSheet.Name.Contains("FutureOpenDoorIntHoriz"))
            {
                pastOpen = new AnimatedTexture(1, 32, 32, Game1.tileTextures["PastOpenDoorIntHoriz"]);
                futureOpen = new AnimatedTexture(1, 32, 32, Game1.tileTextures["FutureOpenDoorIntHoriz0"]);
                pastClosed = new AnimatedTexture(1, 32, 32, Game1.tileTextures["PastLockedDoorIntHoriz"]);
                futureClosed = new AnimatedTexture(1, 32, 32, Game1.tileTextures["FutureLockedDoorIntHoriz0"]);
            }
            else if (past.SpriteSheet.Name == "PastLockedDoorIntVert" || past.SpriteSheet.Name == "PastOpenDoorIntVert"
                || future.SpriteSheet.Name.Contains("FutureLockedDoorIntVert") || future.SpriteSheet.Name.Contains("FutureOpenDoorIntVert"))
            {
                pastOpen = new AnimatedTexture(1, 32, 32, Game1.tileTextures["PastOpenDoorIntVert"]);
                futureOpen = new AnimatedTexture(1, 32, 32, Game1.tileTextures["FutureOpenDoorIntVert0"]);
                pastClosed = new AnimatedTexture(1, 32, 32, Game1.tileTextures["PastLockedDoorIntVert"]);
                futureClosed = new AnimatedTexture(1, 32, 32, Game1.tileTextures["FutureLockedDoorIntVert0"]);
            }

            //if the door is open by default, make sure that players can walk through it
            if(past.SpriteSheet.Name == "PastOpenDoorExtHoriz" || past.SpriteSheet.Name == "PastOpenDoorExtVert" ||
               past.SpriteSheet.Name == "PastOpenDoorIntHoriz" || past.SpriteSheet.Name == "PastOpenDoorIntVert")
            {
                pastCollidable = false;
            }
            if (past.SpriteSheet.Name.Contains("FutureOpenDoorExtHoriz") || past.SpriteSheet.Name.Contains("FutureOpenDoorExtVert") ||
                past.SpriteSheet.Name.Contains("FutureOpenDoorIntHoriz") || past.SpriteSheet.Name.Contains("FutureOpenDoorIntVert"))
            {
                futureCollidable = false;
            }
            recepticles = new List<Recepticle>();
        }

        //public bool Open
        //{
        //    get
        //    {
        //        return open;
        //    }
        //}

        /// <summary>
        /// Links a new recepticle to the door
        /// </summary>
        /// <param name="r">The recepticle to add a link to</param>
        public void LinkRecepticle(Recepticle r) {
            recepticles.Add(r);
        }

        public override void Update(GameTime gameTime)
        {
            bool allRecepticlesOn = true;
            foreach(Recepticle r in recepticles) //Check to make sure all linked recepticles are on
            {
                if(r.On == false)
                {
                    allRecepticlesOn = false;
                }
            }

            //makes sure that:
            //-All recepticles linked to this door are active
            //-makes sure that the recepticles exist in this time state
            //This makes sure that the door only opens in the time that the recepticles are activated in
            if((allRecepticlesOn && recepticles.Count > 0 && recepticles[0].Exists) || recepticles.Count == 0)
            {
                Open(Game1.timeState);
            } else
            {
                Closed(Game1.timeState);
            }
        }

        public void Open(TimeState ts)
        {
            //check the time that we're activating the door
            //make sure that the tile is a door in that time state and not a wall
            if (ts == TimeState.Past && past.SpriteSheet.Name.ToLower().Contains("door"))
            {
                past = pastOpen;
                pastCollidable = false;
                open = true;
            }
            else if (future.SpriteSheet.Name.ToLower().Contains("door"))
            {
                future = futureOpen;
                futureCollidable = false;
                open = true;
            }
        }

        public void Closed(TimeState ts)
        {
            if (ts == TimeState.Past && past.SpriteSheet.Name.ToLower().Contains("door"))
            {
                past = pastClosed;
                pastCollidable = true;
                open = false;
            }
            else if (future.SpriteSheet.Name.ToLower().Contains("door"))
            {
                future = futureClosed;
                futureCollidable = true;
                open = false;
            }
        }
    }
}
