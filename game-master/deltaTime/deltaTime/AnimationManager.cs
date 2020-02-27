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
    class AnimationManager
    {
        private LevelManager levelManager;
        private Player player;
        private UIManager uiManager;
        private CollisionManager colManager;
        private AnimatedTexture timeTravelAnim;
        private double length;
        private double spiralAngle;
        private const double amplitude = 200;
        private double currentTimeOfShearScreenAnim;
        private const float timeOfShearScreenAnim = 1f;
        private const double timeOffSet = .03;
        private bool switchedState;
        private float transparency;
        private float dTrandparency;
        private SpriteBatch spriteBatch;
        private Rectangle sizeOfClock;
        private int spinSpeed;
        private ParticleManager partManager;
        private Texture2D particleTexture;
        private AnimatedTexture backgroundClock;
        private Rectangle backgroundClockPosition;
        private Random rng;
        private double ellapsedTime;
        private List<Rectangle> clockPositions;
        private List<Vector2> clockVectors2;
        
        
        private Rectangle playerPosBeforeTimeTravel;
        private Dictionary<Interactable, Rectangle> interactablesPosBeforeTimeTravel;
        private Dictionary<Recepticle, Rectangle> recepticlesPosBeforeTimeTravel;

        /// <summary>
        /// Contructs an AnimationManager object.
        /// </summary>
        /// <param name="lm"> The LevelManager.</param>
        /// <param name="p"> The Player.</param>
        /// <param name="uiManager"> The UIManager.</param>
        /// <param name="inputManager"> The InputManager</param>
        /// <param name="timeTravelAnim"> The time travel animation. </param>
        /// <param name="colManager"> The CollisionManager.</param>
        public AnimationManager(LevelManager lm, Player p, UIManager uiManager, AnimatedTexture timeTravelAnim, CollisionManager colManager, SpriteBatch spriteBatch, ParticleManager partManager, Texture2D particleTexture)
        {
            this.levelManager = lm;
            this.player = p;
            this.uiManager = uiManager;
            this.timeTravelAnim = timeTravelAnim;
            this.colManager = colManager;
            interactablesPosBeforeTimeTravel = new Dictionary<Interactable, Rectangle>();
            recepticlesPosBeforeTimeTravel = new Dictionary<Recepticle, Rectangle>();
            spiralAngle = 1;
            length = 300;
            currentTimeOfShearScreenAnim = 0;
            switchedState = false;
            transparency = 1;
            dTrandparency = -.1f;
            this.spriteBatch = spriteBatch;
            sizeOfClock = new Rectangle(448, 704, 128, 128);
            spinSpeed = 2;
            this.partManager = partManager;
            this.particleTexture = particleTexture;
            rng = new Random();
            backgroundClockPosition = new Rectangle(0, rng.Next(0, 300), 512,512);
            backgroundClock = new AnimatedTexture(2, 32, 32, particleTexture);
            ellapsedTime = 0;
            clockPositions = new List<Rectangle>();
            clockVectors2 = new List<Vector2>();
        }
        /// <summary>
        /// Draws all of the animations of the current State of the Game.
        /// </summary>
        /// <param name="spriteBatch"> The SpriteBatch</param>
        /// <param name="gameTime"> The GameTime</param>
        public void Draw(GameTime gameTime)
        {
            //Finite State Machine for drawing the game based on the current GameState.
            switch (Game1.gameState)
            {
                case GameState.Start:
                    {
                        DrawBackgroundClock(gameTime);
                        break;
                    }
                case GameState.PauseMenu:
                    {
                        DrawBackgroundClock(gameTime);
                        break;
                    }
                case GameState.OptionsMenu:
                    {
                        DrawBackgroundClock(gameTime);
                        break;                        
                    }
                case GameState.ControlsMenu:
                    {
                        DrawBackgroundClock(gameTime);
                        break;                        
                    }
                case GameState.End:
                    {
                        DrawBackgroundClock(gameTime);
                        break;
                    }
                //Level
                default:
                    {
                        //While in a level, always draw all the tiles, interactable, and the player.
                        levelManager.DrawLevel(gameTime, spriteBatch);
                        player.Draw(gameTime, spriteBatch);

                        //Condition for time travel.
                        if(player.InTimeTravelAnim)
                        {
                            if (uiManager.TimeTravelAnimationActivated)
                            {
                                ShearScreen(gameTime);

                                DrawSpiralAnim(gameTime);

                                ClockTimeTravelAnim(gameTime);
                            }
                            else
                            {
                                levelManager.TimeStateChange();
                                player.InTimeTravelAnim = false;
                            }
                        }
                        //Time travel failed, animated the clock to refelct that
                        else if (levelManager.ThisLevel.Indicators.Count > 0)
                        {
                            FlashClockRed(gameTime);
                        }
                        //Otherwise just draw the clock normally.
                        else
                        {
                            spinSpeed = 2;
                            Game1.pastClock.Fps = spinSpeed;
                            Game1.futureClock.Fps = spinSpeed;
                            sizeOfClock = new Rectangle(448, 704, 128, 128);                            
                            if (Game1.timeState == TimeState.Past)
                            {
                                Game1.pastClock.Draw(sizeOfClock, spriteBatch, gameTime, SpriteEffects.None);
                            }
                            else
                            {
                                Game1.futureClock.Draw(sizeOfClock, spriteBatch, gameTime, SpriteEffects.None);
                            }
                        }
                        break;
                    }
            }
            //Always draws the uiManagers
            uiManager.Draw(Game1.gameState, gameTime, spriteBatch);
        }

        public void StartTimeTravel()
        {
            //Store previous positions only if we want the time travel animation.
            if (uiManager.TimeTravelAnimationActivated)
            {
                playerPosBeforeTimeTravel = player.Position;
                foreach (Interactable i in levelManager.ThisLevel.Interactables)
                {
                    interactablesPosBeforeTimeTravel[i] = i.Position;
                }
                foreach (Recepticle r in levelManager.ThisLevel.Recepticles)
                {
                    recepticlesPosBeforeTimeTravel[r] = r.Position;
                }
                if (player.PlayerState == PlayerState.WalkingLeft)
                {
                    player.PlayerState = PlayerState.FacingLeft;
                }
                else if (player.PlayerState == PlayerState.WalkingRight)
                {
                    player.PlayerState = PlayerState.FacingRight;
                }
                else if (player.PlayerState == PlayerState.WalkingUp)
                {
                    player.PlayerState = PlayerState.FacingUp;
                }
                else if (player.PlayerState == PlayerState.WalkingDown)
                {
                    player.PlayerState = PlayerState.FacingDown;
                }
            }
           
            player.InTimeTravelAnim = true;
        }
        /// <summary>
        /// Draws the spiral animation around the player.
        /// </summary>
        /// <param name="gameTime"> The GameTime </param>
        /// <param name="spriteBatch"> The SpriteBatch</param>
        public void DrawSpiralAnim(GameTime gameTime)
        {
            //Draw each of the sprites. Offsetting the angle each time
            for (double angle = 0; angle < 2 * Math.PI; angle += Math.PI / 8)
            {
                timeTravelAnim.Draw(new Rectangle((int)(player.X-20 + Math.Cos(angle + spiralAngle) * length) + (player.Position.Width / 2), (int)(player.Y-20 + Math.Sin(angle + spiralAngle) * length)
                    + (player.Position.Height / 2), 48, 48), spriteBatch, gameTime, SpriteEffects.None, 1, Color.White);
            }

            //Decrease the length at an increasing rate. Notice that length is a function of spiral angle
            length -= .2 + spiralAngle;
            //Change the angle at an increasing rate. Notice that spiralAngle is a function of itself
            spiralAngle += .006 * (1 + 4 * spiralAngle);
            //Once the sprites have converged onto one another, end the animation and initiate time travel.
            if (!player.InTimeTravelAnim)
            {
                length = 300;
                spiralAngle = 1;   
            }
        }
        /// <summary>
        /// Shears the screen so that each row of tiles and interactable move in unison according to the sin wave, with each row phased off from one another.
        /// </summary>
        public void ShearScreen(GameTime gameTime)
        {
            Rectangle currentTile;
            double x;
            int rowLength = levelManager[levelManager.CurrentLevel].Tiles.GetLength(0);
            int colLength = levelManager[levelManager.CurrentLevel].Tiles.GetLength(1);
            //Loop through every tile and shift all tiles in the same row in unison according to a variation of the sin wave.
            for (int row = 0; row < rowLength; row++)
            {
                for (int col = 0; col < colLength; col++)
                {
                    x = (currentTimeOfShearScreenAnim / timeOfShearScreenAnim * 2 * Math.PI) - (row * timeOffSet * 2 * Math.PI);
                    currentTile = levelManager[levelManager.CurrentLevel].Tiles[row, col].Position;
                    if (x >= 0 && x <= 2 * Math.PI)
                    {
                        currentTile = new Rectangle((int)(col * 64 + (amplitude * (Math.Abs(Math.Sin((x) / 2)) * Math.Sin(2 * (x))))), row * 64, currentTile.Width, currentTile.Height);
                        levelManager[levelManager.CurrentLevel].Tiles[row, col].Position = currentTile;
                    }
                    else if (x > 2 * Math.PI && x < 2 * Math.PI + 3.75)
                    {
                        CenterRow(row);
                    }
                }
            }
            //Loop through every Interactable and shift all tiles in the same row in unison according to the sin curve.
            foreach (Interactable i in levelManager.ThisLevel.Interactables)
            {
                x = (currentTimeOfShearScreenAnim / timeOfShearScreenAnim * 2 * Math.PI) - (i.Y / 64 * timeOffSet * 2 * Math.PI);
                //Move the interactable with respect to it's row and the sin curve.
                if (x >= 0 && x <= 2 * Math.PI)
                {
                    i.Position = new Rectangle((int)(interactablesPosBeforeTimeTravel[i].X + (amplitude * (Math.Abs(Math.Sin((x) / 2)) * Math.Sin(2 * (x))))), i.Y, i.Position.Width, i.Position.Height);
                }
            }
            //Loop through every Recepticle and shift all of them in the same row in unison according to the sin curve.
            foreach(Recepticle r in levelManager.ThisLevel.Recepticles)
            {
                x = (currentTimeOfShearScreenAnim / timeOfShearScreenAnim * 2 * Math.PI) - (r.Y / 64 * timeOffSet * 2 * Math.PI);
                //Move the interactable with respect to it's row and the sin curve.
                if (x >= 0 && x <= 2 * Math.PI)
                {
                    r.Position = new Rectangle((int)(recepticlesPosBeforeTimeTravel[r].X + (amplitude * (Math.Abs(Math.Sin((x) / 2)) * Math.Sin(2 * (x))))), r.Y, r.Position.Width, r.Position.Height);
                }
            }

            x = (currentTimeOfShearScreenAnim / timeOfShearScreenAnim * 2 * Math.PI) - (player.Y / 64 * timeOffSet * 2 * Math.PI);
            //Move the player with respect to it's row and the sin curve.
            if (x >= 0 && x <= 2 * Math.PI)
            {
                player.Position = new Rectangle((int)(playerPosBeforeTimeTravel.X + (amplitude * (Math.Abs(Math.Sin((x) / 2)) * Math.Sin(2 * (x))))), player.Position.Y, player.Position.Width, player.Position.Height);
            }

            currentTimeOfShearScreenAnim += gameTime.ElapsedGameTime.TotalSeconds;
            //Condition for switching time states.
            if (currentTimeOfShearScreenAnim >= (timeOfShearScreenAnim + rowLength * timeOffSet) / 2 && !switchedState)
            {
                levelManager.TimeStateChange();
                switchedState = true;
            }
            //Condition stopping the animation.
            else if (currentTimeOfShearScreenAnim > timeOfShearScreenAnim + (rowLength * timeOffSet))
            {
                currentTimeOfShearScreenAnim = 0;
                player.InTimeTravelAnim = false;
                switchedState = false;
            }
        }
        /// <summary>
        /// Centers all of the tiles, interactables, and the player in the specified row.
        /// </summary>
        /// <param name="row"> The row to center</param>
        public void CenterRow(int row)
        {
            Rectangle currentTile;
            int colLength = levelManager.ThisLevel.Tiles.GetLength(1);
            //Cycles through each tile in the row and reset their positions.
            for (int col = 0; col < colLength; col++)
            {
                currentTile = levelManager.ThisLevel.Tiles[row, col].Position;
                currentTile = new Rectangle(col * 64, row * 64, currentTile.Width, currentTile.Height);
                levelManager.ThisLevel.Tiles[row, col].Position = currentTile;
            }
            //Reset the players position if the player is in the row.
            if (playerPosBeforeTimeTravel.X / 64 == row)
            {
                player.Position = playerPosBeforeTimeTravel;
            }
            //Go through each interactable and reset their position if they are in the row.
            foreach (Interactable i in levelManager.ThisLevel.Interactables)
            {
                if (i.Position.X / 80 == row)
                {
                    i.Position = interactablesPosBeforeTimeTravel[i];
                }
            }
            //Go through each recepticle and reset their position if they are in the row.
            foreach (Recepticle r in levelManager.ThisLevel.Recepticles)
            {
                if (r.Position.X / 80 == row)
                {
                    r.Position = recepticlesPosBeforeTimeTravel[r];
                }
            }

        }
        /// <summary>
        /// Flashes the clock red.
        /// </summary>
        /// <param name="gameTime"> The GameTime</param>
        public void FlashClockRed(GameTime gameTime)
        {
            transparency += dTrandparency;
            if(transparency < .5f)
            {
                dTrandparency = .1f;
            }
            else if (transparency > 1)
            {
                dTrandparency = -.1f;
            }
            if(Game1.timeState == TimeState.Past)
            {
                Game1.pastClock.Draw(new Rectangle(448, 704, 128, 128), spriteBatch, gameTime, SpriteEffects.None, transparency, Color.Red);
            }
            else
            {
                Game1.futureClock.Draw(new Rectangle(448, 704, 128, 128), spriteBatch, gameTime, SpriteEffects.None, transparency, Color.Red);
            }
        }
        /// <summary>
        /// Spin the clock around faster and faster, then slow it down. Also make its size increase then decrease.
        /// </summary>
        /// <param name="gameTime"></param>
        public void ClockTimeTravelAnim(GameTime gameTime)
        {
            if(currentTimeOfShearScreenAnim < timeOfShearScreenAnim / 2 + .2)
            {
                spinSpeed += 1;
                sizeOfClock = new Rectangle(sizeOfClock.X - 1, sizeOfClock.Y-1, sizeOfClock.Width + 2, sizeOfClock.Height + 2);
                if(Game1.timeState == TimeState.Past)
                {
                    Game1.pastClock.Fps = spinSpeed;
                    Game1.pastClock.Draw(sizeOfClock, spriteBatch, gameTime, SpriteEffects.None);
                }
                else
                {
                    Game1.futureClock.Fps = spinSpeed;
                    Game1.futureClock.Draw(sizeOfClock, spriteBatch, gameTime, SpriteEffects.None);
                }
            }
            else
            {
                spinSpeed -= 1;
                sizeOfClock = new Rectangle(sizeOfClock.X +1, sizeOfClock.Y+1, sizeOfClock.Width - 2, sizeOfClock.Height - 2);
                if (Game1.timeState == TimeState.Past)
                {
                    Game1.pastClock.Fps = spinSpeed;
                    Game1.pastClock.Draw(sizeOfClock, spriteBatch, gameTime, SpriteEffects.None);
                }
                else
                {
                    Game1.futureClock.Fps = spinSpeed;
                    Game1.futureClock.Draw(sizeOfClock, spriteBatch, gameTime, SpriteEffects.None);
                }
            }
        }
        public void SpawnParticles()
        {
            //partManager.AddParticleSystem(new ParticleSystem(new Point(0, 0), 60, new Point(0, 0), 50, EmmisionShape.Box, 1, 1, player.Position.Center, new Point(20, 20), new AnimatedTexture(2,32,32,particleTexture)));
            //Console.WriteLine("Created Particle");
        }
        public void DrawBackgroundClock(GameTime gameTime)
        {
            ellapsedTime += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (ellapsedTime > 1000)
            {
                ellapsedTime = 0;
                int ran = rng.Next(0, 2);
                //Spawn on left side of screen.
                if (ran == 0)
                {
                    //Spawn it somewhere along the left side of the screen.
                    clockPositions.Add(new Rectangle(-128, rng.Next(768), 128, 128));
                    //generate an angle between -Pi/3 to Pi/3
                    double angle = (rng.NextDouble() * 2 * Math.PI / 3) - Math.PI / 3;
                    clockVectors2.Add(new Vector2((float)Math.Cos(angle),(float)Math.Sin(angle)));
                }
                //Spawn on right side of screen.
                else
                {
                    clockPositions.Add(new Rectangle(1024, rng.Next(768), 128, 128));
                    double angle = 2 * Math.PI / 3 + 2 * rng.NextDouble() * Math.PI / 3;
                    clockVectors2.Add(new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)));
                }

            }
            for (int i = 0; i < clockPositions.Count; i++)
            {
                //clockVectors2[i].Normalize();
                clockPositions[i] = new Rectangle((int)(clockPositions[i].X + 3*clockVectors2[i].X), (int)(clockPositions[i].Y + 3*clockVectors2[i].Y), clockPositions[i].Width, clockPositions[i].Height);
                backgroundClock.Draw(clockPositions[i], spriteBatch, gameTime, SpriteEffects.None, .5f, Color.White);
                if (clockPositions[i].X > 1024 || clockPositions[i].X<-128 || clockPositions[i].Y > 768 || clockPositions[i].Y<-128)
                {
                    clockPositions.RemoveAt(i);
                    clockVectors2.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}


