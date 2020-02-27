using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace deltaTime
{
    /*INSTRUCTIONS:
	 * 1. Copy this into your interactable file
	 * 2. Find and replace Campfire with the name of your interactable
	 * 3. Edit the state machine on line 20 to add all necessary states
	 * 4. Set pickupable states on line 50
	 * 5. Add any needed functionality to GoToPast(), GoToFuture(), and ReactToItemUse()
	 * 6. Delete this comment if you want
	 */

    public enum CampfireState {UnlitCampfire, LitCampfire, PileOfAsh }
    class Campfire : Interactable
    {
        private CampfireState state;
        private Dictionary<CampfireState, AnimatedTexture> textures;

        public CampfireState State
        {
            get
            {
                return state;
            }
        }

        public AnimatedTexture this[CampfireState s]
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
                return "Campfire_" + state.ToString();
            }
        }

        public override bool Pickupable
        {
            get
            {
                switch (state)
                {
                    case CampfireState.UnlitCampfire:
                    case CampfireState.LitCampfire:
                    case CampfireState.PileOfAsh:
                    default:
                        return false;
                }
            }
        }
        public Campfire(Rectangle position, CampfireState state, double scale, Level level) : base(position, true, scale, level)
        {
            this.state = state;
            textures = LoadTextures<CampfireState>(12);
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(GameTime gameTime, SpriteBatch sb)
        {
            textures[state].Draw(Position, sb, gameTime, SpriteEffects.None);
        }

        /// <summary>
        /// Draws the Campfire in its current state at given position
        /// </summary>
        /// <param name="position">A rectangle setting the position to draw the Campfire at</param>
        public override void DrawAt(Rectangle position, GameTime gameTime, SpriteBatch sb)
        {
            textures[state].Draw(position, sb, gameTime, SpriteEffects.None);
        }

        /// <summary>
        /// changes the current state of the Campfire to its past state
        /// </summary>
        public override void GoToPast()
        {
            switch (state)
            {
                case CampfireState.UnlitCampfire:
                    break;
                case CampfireState.LitCampfire:
                    state = CampfireState.UnlitCampfire;
                    break;
                case CampfireState.PileOfAsh:
                    state = CampfireState.LitCampfire;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// changes the current state of the Campfire to its future state
        /// </summary>
        public override void GoToFuture()
        {
            switch (state)
            {
                case CampfireState.UnlitCampfire:
                    break;
                case CampfireState.LitCampfire:
                    state = CampfireState.PileOfAsh;
                    break;
                case CampfireState.PileOfAsh:
                    break;
                default:
                    break;
            }
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
