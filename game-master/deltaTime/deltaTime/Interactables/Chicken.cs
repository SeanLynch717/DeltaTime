using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace deltaTime
{
    /// <summary>
    /// enum for the different possible states of Chicken
    /// </summary>
    public enum ChickenState { PastNothing, Egg, Chick, Chicken, Bones, FutureNothing }

    /// <summary>
    /// class that stores data about the chicken 
    /// </summary>
    class Chicken : Interactable
    {
        private ChickenState state;
        private Dictionary<ChickenState, AnimatedTexture> textures;

        /// <summary>
        /// Gets the state of the chicken
        /// </summary>
        public ChickenState State
        {
            get
            {
                return state;
            }
        }

        /// <summary>
        /// Gets or sets <c>AnimatedTexture</c>s of the Chicken, indexed by state
        /// </summary>
        /// <param name="c">The state the chicken is in</param>
        /// <returns>The <c>AnimatedTexture</c> for that state of the chicken</returns>
        public AnimatedTexture this[ChickenState s]
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
                return "Chicken_" + state.ToString();
            }
        }

        public override bool Pickupable
        {
            get
            {
                switch (state)
                {
                    case ChickenState.PastNothing:
                    case ChickenState.FutureNothing:
                        return false;
                    case ChickenState.Egg:
                    case ChickenState.Chick:
                    case ChickenState.Chicken:
                    case ChickenState.Bones:
                    default:
                        return true;
                }
            }
        }
        /// <summary>
        /// Creates a chicken at given position and at given state. Collidable by default, (if we find a need for uncollidable chickens we'll need an extra parameter here).
        /// </summary>
        /// <param name="position">The starting position of the chicken.</param>
        /// <param name="state">The starting state of the chicken</param>
        public Chicken(Rectangle position, ChickenState state, double scale, Level level) : base(position, true, scale, level)
        {
            this.state = state;
            textures = LoadTextures<ChickenState>(2);
        }

        /// <summary>
        /// Updates the chickens finite state machine.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            //implement code here
        }

        /// <summary>
        /// Draws the chicken at its position and current state.
        /// </summary>
        public override void Draw(GameTime gameTime, SpriteBatch sb)
        {
            textures[state].Draw(Position, sb, gameTime, SpriteEffects.None);
        }

        /// <summary>
        /// Draws the chicken in its current state at given position
        /// </summary>
        /// <param name="position">A rectangle setting the position to draw the chicken at</param>
        public override void DrawAt(Rectangle position, GameTime gameTime, SpriteBatch sb)
        {
            textures[state].Draw(position, sb, gameTime, SpriteEffects.None);
        }

        /// <summary>
        /// changes the current state of the chicken to its previous state as long as it isn't in its base state
        /// </summary>
        public override void GoToPast()
        {
            switch (state)
            {
                case ChickenState.PastNothing: //this should never happen
                    break;
                case ChickenState.Egg:
                    state = ChickenState.PastNothing;
                    Collidable = false;
                    break;
                case ChickenState.Chick:
                    state = ChickenState.Egg;
                    Collidable = true;
                    break;
                case ChickenState.Chicken:
                    Game1.particleManager.AddGenericParticleSystem(new AnimatedTexture(0, 16, 16, Game1.staticTextureManager["SparkParticle"]), position);
                    state = ChickenState.Chick;
                    Collidable = true;
                    break;
                case ChickenState.Bones:
                    Game1.particleManager.AddGenericParticleSystem(new AnimatedTexture(0, 16, 16, Game1.staticTextureManager["FeatherParticle"]), position);
                    state = ChickenState.Chicken;
                    Collidable = true;
                    break;
                case ChickenState.FutureNothing:
                    state = ChickenState.Bones;
                    Collidable = true;
                    break;
            }
        }

        /// <summary>
        /// changes the current state of the chicken to its next state as long as it isn't in its oldest state.
        /// </summary>
        public override void GoToFuture()
        {
            switch (state)
            {
                case ChickenState.PastNothing:
                    state = ChickenState.Egg;
                    Collidable = true;
                    break;
                case ChickenState.Egg:
                    Game1.particleManager.AddGenericParticleSystem(new AnimatedTexture(0, 16, 16, Game1.staticTextureManager["SparkParticle"]), position);
                    state = ChickenState.Chick;
                    Collidable = true;
                    break;
                case ChickenState.Chick:
                    Game1.particleManager.AddGenericParticleSystem(new AnimatedTexture(0, 16, 16, Game1.staticTextureManager["FeatherParticle"]), position);
                    state = ChickenState.Chicken;
                    Collidable = true;
                    break;
                case ChickenState.Chicken:
                    state = ChickenState.Bones;
                    Collidable = true;
                    break;
                case ChickenState.Bones:
                    state = ChickenState.FutureNothing;
                    Collidable = false;
                    break;
                case ChickenState.FutureNothing: //this should never happen
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
