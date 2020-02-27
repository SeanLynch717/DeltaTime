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
    class UIText
    {
        private SpriteFont font;
        private string text;
        private Vector2 position;

        public string Text {
            set
            {
                text = value;
            }
        }

        public UIText(SpriteFont font, string text, Vector2 position)
        {
            this.font = font;
            this.text = text;
            this.position = position;
        }

        public void Draw(GameTime gameTime, SpriteBatch sb)
        {
            sb.DrawString(font, text, position, Color.White);
        }
    }
}
