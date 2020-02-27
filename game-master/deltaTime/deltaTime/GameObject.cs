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
    /// A base class that holds basic information about many types of objects
    /// </summary>
    public abstract class GameObject
    {
        protected bool collidable;
        protected Rectangle position;
        protected bool flipX;
        protected bool flipY;
        /// <summary>
        /// gets the position of the rectangle
        /// </summary>
        public Rectangle Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }
        /// <summary>
        /// getter and setter for the x position
        /// </summary>
        public int X
        {
            get { return position.X; }
            set { position = new Rectangle(value, position.Y, position.Width, position.Height); }
        }
        /// <summary>
        /// getter and setter for the y position
        /// </summary>
        public int Y
        {
            get { return position.Y; }
            set { position = new Rectangle(position.X, value, position.Width, position.Height); }
        }
        /// <summary>
        /// getter and setter for flipX
        /// </summary>
        public bool FlipX
        {
            get { return flipX; }
            set { flipX = value; }
        }
        /// <summary>
        /// getter and setter for flipY
        /// </summary>
        public bool FlipY
        {
            get { return flipY; }
            set { flipY = value; }
        }
        /// <summary>
        /// Sets whether the gameObject is collidable or not
        /// </summary>
        public bool Collidable {
            set
            {
                collidable = value;
            }
        }

        /// <summary>
        /// constuctor for GameObject
        /// </summary>
        /// <param name="position"> the position </param>
        /// <param name="collidable"> whether or not the object is collidable</param>
        public GameObject(Rectangle position, bool collidable, double scale)
        {
            this.position = new Rectangle(position.X, position.Y, (int)(position.Width * scale), (int)(position.Width * scale));
            this.collidable = collidable;
        }

        /// <summary>
        /// to be implemented in child class
        /// </summary>
        public abstract void Update(GameTime gameTime);

        /// <summary>
        /// to be implemented in child class
        /// </summary>
        public abstract void Draw(GameTime gameTime, SpriteBatch sb);

    }
}
