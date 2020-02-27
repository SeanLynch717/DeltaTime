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
    /// class to display buttons to the screen
    /// </summary>
    class UIButton
    {
        private Texture2D notPressed;
        private Texture2D hovered;
        private Rectangle position;
        private Texture2D current;
        public Texture2D Current{
            get{return current;}
            set{current = value;}
        }
        public Texture2D NotPressed{
            get{return notPressed;}
        }
        public Texture2D Hovered{
            get{return hovered;}
        }
        /// <summary>
        /// constructor for Button
        /// </summary>
        /// <param name="notPressed">The texture for the button at its unpressed state</param>
        /// <param name="hovered">The texture for the button at its hovered state</param>
        /// <param name="pressed">The texture for the button at its pressed state</param>
        /// <param name="position">The position of the button</param>
        public UIButton(Texture2D notPressed, Texture2D hovered, Rectangle position)
        {
            this.notPressed = notPressed;
            this.hovered = hovered;
            this.position = position;
            current = notPressed;
        }

        public bool Clicked()
        {
            MouseState state = Mouse.GetState();
            Point mousePos = new Point(state.X, state.Y);
            return (position.Contains(mousePos) && state.LeftButton == ButtonState.Pressed);
        }
        public bool MouseHover()
        {
            MouseState state = Mouse.GetState();
            Point mousePos = new Point(state.X, state.Y);
            return position.Contains(mousePos);
        }
        /// <summary>
        /// draws the button
        /// </summary>
        /// <param name="gameTime"> the GameTime</param>
        public void Draw(GameTime gameTime, SpriteBatch sb)
        {
            sb.Draw(current, position, Color.White);
        }
    }
}
