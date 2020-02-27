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
	 * 2. Find and replace Axe with the name of your interactable
	 * 3. Edit the state machine on line 20 to add all necessary states
	 * 4. Set pickupable states on line 50
	 * 5. Add any needed functionality to GoToPast(), GoToFuture(), and ReactToItemUse()
	 * 6. Delete this comment if you want
	 */

    public enum AxeState {Axe }
    class Axe : Interactable
    {
        private AxeState state;
        private Dictionary<AxeState, AnimatedTexture> textures;

        public AxeState State
        {
            get
            {
                return state;
            }
        }

        public AnimatedTexture this[AxeState s]
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
                return "Axe_" + state.ToString();
            }
        }

        public override bool Pickupable
        {
            get
            {
                switch (state)
                {
                    case AxeState.Axe:
                    default:
                        return true;
                }
            }
        }
        public Axe(Rectangle position, AxeState state, double scale, Level level) : base(position, true, scale, level)
        {
            this.state = state;
            textures = LoadTextures<AxeState>(0);
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(GameTime gameTime, SpriteBatch sb)
        {
            textures[state].Draw(Position, sb, gameTime, SpriteEffects.None);
        }

        /// <summary>
        /// Draws the Axe in its current state at given position
        /// </summary>
        /// <param name="position">A rectangle setting the position to draw the Axe at</param>
        public override void DrawAt(Rectangle position, GameTime gameTime, SpriteBatch sb)
        {
            textures[state].Draw(position, sb, gameTime, SpriteEffects.None);
        }

        /// <summary>
        /// changes the current state of the Axe to its past state
        /// </summary>
        public override void GoToPast()
        {

        }

        /// <summary>
        /// changes the current state of the Axe to its future state
        /// </summary>
        public override void GoToFuture()
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
