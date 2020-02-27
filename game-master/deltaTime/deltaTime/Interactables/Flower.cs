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
    public enum FlowerState {Dirt, WateredDirt, Seeds, WateredSeeds, Flower, WateredFlower, HeldFlower}
    class Flower : Interactable
    {
        private FlowerState state;
        private Dictionary<FlowerState, AnimatedTexture> textures;

        /// <summary>
        /// Gets the state of the flower
        /// </summary>
        public FlowerState State
        {
            get
            {
                return state;
            }
        }

        /// <summary>
        /// Gets or sets <c>AnimatedTexture</c>s of the Flower, indexed by state
        /// </summary>
        /// <param name="s">The state the flower is in</param>
        /// <returns>The <c>AnimatedTexture</c> for that state of the chicken</returns>
        public AnimatedTexture this[FlowerState s]
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
                return "Flower_" + state.ToString();
            }
        }

        public override bool Pickupable
        {
            get
            {
                switch (state)
                {
                    case FlowerState.Dirt:
                    case FlowerState.WateredDirt:
                    case FlowerState.Seeds:
                    case FlowerState.WateredSeeds:
                    case FlowerState.Flower:
                    case FlowerState.WateredFlower:
                        return false;

                    case FlowerState.HeldFlower:
                    default:
                        return true;
                }
            }
        }
        /// <summary>
        /// Creates a flower at given position and at given state. Uncollidable by default.
        /// </summary>
        /// <param name="position">The starting position of the chicken</param>
        /// <param name="state">The starting state of the chicken</param>
        /// <param name="scale"></param>
        public Flower(Rectangle position, FlowerState state, double scale, Level level) : base(position, true, scale, level)
        {
            this.state = state;
            textures = LoadTextures<FlowerState>(0);
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        /// <summary>
        /// Draws the flower at its position and current state
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="sb"></param>
        public override void Draw(GameTime gameTime, SpriteBatch sb)
        {
            textures[state].Draw(Position, sb, gameTime, SpriteEffects.None);
        }

        /// <summary>
        /// Draws the flower in its current state at given position
        /// </summary>
        /// <param name="position">A rectangle setting the position to draw the flower at</param>
        public override void DrawAt(Rectangle position, GameTime gameTime, SpriteBatch sb)
        {
            textures[state].Draw(position, sb, gameTime, SpriteEffects.None);
        }

        public override void GoToPast()
        {
            switch(state)
            {
                case FlowerState.Dirt:
                    break;
                case FlowerState.WateredDirt:
                    state = FlowerState.Dirt;
                    break;
                case FlowerState.Seeds:
                    break;
                case FlowerState.WateredSeeds:
                    state = FlowerState.Seeds;
                    break;
                case FlowerState.Flower:
                    state = FlowerState.WateredSeeds;
                    break;
                case FlowerState.WateredFlower:
                    state = FlowerState.Flower;
                    break;
                case FlowerState.HeldFlower:
                    break;
            }
        }

        public override void GoToFuture()
        {
            switch (state)
            {
                case FlowerState.Dirt:
                    break;
                case FlowerState.WateredDirt:
                    state = FlowerState.Dirt;
                    break;
                case FlowerState.Seeds:
                    break;
                case FlowerState.WateredSeeds:
                    Game1.particleManager.AddGenericParticleSystem(new AnimatedTexture(0, 16, 16, Game1.staticTextureManager["LeafParticle"]), position);
                    state = FlowerState.Flower;
                    break;
                case FlowerState.Flower:
                    break;
                case FlowerState.WateredFlower:
                    state = FlowerState.Flower;
                    break;
                case FlowerState.HeldFlower:
                    break;
            }
        }

        public override void ReactToItemUse(string itemNameAndState)
        {
            switch (itemNameAndState)
            {
                case "null":
                    GetPicked();
                    break;
                case "WateringCan_WateringCan":
                    GetWatered();
                    break;
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

        /// <summary>
        /// Changes unwatered states to their corresponding watered states
        /// </summary>
        private void GetWatered()
        {
            Game1.particleManager.AddGenericParticleSystem(new AnimatedTexture(0, 16, 16, Game1.staticTextureManager["WaterParticle"]), position);
            switch (state)
            {
                case FlowerState.Dirt:
                    state = FlowerState.WateredDirt;
                    break;
                case FlowerState.WateredDirt:
                    break;
                case FlowerState.Seeds:
                    state = FlowerState.WateredSeeds;
                    break;
                case FlowerState.WateredSeeds:
                    break;
                case FlowerState.Flower:
                    state = FlowerState.WateredFlower;
                    break;
                case FlowerState.WateredFlower:
                    break;
                case FlowerState.HeldFlower:
                    break;
            }
        }

        /// <summary>
        /// Changes states with a flower to their corresponding dirt states
        /// </summary>
        private void GetPicked()
        {
            switch(state)
            {
                case FlowerState.Dirt:
                    break;
                case FlowerState.WateredDirt:
                    break;
                case FlowerState.Seeds:
                    break;
                case FlowerState.WateredSeeds:
                    break;
                case FlowerState.Flower:
                    Game1.particleManager.AddGenericParticleSystem(new AnimatedTexture(0, 16, 16, Game1.staticTextureManager["LeafParticle"]), position);
                    state = FlowerState.Dirt;
                    break;
                case FlowerState.WateredFlower:
                    Game1.particleManager.AddGenericParticleSystem(new AnimatedTexture(0, 16, 16, Game1.staticTextureManager["LeafParticle"]), position);
                    state = FlowerState.WateredDirt;
                    break;
                case FlowerState.HeldFlower:
                    break;
            }
        }
    }


}
