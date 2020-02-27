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
    /// this class holds a running list of all of the levels and had the ability to display them.
    /// </summary>
    public class LevelManager
    {
        private List<Level> levels;
        private int currentLevel;
        private Player player;
        /// <summary>
        /// Indexer property for level. Includes a getter and setter.
        /// </summary>
        /// <param name="index"> the index</padram>
        /// <returns> the specified level</returns>
        public Level this[int index]
        {
            get { return levels[index]; }
            set { levels[index] = value; }
        }
        public int CurrentLevel
        {
            get{return currentLevel;}
            set{currentLevel = value;}
        }

        /// <summary>
        /// Adds all levels in the game
        /// </summary>
        /// <param name="tileTextures">List of potential tile textures</param>
        public LevelManager(Dictionary<String, Texture2D> tileTextures, Player p)
        {
            levels = new List<Level>();
            currentLevel = 0;
            player = p;
            player.LevelManager = this;
            levels.Add(new Level("Content/Level1.txt", tileTextures));
            levels.Add(new Level("Content/Level2.txt", tileTextures));
            levels.Add(new Level("Content/Level5.txt", tileTextures));
            levels.Add(new Level("Content/Level6.txt", tileTextures));
            levels.Add(new Level("Content/Level7.txt", tileTextures));
            levels.Add(new Level("Content/Level8.txt", tileTextures));
        }
        /// <summary>
        /// draws the current level.
        /// </summary>
        public void DrawLevel(GameTime gameTime, SpriteBatch sb)
        {
            //draws whatever current level is
            levels[currentLevel].Draw(gameTime, sb);
        }

        //creates easy accessor for the level we're currently on
        //This changes automatically when currentLevel changes
        //Currently used by collisionManager
        public Level ThisLevel
        {
            get
            {
                return levels[currentLevel];
            }
        }

        /// <summary>
        /// Amount of levels in the list
        /// </summary>
        public int MaxLevel
        {
            get
            {
                return levels.Count;
            }
        }

        /// <summary>
        /// remove a level from the level manager
        /// </summary>
        /// <param name="index"> the index to remove at</param>
        public void DropLevel(int index)
        {
            //implement code here
        }

        ///// <summary>
        ///// this method is data driven. Will read in a file, interpret it, and add it to the list of levels.
        ///// </summary>
        //public void ReadInLevel()
        //{
        //    //implement code here
        //}

        /// <summary>
        /// Switch the time state.
        /// </summary>
        public void TimeStateChange()
        {
            if (Game1.timeState == TimeState.Past)
            {
                ThisLevel.GoToFuture();
                Game1.timeState = TimeState.Future;
            }
            else
            {
                ThisLevel.GoToPast();
                Game1.timeState = TimeState.Past;
            }
        }

        public void NextLevel()
        {
            currentLevel++;
            
            player.X = 100;
            player.Y = 300;
        }
    }
}
