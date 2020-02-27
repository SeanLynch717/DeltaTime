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
    /// this class deals with all of the collisions within the game
    /// </summary>
    class CollisionManager
    {
        private Player player;
        private LevelManager levelManager;
        private Tile[,] levelArray;
        int screenWidth;
        int screenHeight;
        //This method 
        public CollisionManager(Player p, LevelManager lm)
        {
            player = p;
            levelManager = lm;
            levelArray = levelManager.ThisLevel.Tiles;
            screenWidth = 1024;
            screenHeight = 768;
            //implement code here
        }
        //detects collisions between the player and the objects in the level.
        public void Update()
        {

            //Checking to seee if the player is out of bounds, and then change the levels ----
            if ((player.Position.X < 0 || player.Position.X > screenWidth || player.Position.Y < 0 || player.Position.Y > screenHeight ) && !(player.InTimeTravelAnim))
            {
                levelManager.NextLevel();
                levelArray = levelManager.ThisLevel.Tiles;
            }
            //If player is inside a wall, move him out in the right direction. 
            //Make a new rect, make its x the players x and its y the players y + 32
            //If tile bool is past collidable, do diffrent things
            foreach (Tile tile in levelArray)
            {
                //Point playerSpot = new Point(player.Position.X / 64, player.Position.Y / 64);
                //Checking to see if the tiles are collidable in the future or past
                if (Game1.timeState == TimeState.Future && tile.FutureCollidable == false)
                {
                    continue;
                }
                if (Game1.timeState == TimeState.Past && tile.PastCollidable == false)
                {
                    continue;
                }

                //---------------------------------------------------------------------------------------------------------------------------
                Rectangle intersectingRect;
                //Acount for the fact that the collision is off on the left side.
                if (player.PlayerState == PlayerState.FacingLeft || player.PlayerState == PlayerState.WalkingLeft)
                {
                    intersectingRect = Rectangle.Intersect(new Rectangle(player.Position.X+25, player.Position.Y + 38, 60, 37), tile.Position);
                }
                else
                {
                    intersectingRect = Rectangle.Intersect(new Rectangle(player.Position.X, player.Position.Y + 38, 60, 37), tile.Position);
                }

                if (intersectingRect != Rectangle.Empty && !player.InTimeTravelAnim)
                {
                    //if (tile.IsDoor && !player.InTimeTravelAnim)
                    //{
                    //    levelManager.NextLevel();
                    //    levelArray = levelManager.ThisLevel.Tiles;
                    //}

                    if (intersectingRect.Width > intersectingRect.Height)
                    {
                        //Move player up or down
                        if (tile.Y < player.Y)
                        {
                            //above, move player down
                            player.Y += player.Speed + 1;
                        }
                        else
                        {
                            //below, move player up
                            player.Y -= player.Speed + 1;

                        }
                    }
                    else
                    {
                        //Move player left or right
                        if (tile.X < player.X)
                        {
                            //Left, move player right
                            player.X += player.Speed + 1;
                        }
                        else
                        {
                            //right, move player left
                            player.X -= player.Speed + 1;

                        }
                    }

                }
            }
        }
        /// <summary>
        /// Checks whether or not the player will collide with a wall in the other time state.
        /// </summary>
        /// <returns><c>true</c> if the player will collide with a wall in the non-present time state, <c>false</c> otherwise</returns>
        public bool PlayerIsCollidingInOtherTime()
        {
            int centerY = player.Y / 64;
            int centerX = player.X / 64;
            for (int row = centerY - 1; row <= centerY + 1; row++)
            {
                for (int col = centerX - 1; col <= centerX + 1; col++)
                {
                    if (row >= 0 && col >= 0)
                    {
                        if (Game1.timeState == TimeState.Past && levelArray[row, col].FutureCollidable && levelArray[row, col].Position.Intersects(new Rectangle(player.Position.X, player.Position.Y + 38, 60, 37))) //player.Position.Intersects(levelArray[row, col].Position))
                        {
                            return true;
                        }
                        else if (Game1.timeState == TimeState.Future && levelArray[row, col].PastCollidable && levelArray[row, col].Position.Intersects(new Rectangle(player.Position.X, player.Position.Y + 38, 60, 37)))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}
