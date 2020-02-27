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
    /// The different states of motion the player can be in
    /// </summary>
    public enum PlayerState
    {
        FacingLeft,
        FacingRight,
        FacingUp,
        FacingDown,
        WalkingLeft,
        WalkingRight,
        WalkingUp,
        WalkingDown
    }
    /// <summary>
    /// class that holds data for the Player
    /// </summary>
    public class Player : GameObject
    {
        private Dictionary<PlayerState, AnimatedTexture> textures;
        private PlayerState playerState;
        private int speed;
        private bool inTimeTravelAnim; //True while the time travel animation is in effect.
        private Interactable heldItem;
        private Texture2D indicatorTexture;
        private LevelManager levelManager;

        /// <summary>
        /// indexer property for textures. Includes both a getter and setter
        /// </summary>
        /// <param name="p">The PlayerState</param>
        /// <returns>The AnimatedTexture for that state</returns>
        public AnimatedTexture this[PlayerState p]
        {
            get { return textures[p]; }
            set { textures[p] = value; }
        }
        /// <summary>
        /// Getter and setter for startTimeTravel.
        /// </summary>
        public bool InTimeTravelAnim
        {
            get { return inTimeTravelAnim; }
            set { inTimeTravelAnim = value; }
        }
        /// <summary>
        /// Gets or sets the state of the player
        /// </summary>
        public PlayerState PlayerState
        {
            get { return playerState; }
            set { playerState = value; }
        }

        /// <summary>
        /// Gets the speed of the player, in pixels per frame
        /// </summary>
        public int Speed
        {
            get { return speed; }
        }

        /// <summary>
        /// Gets the item the player is holding
        /// </summary>
        public Interactable HeldItem
        {
            get
            {
                return heldItem;
            }
        }
        /// <summary>
        /// Gets a Vector2 containing the X and Y positions of the player in the world grid, measured in tiles
        /// </summary>
        private Vector2 GridCoords
        {
            get
            {
                return new Vector2(X / 64, Y / 64);
            }
        }

        /// <summary>
        /// Sets the texture for the item pickup indicator
        /// </summary>
        public Texture2D IndicatorTexture
        {
            set
            {
                indicatorTexture = value;
            }
        }
        /// <summary>
        /// Gets a Vector2 containing the position of the space in the world grid that the player is facing
        /// </summary>
        public Vector2 TargetPosition
        {
            get
            {
                Vector2 adjustedPlayerCoords = new Vector2((X + 32) / 64, (Y + 32) / 64);
                int placementX = (int)adjustedPlayerCoords.X;
                int placementY = (int)adjustedPlayerCoords.Y;
                switch (playerState)
                {
                    case PlayerState.FacingLeft:
                    case PlayerState.WalkingLeft:
                        placementX--;
                        break;
                    case PlayerState.FacingRight:
                    case PlayerState.WalkingRight:
                        placementX++;
                        break;
                    case PlayerState.FacingUp:
                    case PlayerState.WalkingUp:
                        placementY--;
                        break;
                    case PlayerState.FacingDown:
                    case PlayerState.WalkingDown:
                        placementY++;
                        break;
                }

                return new Vector2(placementX, placementY);
            }
        }

        public LevelManager LevelManager
        {
            set
            {
                levelManager = value;
            }
        }
        /// <summary>
        /// Creates a player
        /// </summary>
        /// <param name="position">The starting position of the player</param>
        /// <param name="collidable">Whether or not Player is collisable</param>
        public Player(Rectangle position, bool collidable, double scale): base(position, collidable, scale)
        {
            textures = new Dictionary<PlayerState, AnimatedTexture>();
            speed = 10;
            playerState = PlayerState.FacingRight;
            inTimeTravelAnim = false;
        }
        /// <summary>
        /// updates the Player's finite state machinne and other necessary fields
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            //implement code here
        }
        /// <summary>
        /// Draws the player at its current position and current frame of animation
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime, SpriteBatch sb)
        {
            if(playerState == PlayerState.FacingDown || playerState == PlayerState.FacingUp || playerState == PlayerState.FacingRight ||
                playerState == PlayerState.WalkingDown || playerState == PlayerState.WalkingUp || playerState == PlayerState.WalkingRight)
            {
                textures[playerState].Draw(position, sb, gameTime, SpriteEffects.None);
            }
            else if(playerState == PlayerState.FacingLeft || playerState == PlayerState.WalkingLeft)
            {
                textures[playerState].Draw(position, sb, gameTime, SpriteEffects.FlipHorizontally);
            }
            if(!inTimeTravelAnim)
            {
                sb.Draw(indicatorTexture, new Rectangle((int)(TargetPosition.X * 64), (int)(TargetPosition.Y * 64), 64, 64), Color.White);
            }
            
            //heldItem.Draw(gameTime, sb);
        }

        /// <summary>
        /// Attempts to pick up an interactable. Returns true if an interactable is picked up
        /// </summary>
        /// <param name="i">The interactable to pick up. Null if the player attempts to pick up empty space</param>
        /// <returns>True if there is no other interactable currently being held and something is picked up</returns>
        public bool PickUp(Interactable i)
        {
            if(i == null || i.Pickupable == false)
            {
                return false;
            } else
            {
                if(heldItem == null )
                {
                    heldItem = i;
                    return true;
                } else
                {
                    return false;
                }
            }
        }
        
        /// <summary>
        /// Attempts to place down an interactable into the world.
        /// </summary>
        /// <returns>The interactable that was placed if one was successfully placed, null otherwise</returns>
        public Interactable PutDown(Level l)
        {
            if(heldItem == null)
            {
                return null;
            } else
            {
                int placementX = (int)TargetPosition.X;
                int placementY = (int)TargetPosition.Y;
                if (l.IsEmpty(placementX, placementY, Game1.timeState))
                {
                    Interactable itemToPlace = heldItem;
                    itemToPlace.X = (int)(TargetPosition.X * 64);
                    itemToPlace.Y = (int)(TargetPosition.Y * 64);
                    l.AddInteractable(itemToPlace);
                    heldItem = null;
                    return itemToPlace;
                } else
                {
                    return null;
                }
            }
        }


        /// <summary>
        /// Attempts to use the current held item on <c>target</c>.
        /// </summary>
        /// <param name="target">The target interactable to use the current held item on</param>
        /// <param name="level">The level the target interactable is in</param>
        public void UseHeldItemOn(Interactable target)
        {
            if (heldItem == null)
            {
                UseEmptyHandOn(target.NameAndState);
                target.ReactToItemUse("null");
            } else
            {
                string heldItemNameAndState = heldItem.NameAndState;
                string targetItemNameAndState = target.NameAndState;
                target.ReactToItemUse(heldItemNameAndState);
                if(!heldItem.ReactToUsingOn(targetItemNameAndState))
                {
                    heldItem = null;
                }
                
            }
            
            
            
        }

        public void PlaceHeldItemIn(Recepticle target)
        {
            if(target.AcceptNewItem(heldItem))
            {
                heldItem = null;
            }
        }

        /// <summary>
        /// Handles all actions when the player attempts to pick up from an un-pickupable item.
        /// For all behavior here, it can be assumed that heldItem is null
        /// </summary>
        /// <param name="itemNameAndState">The name and state string of the item on the ground</param>
        private void UseEmptyHandOn(string itemNameAndState)
        {
            switch(itemNameAndState)
            {
                case "Flower_Flower":
                case "Flower_WateredFlower":
                    heldItem = new Flower(new Rectangle(-64, -64, 64, 64), FlowerState.HeldFlower, 1, levelManager.ThisLevel);
                    break;

                default:
                    break;
            }
        }
    }
}
