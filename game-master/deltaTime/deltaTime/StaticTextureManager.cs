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
    public class StaticTextureManager
    {
        private Dictionary<string, Texture2D> textures;

        /// <summary>
        /// Gets or sets the static texture corresponding to the given name
        /// </summary>
        /// <param name="name">The name of the texture wanted</param>
        /// <returns>That texture</returns>
        public Texture2D this[string name]
        {
            get
            {
                if(textures.Keys.Contains(name))
                {
                    return textures[name];
                }
                return null;
            }
            set
            {
                textures[name] = value;
            }
        }

        public StaticTextureManager()
        {
            textures = new Dictionary<string, Texture2D>();
        }
    }
}
