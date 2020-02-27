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
    /// <summary>
    /// class that stores information about the Tile
    /// </summary>
    public class Tile : GameObject
    {
        protected AnimatedTexture past;
        protected AnimatedTexture future;
        protected SpriteEffects pastFlip;
        protected SpriteEffects futureFlip;
        protected bool pastCollidable;
        protected bool futureCollidable;
       
        /// <summary>
        /// Gets whether the tile is collidable in the future
        /// </summary>
        public bool FutureCollidable
        {
            get
            {
                return futureCollidable;
            }
        }

        /// <summary>
        /// Gets whether the tile is collidable in the past
        /// </summary>
        public bool PastCollidable
        {
            get
            {
                return pastCollidable;
            }
        }

        /// <summary>
        /// Gets whether the tile is collidable in the past
        /// </summary>
        public AnimatedTexture PastTexture
        {
            get
            {
                return past;
            }
        }

        /// <summary>
        /// Gets whether the tile is collidable in the past
        /// </summary>
        public AnimatedTexture FutureTexture
        {
            get
            {
                return future;
            }
        }

        /// <summary>
        /// constructor for Tile
        /// </summary>
        /// <param name="past"> the AnimatedTexture for the past version of the Tile</param>
        /// <param name="future"> the AnimatedTexture for the future version of the TIle </param>
        /// <param name="pastCollidable"> whether or not the Tile is collidable in the past</param>
        /// <param name="futureCollidable"> whether or not the TIle is collidable in the future</param>
        /// <param name="door">if this tile is the door -- end of the level</param>
        /// 
        public Tile(Rectangle position, AnimatedTexture past, int pFlip, bool pastCollidable, AnimatedTexture future, int fFlip, bool futureCollidable, double scale)
            : base(position, pastCollidable, scale)
        {
            this.past = past;

            if (pFlip == 0)
                pastFlip = SpriteEffects.None;
            else if (pFlip == 1)
                pastFlip = SpriteEffects.FlipVertically;
            else if (pFlip == 2)
                pastFlip = SpriteEffects.FlipHorizontally;
            else
                pastFlip = SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;

            this.pastCollidable = pastCollidable;
            this.future = future;

            if (fFlip == 0)
                futureFlip = SpriteEffects.None;
            else if (fFlip == 1)
                futureFlip = SpriteEffects.FlipVertically;
            else if (fFlip == 2)
                futureFlip = SpriteEffects.FlipHorizontally;
            else
                futureFlip = SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;

            this.futureCollidable = futureCollidable;
        }

        /// <summary>
        /// Really doesn't do anything don't call this
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            //implement code here
        }

        /// <summary>
        /// Calls the draw method of the animated texture based on the current timestate
        /// </summary>
        /// <param name="gameTime">Game1's gameTime</param>
        /// <param name="sb">The Official Spritebath(TM)</param>
        public override void Draw(GameTime gameTime, SpriteBatch sb)
        {
            if(Game1.timeState == TimeState.Past)
            {
                past.Draw(position, sb, gameTime, pastFlip);
            }
            else
            {
                future.Draw(position, sb, gameTime, futureFlip);
            }
        }
        /// <summary>
        /// Draw the tile in the other time state.
        /// </summary>
        /// <param name="gameTime"> The gametime</param>
        /// <param name="sb"> The spritebatch</param>
        public void DrawOtherTimeState(GameTime gameTime, SpriteBatch sb)
        {
            //If the gamestate is future then draw the past version of the tile
            if (Game1.timeState == TimeState.Future)
            {
                past.Draw(position, sb, gameTime, pastFlip, .5f, Color.Red);
            }
            //If the gamestate is the past then draw the future version of the tile.
            else
            {
                future.Draw(position, sb, gameTime, futureFlip, .5f, Color.Red);
            }
        }
    }
}
