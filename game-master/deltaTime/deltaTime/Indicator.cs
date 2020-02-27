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
    public class Indicator : GameObject
    {
        private Texture2D texture;
        private Color color;
        private double lifetimeLeft;
        /// <summary>
        /// Creates an indicator under given GameObject
        /// </summary>
        /// <param name="target">The gameObject to create the indicator under</param>
        /// <param name="color">The color of the indicator</param>
        /// <param name="lifespan">How long the indicator should last before it disappears</param>
        public Indicator(GameObject target, Color color, double lifespan) : base(target.Position, false, 1)
        {
            texture = Game1.staticTextureManager["indicator"];
            this.color = color;
            lifetimeLeft = lifespan;
        }

        /// <summary>
        /// Gets the amount of time the indicator has left to exist
        /// </summary>
        public double LifetimeLeft
        {
            get
            {
                return lifetimeLeft;
            }
        }
        public override void Update(GameTime gameTime)
        {
            lifetimeLeft -= gameTime.ElapsedGameTime.TotalSeconds;
        }

        public override void Draw(GameTime gameTime, SpriteBatch sb)
        {
            sb.Draw(texture, position, color);
        }
    }
}
