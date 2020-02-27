using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace deltaTime
{
    public enum WateringCanState { WateringCan}
    class WateringCan : Interactable
    {
        private WateringCanState state;
        private Dictionary<WateringCanState, AnimatedTexture> textures;

        public WateringCanState State
        {
            get
            {
                return state;
            }
        }

        public AnimatedTexture this[WateringCanState s]
        {
            get
            {
                return textures[s];
            }
        }

        public override string NameAndState
        {
            get
            {
                return "WateringCan_" + state.ToString();
            }
        }

        public override bool Pickupable
        {
            get
            {
                switch (state)
                {
                    case WateringCanState.WateringCan:
                    default:
                        return true;
                }
            }
        }

        public WateringCan(Rectangle position, WateringCanState state, double scale, Level level) : base(position, true, scale, level)
        {
            this.state = state;
            textures = LoadTextures<WateringCanState>(0);
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(GameTime gameTime, SpriteBatch sb)
        {
            textures[state].Draw(Position, sb, gameTime, SpriteEffects.None);
        }

        /// <summary>
        /// Draws the watering can in its current state at given position
        /// </summary>
        /// <param name="position">A rectangle setting the position to draw the watering can at</param>
        public override void DrawAt(Rectangle position, GameTime gameTime, SpriteBatch sb)
        {
            textures[state].Draw(position, sb, gameTime, SpriteEffects.None);
        }

        public override void GoToPast() //don't do anything - it's a watering can
        {
        }
        
        public override void GoToFuture() //don't do anything - it's a watering can
        {
        }

        public override void ReactToItemUse(string itemNameAndState)
        {
            switch (itemNameAndState)
            {
                default:
                    break;
            }
        }

        public override bool ReactToUsingOn(string itemNameAndState)
        {
            switch (itemNameAndState)
            {
                default:
                    return true;
            }
        }
    }
}
