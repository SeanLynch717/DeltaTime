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
    public enum TreeState { Dirt, Sapling, Tree, DeadTree, BagSapling, ChoppedTree, Log, DirtThatWillBeSapling}
    class Tree : Interactable
    {
        
        private TreeState state;
        private Dictionary<TreeState, AnimatedTexture> textures;

        /// <summary>
        /// Gets the state of the tree
        /// </summary>
        public TreeState State
        {
            get
            {
                return state;
            }
        }

        public AnimatedTexture this[TreeState s]
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
                return "Tree_" + state.ToString();
            }
        }

        public override bool Pickupable
        {
            get
            {
                switch (state)
                {
                    case TreeState.Dirt:
                    case TreeState.Sapling:
                    case TreeState.DeadTree:
                    case TreeState.ChoppedTree:
                    case TreeState.DirtThatWillBeSapling:
                        return false;
                    case TreeState.BagSapling:
                    case TreeState.Log:
                    case TreeState.Tree:
                        return true;
                    default:
                        return false;
                }
            }
        }
        public Tree(Rectangle position, TreeState state, double scale, Level level) : base(position, true, scale, level)
        {
            this.state = state;
            textures = LoadTextures<TreeState>(0);
        }

        public override void Update(GameTime gameTime)
        {
            //implement code here
        }

        public override void Draw(GameTime gameTime, SpriteBatch sb)
        {
            textures[state].Draw(Position, sb, gameTime, SpriteEffects.None);
        }

        /// <summary>
        /// Draws the tree in its current state at given position
        /// </summary>
        /// <param name="position">A rectangle setting the position to draw the tree at</param>
        public override void DrawAt(Rectangle position, GameTime gameTime, SpriteBatch sb)
        {
            textures[state].Draw(position, sb, gameTime, SpriteEffects.None);
        }

        /// <summary>
        /// changes the current state of the tree to its previous state as long as it isn't in its base state
        /// </summary>
        public override void GoToPast()
        {

            switch (state)
            {
                case TreeState.Dirt:
                    break;
                case TreeState.Sapling:
                    state = TreeState.DirtThatWillBeSapling;
                    break;
                case TreeState.Tree:
                    state = TreeState.Sapling;
                    break;
                case TreeState.DeadTree:
                    state = TreeState.Tree;
                    break;
                case TreeState.BagSapling:
                    break;
                case TreeState.ChoppedTree:
                    break;
                case TreeState.Log:
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// changes the current state of the tree to its next state as long as it isn't in its oldest state.
        /// </summary>
        public override void GoToFuture()
        {
            switch (state)
            {
                case TreeState.Dirt:
                    break;
                case TreeState.DirtThatWillBeSapling:
                    Game1.particleManager.AddGenericParticleSystem(new AnimatedTexture(0, 16, 16, Game1.staticTextureManager["LeafParticle"]), position);
                    state = TreeState.Sapling;
                    break;
                case TreeState.Sapling:
                    Game1.particleManager.AddGenericParticleSystem(new AnimatedTexture(0, 16, 16, Game1.staticTextureManager["LeafParticle"]), position);
                    state = TreeState.Tree;
                    break;
                case TreeState.Tree:
                    state = TreeState.DeadTree;
                    break;
                case TreeState.DeadTree:
                    state = TreeState.Dirt;
                    break;
                case TreeState.BagSapling:
                    break;
                case TreeState.ChoppedTree:
                    break;
                case TreeState.Log:
                    break;
                default:
                    break;
            }
        }

        public override void ReactToItemUse(string itemNameAndState)
        {
            switch (itemNameAndState)
            {
                case "Shovel_Shovel":
                    GetDugUp();
                    break;
                case "Axe_Axe":
                    GetChopped();
                    break;
                case "Tree_BagSapling":
                    PlantTree();
                    break;
                default:
                    break;
            }
        }

        public override bool ReactToUsingOn(string itemNameAndState)
        {
            switch (itemNameAndState)
            {
                case "Tree_Dirt":
                    return GetPlanted();
                default:
                    return true;
            }
        }

        public void GetDugUp()
        {
            if(state == TreeState.Sapling)
            {
                Game1.particleManager.AddGenericParticleSystem(new AnimatedTexture(0, 16, 16, Game1.staticTextureManager["DirtParticle"]), position);
                state = TreeState.Dirt;
                currentLevel.AddInteractable(new Tree(new Rectangle(position.X + 64, position.Y, position.Width, position.Height), TreeState.BagSapling, 1, currentLevel));
            }
        }

        private void PlantTree()
        {
            if(state == TreeState.Dirt)
            {
                Game1.particleManager.AddGenericParticleSystem(new AnimatedTexture(0, 16, 16, Game1.staticTextureManager["DirtParticle"]), position);
                state = TreeState.Sapling;
            }
        }

        private bool GetPlanted()
        {
            if(state == TreeState.BagSapling)
            {
                return false;
            } else
            {
                Game1.particleManager.AddGenericParticleSystem(new AnimatedTexture(0, 16, 16, Game1.staticTextureManager["DirtParticle"]), position);
                return true;
            }
        }

        public void GetChopped()
        {
            if (state == TreeState.Tree)
            {
                Game1.particleManager.AddGenericParticleSystem(new AnimatedTexture(0, 16, 16, Game1.staticTextureManager["DirtParticle"]), position);
                state = TreeState.Dirt;
                currentLevel.AddInteractable(new Tree(new Rectangle(position.X + 64, position.Y, position.Width, position.Height), TreeState.ChoppedTree, 1, currentLevel));
            }
            else if (state == TreeState.ChoppedTree)
            {
                Game1.particleManager.AddGenericParticleSystem(new AnimatedTexture(0, 16, 16, Game1.staticTextureManager["LeafParticle"]), position);
                state = TreeState.Log;
            }
        }
    }
}
