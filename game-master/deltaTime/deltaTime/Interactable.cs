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
    /// A <c>GameObject</c> that has different states that can change with time or player interaction
    /// </summary>
    public abstract class Interactable : GameObject
    {
        /// <summary>
        /// The level the interactable is in
        /// </summary>
        protected Level currentLevel;

        /// <summary>
        /// Gets the pickupability of the interactable - this is dependent on state
        /// </summary>
        public abstract bool Pickupable
        {
            get;
        }

        /// <summary>
        /// The name and state
        /// </summary>
        public abstract string NameAndState
        {
            get;
        }
        /// <summary>
        /// Gets the coordinates of the interactable relative to the grid
        /// </summary>
        public Vector2 GridCoords {
            get
            {
                return new Vector2(position.X / 64, position.Y / 64);
            }
        }
        /// <summary>
        /// Constructor for Interactable
        /// </summary>
        /// <param name="position">The position the Interactable starts at</param>
        /// <param name="Collidable">Whether or not the Interactable is collidable</param>
        public Interactable(Rectangle position, bool collidable, double scale, Level level):base(position, collidable, scale)
        {
            currentLevel = level;
        }

        /// <summary>
        /// Draws the interactable at a given position
        /// </summary>
        /// <param name="position">The position to draw it at</param>
        public abstract void DrawAt(Rectangle position, GameTime gameTime, SpriteBatch sb);

        /// <summary>
        /// Performs whatever actions happen when the interactable goes to the past
        /// To be implemented in child classes
        /// </summary>
        public abstract void GoToPast();

        /// <summary>
        /// Performs whatever actions happen when the interactable goes to the future
        /// To be implemented in child classes
        /// </summary>
        public abstract void GoToFuture();


        /// <summary>
        /// Loads all the textures for the Interactable
        /// </summary>
        /// <typeparam name="StateEnum">
        /// The Enum storing the different states of the interactable
        /// e.g. ChickenState for Chicken
        /// </typeparam>
        /// <param name="fps">The fps the animation should go at</param>
        /// <returns>
        /// A dictionary mapping from StateEnum to AnimatedTexture of all the
        /// animations for the interactable
        /// </returns>
        protected Dictionary<StateEnum, AnimatedTexture> LoadTextures<StateEnum>(int fps)
        {
            Dictionary<StateEnum, AnimatedTexture>  textures = new Dictionary<StateEnum, AnimatedTexture>();
            string typeName = typeof(StateEnum).ToString();
            typeName = typeName.Substring(10, typeName.Length - 15);
            foreach (StateEnum state in Enum.GetValues(typeof(StateEnum)))
            {
                
                textures[state] = new AnimatedTexture(fps, 16, 16, Game1.staticTextureManager[typeName + "_" + state.ToString()]);
            }

            return textures;
        }

        /// <summary>
        /// Reacts to the use of an item with given name and state on it.
        /// This method should take the form of a large switch statement taking 
        /// a concatenation of <c>itemName</c> and <c>itemState</c> containing 
        /// whatever code is wanted for that name and state
        /// </summary>
        /// <param name="itemNameAndState">The name and state of the item used on it, as a string separated by an underscore</param>
        public abstract void ReactToItemUse(string itemNameAndState);

        /// <summary>
        /// Reacts to this item being used on another item with given name and state.
        /// This method should take the form of a large switch statement taking
        /// a concatenation of <c>itemName</c> and <c>itemState</c> containing 
        /// whatever code is wanted for that name and state
        /// </summary>
        /// <returns>true if the item should remain in the player's hands afterward</returns>
        public abstract bool ReactToUsingOn(string itemNameAndState);
    }
}
