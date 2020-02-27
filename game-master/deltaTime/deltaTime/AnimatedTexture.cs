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
    /// responsible for providing animations
    /// </summary>
    public class AnimatedTexture
    {
        private int fps;
        private double spf;
        private int currentFrame;
        private int widthOfFrame;
        private int heightOfFrame;
        private int totalFrames;
        private Texture2D spriteSheet;
        private double timer;
        /// <summary>
        /// getter and setter for fps
        /// </summary>
        public int Fps
        {
            get { return fps; }
            set { fps = value; spf = 1.0 / fps; }
        }
        /// <summary>
        /// getter and setter for spf
        /// </summary>
        public double Spf
        {
            get;//implement code here
            set;
        }
        /// <summary>
        /// getter and setter for currentFrame
        /// </summary>
        public int CurrentFrame
        {
            get;//implement code here
            set;
        }
        /// <summary>
        /// getter and setter for spriteSheet
        /// </summary>
        public Texture2D SpriteSheet
        {
            get
            {
                return spriteSheet;
            }
            set
            {
                spriteSheet = value;
            }
        }
        /// <summary>
        /// constructor for AnimatedTexture
        /// </summary>
        /// <param name="fps"> the number of frames that are cycle through every second</param>
        /// <param name="spf"> how long it takes to cycle through one frame</param>
        /// <param name="currentFrame"> the current frame of animation</param>
        /// <param name="spriteSheet"> the spritesheet</param>
        public AnimatedTexture(int fps, int widthOfFrame, int heightOfFrame, Texture2D spriteSheet)
        {
            this.fps = fps;
            this.spf = 1.0 / fps;
            this.currentFrame = 0;
            this.spriteSheet = spriteSheet;
            this.widthOfFrame = widthOfFrame;
            this.heightOfFrame = heightOfFrame;
            this.totalFrames = spriteSheet.Width / widthOfFrame;
        }
        /// <summary>
        /// Draws the animation at the appropriate frame.
        /// </summary>
        /// <param name="position"> the position to draw at</param>
        /// <param name="spriteBatch"> the spriteBatch</param>
        /// <param name="gameTime"> the GameTime</param>
        /// <param name="flip"> whether or not to flip the sprite horizontally or vetically or none</param>
        public void Draw(Rectangle position, SpriteBatch spriteBatch, GameTime gameTime, SpriteEffects flip)
        {
            timer += gameTime.ElapsedGameTime.TotalSeconds;
            if(timer > spf)
            {
                timer -= spf;
                currentFrame++;
                if(currentFrame >= totalFrames)
                {
                    currentFrame = 0;
                }
            }
            spriteBatch.Draw(
            spriteSheet,
            position,
            new Rectangle(widthOfFrame * currentFrame, 0, widthOfFrame, heightOfFrame),
            Color.White,
            0.0f,
            Vector2.Zero,
            flip,
            0.0f);
        }
        /// <summary>
        /// Overloaded Draw method that takes in a color and a transparency. Primarily used for the onion skin when time travel fails.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="spriteBatch"></param>
        /// <param name="gameTime"></param>
        /// <param name="flip"></param>
        /// <param name="color"></param>
        public void Draw(Rectangle position, SpriteBatch spriteBatch, GameTime gameTime, SpriteEffects flip, float transparency, Color color)
        {
            timer += gameTime.ElapsedGameTime.TotalSeconds;
            if (timer > spf)
            {
                timer -= spf;
                currentFrame++;
                if (currentFrame >= totalFrames)
                {
                    currentFrame = 0;
                }
            }
            spriteBatch.Draw(
            spriteSheet,
            position,
            new Rectangle(widthOfFrame * currentFrame, 0, widthOfFrame, heightOfFrame),
            color * transparency,
            0.0f,
            Vector2.Zero,
            flip,
            0.0f);
        }

    }
}
