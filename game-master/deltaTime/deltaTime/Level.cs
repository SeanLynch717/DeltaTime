using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace deltaTime
{
    /// <summary>
    /// A level, holding a two-dimensional array of tiles and a list of interactables in the level
    /// </summary>
    public class Level
    {
        private Tile[,] tiles;
        private List<Interactable> interactables;
        private List<Indicator> indicators;
        private List<Recepticle> recepticles;
        private List<Point> doors;
        public Random myRNG;
        private int tileVariant;
        //private bool isFuture;

        /// <summary>
        /// Creates a level given a file name and a dictionary of tile textures to use
        /// </summary>
        public Level(String fileName, Dictionary<String, Texture2D> tileTextures)
        {
            myRNG = new Random();
            tileVariant = 0;
            interactables = new List<Interactable>();
            recepticles = new List<Recepticle>();
            indicators = new List<Indicator>();
            doors = new List<Point>();
            tiles = new Tile[12, 16];
            //finds file, creates holder string for reader
            StreamReader reader = new StreamReader(fileName);
            String line = "";
            String[] words;

            //for every tile in the 2D array:\
            //checks if this tile is a floor tile is past and/or future
            //if floor, make it non-collidable in that timeState
            //otherwise, it is collidable
            for (int h = 0; h < 12; h++)
            {
                for (int w = 0; w < 16; w++)
                {
                    line = reader.ReadLine();
                    //ignore documented lines in Level files
                    if (line[0] != '/')
                    {
                        tileVariant = myRNG.Next(20) + 1;

                        if (tileVariant == 17)
                            tileVariant = 1;
                        else if (tileVariant == 18)
                            tileVariant = 2;
                        else if (tileVariant == 19)
                            tileVariant = 3;
                        else if (tileVariant == 20)
                            tileVariant = 4;
                        else
                            tileVariant = 0;

                            words = line.Split(' ');

                        //handles situations where both, one, or neither time state is collidable
                        //HEY YOURE GONNA HAVE TO FIX THIS LATER
                        //^^this is bad documentation
                        //I think I fixed this, because this only generates an error with out-of-date level files
                        if (words[0] == "door" || words[0] == "OpenDoorExtHoriz" || words[0] == "OpenDoorExtVert" || words[0] == "OpenDoorIntVert" || words[0] == "OpenDoorIntVert"
                            || words[2] == "door" || words[2] == "OpenDoorExtHoriz" || words[2] == "OpenDoorExtVert" || words[2] == "OpenDoorIntVert" || words[2] == "OpenDoorIntVert")
                        {
                            tiles[h, w] = new Door(
                               new Rectangle((w * 64), (h * 64), 64, 64),
                               new AnimatedTexture(0, 32, 32, tileTextures["Past" + words[0]]), int.Parse(words[1]), false,
                               new AnimatedTexture(0, 32, 32, tileTextures["Future" + words[2] + tileVariant]), int.Parse(words[3]), false, 1);
                            doors.Add(new Point(h, w));
                        }
                        else if (words[0] == "LockedDoorExtHoriz" || words[0] == "LockedDoorExtVert" || words[0] == "LockedDoorIntVert" || words[0] == "LockedDoorIntVert"
                            || words[2] == "LockedDoorExtHoriz" || words[2] == "LockedDoorExtVert" || words[2] == "LockedDoorIntVert" || words[2] == "LockedDoorIntVert")
                        {
                            tiles[h, w] = new Door(
                               new Rectangle((w * 64), (h * 64), 64, 64),
                               new AnimatedTexture(0, 32, 32, tileTextures["Past" + words[0]]), int.Parse(words[1]), true,
                               new AnimatedTexture(0, 32, 32, tileTextures["Future" + words[2] + tileVariant]), int.Parse(words[3]), true, 1);
                            doors.Add(new Point(h, w));
                        }
                        else if (words[0] == "Floor" && words[2] == "Floor")
                        {
                            tiles[h, w] = new Tile(
                                new Rectangle((w * 64), (h * 64), 64, 64),
                                new AnimatedTexture(0, 32, 32, tileTextures["Past" + words[0]]), int.Parse(words[1]), false,
                                new AnimatedTexture(0, 32, 32, tileTextures["Future" + words[2] + tileVariant]), int.Parse(words[3]), false, 1);
                        }
                        else if (words[0] == "Floor" && words[2] != "Floor")
                        {
                            tiles[h, w] = new Tile( 
                                new Rectangle((w * 64), (h * 64), 64, 64),
                                new AnimatedTexture(0, 32, 32, tileTextures["Past" + words[0]]), int.Parse(words[1]), false,
                                new AnimatedTexture(0, 32, 32, tileTextures["Future" + words[2] + tileVariant]), int.Parse(words[3]), true, 1);
                        }
                        else if (words[0] != "Floor" && words[2] == "Floor")
                        {
                            tiles[h, w] = new Tile(
                                new Rectangle((w * 64), (h * 64), 64, 64),
                                new AnimatedTexture(0, 32, 32, tileTextures["Past" + words[0]]), int.Parse(words[1]), true,
                                new AnimatedTexture(0, 32, 32, tileTextures["Future" + words[2] + tileVariant]), int.Parse(words[3]), false, 1);
                        }
                        //HEY YOURE GONNA HAVE TO FIX THIS LATER
                        //see above document
                        else if (words[0] != "Floor" && words[2] != "Floor")
                        {
                            tiles[h, w] = new Tile(
                                new Rectangle((w * 64), (h * 64), 64, 64),
                                new AnimatedTexture(0, 32, 32, tileTextures["Past" + words[0]]), int.Parse(words[1]), true,
                                new AnimatedTexture(0, 32, 32, tileTextures["Future" + words[2] + tileVariant]), int.Parse(words[3]), true, 1);
                        }
                    }
                    //this part of if ladder just makes sure that skipping lines due to documentation in the level files
                    //doesn't skip a tile
                    else if (w == 0)
                    {
                        w = 15;
                        h--;
                    }
                    else
                        w--;
                }
            }

            //begin interactable stuff
            while(true)
            {
                line = reader.ReadLine();
                if(line == null || line == "/recepticles")
                {
                    break;
                }
                words = line.Split(' ');
                if(words.Length < 4)
                {
                    continue;
                }
                string interactableName = words[0];
                string interactableState = words[1];
                int gridX = int.Parse(words[2]);
                int gridY = int.Parse(words[3]);
                Rectangle r = new Rectangle(gridX * 64, gridY * 64, 64, 64);
                switch(interactableName)
                {
                    case "Chicken":
                        interactables.Add(new Chicken(r, (ChickenState)Enum.Parse(typeof(ChickenState), interactableState), 1, this));
                        break;
                    case "Flower":
                        interactables.Add(new Flower(r, (FlowerState)Enum.Parse(typeof(FlowerState), interactableState), 1, this));
                        break;
                    case "Tree":
                        interactables.Add(new Tree(r, (TreeState)Enum.Parse(typeof(TreeState), interactableState), 1, this));
                        break;
                    case "WateringCan":
                        interactables.Add(new WateringCan(r, (WateringCanState)Enum.Parse(typeof(WateringCanState), interactableState), 1, this));
                        break;
                    case "Shovel":
                        interactables.Add(new Shovel(r, (ShovelState)Enum.Parse(typeof(ShovelState), interactableState), 1, this));
                        break;
                    case "Axe": 
                        Interactables.Add(new Axe(r, (AxeState)Enum.Parse(typeof(AxeState), interactableState), 1, this));
                        break;
                }
            }
            
            //skip documentation line in level file
            line = reader.ReadLine();

            //handles recepticles
            while (line != null)
            {
                //to store coordinates of the door
                String[] doorCoord;
                //a temporary reference to a door that needs to be linked to this recepticle
                Tile door;
                //a temporary reference to this recepticle
                Recepticle r;
                
                words = line.Split(' ');

                if(words[0] == "Past")
                {
                    r = new Recepticle(
                            TimeState.Past,
                            (words[1] + "_" + words[2]),
                            new Rectangle(int.Parse(words[4]) * 64, int.Parse(words[3]) * 64, 64, 64));

                    //loops through remainder of words
                    //which are only coordinates for doors to link to the recepticles
                    for (int i = 5; i < words.Length; i++)
                    {
                        doorCoord = words[i].Split(',');
                        //find that door
                        door = tiles[int.Parse(doorCoord[1]), int.Parse(doorCoord[0])];
                        ((Door)(door)).LinkRecepticle(r);
                    }
                    recepticles.Add(r);
                }
                else if(words[0] == "Future")
                {
                    r = new Recepticle(
                            TimeState.Future,
                            (words[1] + "_" + words[2]),
                            new Rectangle(int.Parse(words[4]) * 64, int.Parse(words[3]) * 64, 64, 64));

                    //loops through remainder of words
                    //which are only coordinates for doors to link to the recepticles
                    for (int i = 5; i < words.Length; i++)
                    {
                        doorCoord = words[i].Split(',');
                        //find that door
                        door = tiles[int.Parse(doorCoord[1]), int.Parse(doorCoord[0])];
                        ((Door)(door)).LinkRecepticle(r);
                    }
                    recepticles.Add(r);
                }

                //go to next line
                line = reader.ReadLine();
            }
        }

        //way to access 2D array within level
        //I think this is also only used bt CollisionManager
        public Tile[,] Tiles
        {
            get
            {
                return tiles;
            }
        }

        public List<Interactable> Interactables
        {

            get
            {
                return interactables;
            }

        }

        public List<Indicator> Indicators
        {
            get
            {
                return indicators;
            }
        }
        public List<Recepticle> Recepticles
        {
            get { return recepticles; }
        }
        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < indicators.Count; i++)
            {
                indicators[i].Update(gameTime);
                if(indicators[i].LifetimeLeft < 0)
                {
                    indicators.Remove(indicators[i]);
                    i--;
                }
            }
            
            foreach(Point p in doors)
            {
                tiles[p.X, p.Y].Update(gameTime);
            }
        }
        /// <summary>
        /// Draws all tiles and interactables in the level
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="sb">The spritebatch to draw with</param>
        public void Draw(GameTime gameTime, SpriteBatch sb)
        {
            //Draw all of the current levels tiles
            for (int h = 0; h < 12; h++)
            {
                for (int w = 0; w < 16; w++)
                {
                    tiles[h, w].Draw(gameTime, sb);
                }
            }
            //Draw all of the recepticles
            foreach(Recepticle r in recepticles)
            {
                r.Draw(gameTime, sb);
            }
            //Draw all of the interactables
            foreach (Interactable i in interactables)
            {
                i.Draw(gameTime, sb);
            }
            List<Rectangle> positionsOfIndicators = new List<Rectangle>();
            //Draw all of the indicators
            foreach (Indicator i in indicators)
            {
                //i.Draw(gameTime, sb);
                positionsOfIndicators.Add(i.Position);
            }
            //If there are indicators, show and onion skin of the elements preventing the player to time travel
            if(indicators.Count > 0)
            {
                //Go through each indicator in the list of indicators and draw the surrounding pieces of the other timestate.
                foreach(Rectangle r in positionsOfIndicators)
                {
                    for(int row = r.Y/64-1; row < r.Y/64 +2; row++)
                    {
                        for(int col = r.X/64-1; col < r.X/64 + 2; col++)
                        {
                            //If this tile is in a valid position.
                            if(row>=0 && row<=11 && col>=0 && col<16)
                                //If it is collidable in the other time state, then draw it.
                                if((Game1.timeState == TimeState.Past && tiles[row,col].FutureCollidable) ||
                                    (Game1.timeState == TimeState.Future && tiles[row, col].PastCollidable))
                                    tiles[row, col].DrawOtherTimeState(gameTime, sb);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Adds an interactable to the level
        /// </summary>
        /// <param name="i">The interactable to add</param>
        public void AddInteractable(Interactable i)
        {
            Rectangle originalPosition = i.Position;
            while(!IsEmpty(i.GridCoords, TimeState.Past) || !IsEmpty(i.GridCoords, TimeState.Future) || InteractableAt(i.GridCoords) != null)
            {
                i.Position = new Rectangle(i.Position.X + 64, i.Position.Y, i.Position.Width, i.Position.Height);
                if(i.GridCoords.X >= 16)
                {
                    i.Position = originalPosition;
                    i.Position = new Rectangle(i.Position.X, i.Position.Y - 64, i.Position.Width, i.Position.Height);
                    originalPosition = i.Position;
                }
            }
            interactables.Add(i);
            
            
        }
        /// <summary>
        /// Removes an interactable from the level
        /// </summary>
        /// <param name="i">The interactable to remove</param>
        public void RemoveInteractable(Interactable i)
        {
            interactables.Remove(i);
        }

        /// <summary>
        /// Adds a recepticle to the level
        /// </summary>
        /// <param name="r">The recepticle to add</param>
        public void AddRecepticle(Recepticle r)
        {
            recepticles.Add(r);
        }

        /// <summary>
        /// Brings the level and all interactables in it to the past
        /// </summary>
        public void GoToPast()
        {
            foreach(Interactable i in interactables)
            {
                i.GoToPast();
            }
        }

        /// <summary>
        /// Brings the level and all interactables in it to the future
        /// </summary>
        public void GoToFuture()
        {
            foreach (Interactable i in interactables)
            {
                i.GoToFuture();
            }
        }

        /// <summary>
        /// Checks whether the given point (in grid space) is empty
        /// </summary>
        /// <param name="x">The X position to check (in grid space)</param>
        /// <param name="y">The Y position to check (in grid space)</param>
        /// <returns><c>true</c> if the position has no collidable tile or interactable in it, <c>false</c> otherwise</returns>
        public bool IsEmpty(int x, int y, TimeState timeState)
        {
            bool empty = true;
            //I'm going to guess that we dont want the player to be able to place the interactables somewhere that will be a wall in the next time state.
            if (timeState == TimeState.Past)
            {
                if (tiles[y,x].PastCollidable)
                {
                    empty = false;
                }
            }
            else
            {
                if (tiles[y,x].FutureCollidable)
                {
                    empty = false;
                }
            }
            
            /*foreach(Interactable i in interactables)
            {
                if(i.GridCoords == new Vector2(x,y))
                {
                    empty = false;
                }
            }*/
            

            return empty;
        }

        public bool IsEmpty(Vector2 gridCoords, TimeState timeState) {
            return IsEmpty((int)gridCoords.X, (int)gridCoords.Y, timeState);
        }

        public List<Interactable> InteractablesCollidingInOtherTime()
        {
            List<Interactable> collidingInteractables = new List<Interactable>();
            foreach(Interactable i in interactables)
            {
                if (!IsEmpty((int)i.GridCoords.X, (int)i.GridCoords.Y, //if there is a tile there
                    Game1.timeState == TimeState.Past ? TimeState.Future : TimeState.Past))
                {
                    collidingInteractables.Add(i);
                }
            }
            return collidingInteractables;
        }
        /// <summary>
        /// Gets the interactable at given position x,y in grid space
        /// </summary>
        /// <param name="x">The x position to get the interactable at</param>
        /// <param name="y">The y position to get the interactable at</param>
        /// <returns>The interactable at that position, if it exists, otherwise <c>null</c></returns>
        public Interactable InteractableAt(int x, int y)
        {
            foreach(Interactable i in interactables)
            {
                if (i.GridCoords == new Vector2(x, y))
                {
                    return i;
                }
            }
            return null;
        }

        public Interactable InteractableAt(Vector2 v)
        {
            return InteractableAt((int)v.X, (int)v.Y);
        }

        /// <summary>
        /// Gets the recepticle at given position x,y in grid space
        /// </summary>
        /// <param name="x">The x position to get the recepticle at</param>
        /// <param name="y">The y position to get the recepticle at</param>
        /// <returns>The recepticle at that position, if it exists, otherwise <c>null</c></returns>
        public Recepticle RecepticleAt(int x, int y)
        {
            foreach(Recepticle r in recepticles)
            {
                if(r.Exists && r.GridCoords == new Vector2(x, y))
                {
                    return r;
                }
            }
            return null;
        }

        public Recepticle RecepticleAt(Vector2 v)
        {
            return RecepticleAt((int)v.X, (int)v.Y);
        }
    }
}
