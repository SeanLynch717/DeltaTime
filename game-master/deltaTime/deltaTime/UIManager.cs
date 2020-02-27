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
    /// 
    /// </summary>
    class UIManager
    {
        public Dictionary<string,UIButton> buttons;
        private List<UIText> texts;
        private InputManager inputManager;
        private bool timeTravelAnimationActivated;
        private Player player;
        private SpriteBatch spriteBatch;

        /// <summary>
        /// constructor for UIManager
        /// </summary>
        /// <param name="buttons"></param>
        public UIManager(Player player, SpriteBatch spriteBatch)
        {
            buttons = new Dictionary<string, UIButton>();
            texts = new List<UIText>();
            timeTravelAnimationActivated = true;
            this.player = player;
            this.spriteBatch = spriteBatch;
        }
        /// <summary>
        /// Getter for buttons
        /// </summary>
        /// <param name="index"> the index to get</param>
        /// <returns> the button at index</returns>
        public UIButton this[string index]
        {
            get { return buttons[index]; }
        }
        /// <summary>
        /// Setter for input manager.
        /// </summary>
        public InputManager InputManager
        {
            set { inputManager = value; }
        }
        public bool TimeTravelAnimationActivated
        {
            get { return timeTravelAnimationActivated; }
        }
        public void Draw(GameState gameState, GameTime gameTime, SpriteBatch sb)
        {
            //Draw title screen along with necessary buttons when in the menu.
            if(gameState == GameState.Start)
            {
                buttons["logo"].Draw(gameTime, sb);
                buttons["start"].Draw(gameTime, sb);
                buttons["options"].Draw(gameTime, sb);
                buttons["controls"].Draw(gameTime,sb);
            }
            //Draw the pause menu.
            else if(gameState == GameState.PauseMenu)
            {   
                buttons["resume"].Draw(gameTime, sb);
                buttons["options"].Draw(gameTime, sb);
                buttons["logo"].Draw(gameTime, sb);
                buttons["controls"].Draw(gameTime,sb);
            }
            //Draw the options menu.
            else if(gameState == GameState.OptionsMenu)
            {
                texts[0].Draw(gameTime, sb);
                buttons["back"].Draw(gameTime,sb);
                if(inputManager.MouseClick() && buttons["clickedCheckBox"].MouseHover())
                {
                    if (timeTravelAnimationActivated)
                    {
                        timeTravelAnimationActivated = false;
                    }
                    else
                    {
                        timeTravelAnimationActivated = true;
                    }                  
                }
                if (timeTravelAnimationActivated)
                {
                    buttons["clickedCheckBox"].Draw(gameTime, sb);
                }
                else
                {
                    buttons["blankCheckBox"].Draw(gameTime, sb);
                }
                
            }
            //Draw the controls menu
            else if(gameState == GameState.ControlsMenu)
            {
                buttons["controlsMenu"].Draw(gameTime,sb);
                buttons["back"].Draw(gameTime,sb);
            }
            //Draw necessary level UI
            else if(gameState == GameState.End)
            {
                buttons["End"].Draw(gameTime, sb);
            } 
            //Otherwise you're in a level.
            else
            {
                buttons["pause"].Draw(gameTime, sb);
                
                if (player.HeldItem != null)
                {
                    Texture2D texture = Game1.staticTextureManager[player.HeldItem.NameAndState];
                    AnimatedTexture a = new AnimatedTexture(0, 16, 16, texture);
                    a.Draw(new Rectangle(486, 35, 52, 52), spriteBatch, gameTime, SpriteEffects.None);
                }
                Game1.interactableBox.Draw(new Rectangle(464, 0, 96, 96), spriteBatch, gameTime, SpriteEffects.None);
            }
        }

        /// <summary>
        /// Adds a button to the UI Manager
        /// </summary>
        /// <param name="b">The button to add</param>
        public void Add(string key, UIButton value)
        {
            buttons[key] = value;
        }

        /// <summary>
        /// Adds a text to the UI Manager
        /// </summary>
        /// <param name="b">The text to add</param>
        public void Add(UIText t)
        {
            texts.Add(t);
        }
    }
}
