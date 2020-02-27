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
    /// this class deals with all of the input from the user.
    /// </summary>
    class InputManager
    {
        private KeyboardState kbState;
        private KeyboardState previousKbState;
        private MouseState mState;
        private MouseState previousMState;
        private Player player;
        private LevelManager levelManager;
        private UIManager uiManager;
        private CollisionManager colManager;
        private AnimationManager animManager;
        private GameState previousState;
        private ParticleManager pManager;


        /// <summary>
        /// constructor for InputManager
        /// </summary>
        public InputManager(Player p, LevelManager lm, UIManager ui, CollisionManager col, AnimationManager anim, ParticleManager pm)
        {
            player = p;
            levelManager = lm;
            uiManager = ui;
            colManager = col;
            animManager = anim;
            pManager = pm;
        }
        /// <summary>
        /// updates the game based on the users input
        /// </summary>
        public void Update()
        {
            previousKbState = kbState;
            previousMState = mState;
            kbState = Keyboard.GetState();
            mState = Mouse.GetState();

            //Always check for player using UI Elements.
            ProcessUIElements();
            //Only check for player input if the user is in a level.
            if (Game1.gameState.ToString().Length>5 && Game1.gameState.ToString().Substring(0, 5) == "Level")
            {
                //Player is not time traveling.
                if (!player.InTimeTravelAnim)
                {

                    ProcessPlayerState();
                    ProcessMovement();
                    ProcessPickup();
                    ProcessTimeTravel();
                    ProcessParticles();
                    ProcessGameEnd();
                }
            }
        }
        /// <summary>
        /// Checks if this is the first frame the key is being pressed
        /// </summary>
        /// <param name="key">The key to check</param>
        /// <returns>True if this is the first frame the key is being pressed, false otherwise</returns>
        public bool KeyPress(Keys key)
        {
            if(kbState.IsKeyDown(key) && previousKbState.IsKeyUp(key))
            {
                return true;
            } else
            {
                return false;
            }

        }
        public void ProcessPlayerState()
        {
            //Updates the players finite state machine.
            switch(player.PlayerState){
                case PlayerState.FacingDown:{
                    if (kbState.IsKeyDown(Keys.S))
                    {
                        player.PlayerState = PlayerState.WalkingDown;
                    }
                    if (kbState.IsKeyDown(Keys.D))
                    {
                        player.PlayerState = PlayerState.FacingRight;
                    }
                    if (kbState.IsKeyDown(Keys.W))
                    {
                        player.PlayerState = PlayerState.FacingUp;
                    }
                    if (kbState.IsKeyDown(Keys.A))
                    {
                        player.PlayerState = PlayerState.FacingLeft;
                    }
                    break;
                }
                case PlayerState.FacingLeft:{
                    if (kbState.IsKeyDown(Keys.S))
                    {
                        player.PlayerState = PlayerState.FacingDown;
                    }
                    if (kbState.IsKeyDown(Keys.D))
                    {
                        player.PlayerState = PlayerState.FacingRight;
                    }
                    if (kbState.IsKeyDown(Keys.W))
                    {
                        player.PlayerState = PlayerState.FacingUp;
                    }
                    if (kbState.IsKeyDown(Keys.A))
                    {
                        player.PlayerState = PlayerState.WalkingLeft;
                    }
                    break;
                }
                case PlayerState.FacingRight:{
                    if (kbState.IsKeyDown(Keys.S))
                    {
                        player.PlayerState = PlayerState.FacingDown;
                    }
                    if (kbState.IsKeyDown(Keys.D))
                    {
                        player.PlayerState = PlayerState.WalkingRight;
                    }
                    if (kbState.IsKeyDown(Keys.W))
                    {
                        player.PlayerState = PlayerState.FacingUp;
                    }
                    if (kbState.IsKeyDown(Keys.A))
                    {
                        player.PlayerState = PlayerState.FacingLeft;
                    }
                    break;
                }
                case PlayerState.FacingUp:{
                    if (kbState.IsKeyDown(Keys.S))
                    {
                        player.PlayerState = PlayerState.FacingDown;
                    }
                    if (kbState.IsKeyDown(Keys.D))
                    {
                        player.PlayerState = PlayerState.FacingRight;
                    }
                    if (kbState.IsKeyDown(Keys.W))
                    {
                        player.PlayerState = PlayerState.WalkingUp;
                    }
                    if (kbState.IsKeyDown(Keys.A))
                    {
                        player.PlayerState = PlayerState.FacingLeft;
                    }
                    break;
                }
                case PlayerState.WalkingDown:{
                    if(kbState.IsKeyUp(Keys.S)){
                        player.PlayerState = PlayerState.FacingDown;
                    }
                    break;
                }
                case PlayerState.WalkingLeft:{
                    if(kbState.IsKeyUp(Keys.A)){
                        player.PlayerState = PlayerState.FacingLeft;
                    }
                    break;
                }
                case PlayerState.WalkingRight:{
                    if(kbState.IsKeyUp(Keys.D)){
                        player.PlayerState = PlayerState.FacingRight;
                    }
                    break;
                }
                case PlayerState.WalkingUp:{
                    if(kbState.IsKeyUp(Keys.W)){
                        player.PlayerState = PlayerState.FacingUp;
                    }
                    break;
                }
            }
        }

        public void ProcessMovement()
        {
            //Updates the players position based on the players state.
            if(player.PlayerState == PlayerState.WalkingDown){
                player.Y += player.Speed;
                if(kbState.IsKeyDown(Keys.A)){
                    player.X -= player.Speed;
                }
                else if(kbState.IsKeyDown(Keys.D)){
                    player.X += player.Speed;
                }
            }
            if(player.PlayerState == PlayerState.WalkingLeft){
                player.X -= player.Speed;
                if(kbState.IsKeyDown(Keys.W)){
                    player.Y -= player.Speed;
                }
                else if(kbState.IsKeyDown(Keys.S)){
                    player.Y += player.Speed;
                }
            }
            if(player.PlayerState == PlayerState.WalkingRight){
                player.X += player.Speed;
                if(kbState.IsKeyDown(Keys.W)){
                    player.Y -= player.Speed;
                }
                else if(kbState.IsKeyDown(Keys.S)){
                    player.Y += player.Speed;
                }
            }
            if(player.PlayerState == PlayerState.WalkingUp){
                player.Y -= player.Speed;
                if(kbState.IsKeyDown(Keys.A)){
                    player.X -= player.Speed;
                }
                else if(kbState.IsKeyDown(Keys.D)){
                    player.X += player.Speed;
                }
            }       
        }

        public void ProcessPickup()
        {
            if(KeyPress(Keys.F))
            {
                if(player.HeldItem == null) //player is not holding an item --> try to pick up
                {
                    //Attempts to pickup the item in front of the player. If successful, removes it from the level.
                    if (player.PickUp(levelManager.ThisLevel.InteractableAt(player.TargetPosition)))
                    {
                        levelManager.ThisLevel.RemoveInteractable(player.HeldItem);
                    }
                    else if (levelManager.ThisLevel.InteractableAt(player.TargetPosition) != null) //There is an interactable there, but it cannot be picked up - try to use empty hand on it
                    {
                        player.UseHeldItemOn(levelManager.ThisLevel.InteractableAt(player.TargetPosition));
                    }
                    //Attempts to pickup the item from the recepticle in front of the player. If successful, removes it from the recepticle
                    if(levelManager.ThisLevel.RecepticleAt(player.TargetPosition) != null && player.PickUp(levelManager.ThisLevel.RecepticleAt(player.TargetPosition).Item))
                    {
                        levelManager.ThisLevel.RecepticleAt(player.TargetPosition).RemoveItem();
                    }
                } else //player is holding an item --> try to put down, use, or place into recepticle current held item
                {
                    if (levelManager.ThisLevel.InteractableAt(player.TargetPosition) != null) //there is an interactable on the ground --> use held item on it
                    {
                        player.UseHeldItemOn(levelManager.ThisLevel.InteractableAt(player.TargetPosition));
                    }
                    else if (levelManager.ThisLevel.RecepticleAt(player.TargetPosition) != null) //there is a recepticle on the ground --> place held item in it
                    {
                        player.PlaceHeldItemIn(levelManager.ThisLevel.RecepticleAt(player.TargetPosition));
                    } else //there is not an interactable on the ground --> put held item down
                    {
                        player.PutDown(levelManager.ThisLevel);
                    }
                }
                
            }
        }

        public void ProcessTimeTravel()
        {
            if(KeyPress(Keys.Space))
            {
                // A list of all interactables that would collide in the future.
                List<Interactable> collidingInteractables = levelManager.ThisLevel.InteractablesCollidingInOtherTime();

                if (collidingInteractables.Count == 0 & !colManager.PlayerIsCollidingInOtherTime() && levelManager.ThisLevel.Indicators.Count == 0)
                {
                    animManager.StartTimeTravel();
                } else
                {
                    foreach (Interactable i in collidingInteractables)
                    {
                        levelManager.ThisLevel.Indicators.Add(new Indicator(i, Color.Red, 1));
                    }
                    if(colManager.PlayerIsCollidingInOtherTime())
                    {
                        levelManager.ThisLevel.Indicators.Add(new Indicator(player, Color.Red, 1));
                    }
                }
                
            }
        }

        public void ProcessUIElements()
        {
            //Check whether or not the users mouse is hovering on the button and switch its state appropriately.
            foreach(KeyValuePair<string,UIButton> entry in uiManager.buttons)
            {
                if(entry.Value.MouseHover()){
                    entry.Value.Current = entry.Value.Hovered;
                }
                else{
                    entry.Value.Current = entry.Value.NotPressed;
                }
            }
            //Finite State Machine to check necessary input for each state.
            switch (Game1.gameState)
            {
                case GameState.Start:
                    {
                        //Start button is clicked.
                        if (uiManager["start"].Clicked())
                        {
                            Game1.gameState = GameState.Level1;
                        }
                        else if(uiManager["options"].Clicked())
                        {
                            previousState = GameState.Start;
                            Game1.gameState = GameState.OptionsMenu;
                        }
                        else if(uiManager["controls"].Clicked())
                        {
                            previousState = GameState.Start;
                            Game1.gameState = GameState.ControlsMenu;
                        }
                        break;
                    }
                case GameState.PauseMenu:
                    {
                        //Resume button is clicked.
                        if (uiManager["resume"].Clicked())
                        {
                            //Level 1.
                            if (levelManager.CurrentLevel == 0)
                            {
                                Game1.gameState = GameState.Level1;
                            }
                            //Level 2.
                            else if (levelManager.CurrentLevel == 1)
                            {
                                Game1.gameState = GameState.Level2;
                            }
                            //Level 3.
                            else if (levelManager.CurrentLevel == 1)
                            {
                                Game1.gameState = GameState.Level3;
                            }
                        }
                        else if(uiManager["options"].Clicked())
                        {
                            previousState = GameState.PauseMenu;
                            Game1.gameState = GameState.OptionsMenu;
                        }
                        else if(uiManager["controls"].Clicked())
                        {
                            previousState = GameState.PauseMenu;
                            Game1.gameState = GameState.ControlsMenu;
                        }
                        break;
                    }
                case GameState.OptionsMenu:
                    {
                        if(uiManager["back"].Clicked())
                        {
                            Game1.gameState = previousState;
                        }
                        break;
                    }
                case GameState.ControlsMenu:
                    {
                        if(uiManager["back"].Clicked())
                        {
                            Game1.gameState = previousState;
                        }
                        break;
                    }
                case GameState.End:
                    {
                        break;
                    }
                //Otherwise, the player is in a level.
                default:
                    {
                        //Pause button is clicked.
                        if (uiManager["pause"].Clicked())
                        {
                            Game1.gameState = GameState.PauseMenu;
                        }
                        break;
                    }
            }
        }
        public void ProcessParticles()
        {
            if (KeyPress(Keys.P))
            {
                animManager.SpawnParticles();               
            }

            if(KeyPress(Keys.C))
            {
                pManager.RemoveAllParticles();
            }
        }
        public bool MouseClick()
        {
            return (mState.LeftButton == ButtonState.Pressed && previousMState.LeftButton != ButtonState.Pressed);
        }
        public void ProcessGameEnd()
        {
            if(levelManager.CurrentLevel == levelManager.MaxLevel-1 && ( player.X > 1024 || player.X < 0 || player. Y <0 || player.Y > 768))
            {
                Game1.gameState = GameState.End;
            }
        }
    }
}
