using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace deltaTime
{
    public enum GameState{
        Start,
        PauseMenu,
        OptionsMenu,
        ControlsMenu,
        Level1,
        Level2,
        Level3,
        End
    }
    public enum TimeState
    {
        Past,
        Future
    }

    /// <summary>
    /// Yellow bird is better then the blue bird.
    /// Sky is a woman
    /// Justin is able to type basic sentences. (and little else)
    /// Jimmie Harkins
    /// </summary>
    class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Dictionary<PlayerState, Texture2D> playerSpriteSheets;
        private UIManager uiManager;
        private CollisionManager collisionManager;
        private InputManager inputManager;
        private LevelManager levelManager;
        private Player player;
        private double playerScale;
        private AnimationManager animationManager;
        private Texture2D pickupIndicatorTexture;
        public static TimeState timeState;
        public static GameState gameState;
        public static ParticleManager particleManager;
        public static AnimatedTexture pastClock;
        public static AnimatedTexture futureClock;
        public static AnimatedTexture regularClock;
        public static AnimatedTexture interactableBox;

        private Song backgroundMusic;

        public static StaticTextureManager staticTextureManager;
        public static Dictionary<String, Texture2D> tileTextures;

        //Debug objects
        private UIText optionsText;
        private SpriteFont arial16;
        //End debug objects

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            IsMouseVisible = true;

            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;
            graphics.ApplyChanges();

            gameState = GameState.Start;
            timeState = TimeState.Future;

            //Debug
            arial16 = Content.Load<SpriteFont>("Arial16");
            optionsText = new UIText(arial16, "Allow Time Travel Animation?", new Vector2(200, 350));

            //End Debug

            playerScale = 1.5;
            player = new Player(new Rectangle(100, 300, 60,75), true, playerScale);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            backgroundMusic = Content.Load<Song>("deltaTime");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.1f;
            MediaPlayer.Play(backgroundMusic);

            playerSpriteSheets = new Dictionary<PlayerState, Texture2D>();
            playerSpriteSheets[PlayerState.FacingLeft] = Content.Load<Texture2D>("mainStandingSpriteSheet"); //all temporary code
            playerSpriteSheets[PlayerState.WalkingLeft] = Content.Load<Texture2D>("mainWalkingSpriteSheet");
            player[PlayerState.WalkingLeft] = new AnimatedTexture(8, 64, 80, playerSpriteSheets[PlayerState.WalkingLeft]);
            player[PlayerState.WalkingRight] = new AnimatedTexture(8, 64, 80, playerSpriteSheets[PlayerState.WalkingLeft]);
            player[PlayerState.WalkingDown] = new AnimatedTexture(8, 64, 80, playerSpriteSheets[PlayerState.WalkingLeft]);
            player[PlayerState.WalkingUp] = new AnimatedTexture(8, 64, 80, playerSpriteSheets[PlayerState.WalkingLeft]);
            player[PlayerState.FacingDown] = new AnimatedTexture(0, 64, 80, playerSpriteSheets[PlayerState.FacingLeft]);
            player[PlayerState.FacingLeft] = new AnimatedTexture(0, 64, 80, playerSpriteSheets[PlayerState.FacingLeft]);
            player[PlayerState.FacingRight] = new AnimatedTexture(0, 64, 80, playerSpriteSheets[PlayerState.FacingLeft]);
            player[PlayerState.FacingUp] = new AnimatedTexture(0, 64, 80, playerSpriteSheets[PlayerState.FacingLeft]);
            pickupIndicatorTexture = Content.Load<Texture2D>("pickupIndicator");
            player.IndicatorTexture = pickupIndicatorTexture;


            staticTextureManager = new StaticTextureManager();
            staticTextureManager["indicator"] = Content.Load<Texture2D>("Indicator");
            staticTextureManager["OnRecepticle"] = Content.Load<Texture2D>("OnRecepticle");
            staticTextureManager["OffRecepticle"] = Content.Load<Texture2D>("OffRecepticle");
            staticTextureManager["LeafParticle"] = Content.Load<Texture2D>("LeafParticle");
            staticTextureManager["DirtParticle"] = Content.Load<Texture2D>("DirtParticle");
            staticTextureManager["WaterParticle"] = Content.Load<Texture2D>("WaterParticle");
            staticTextureManager["SparkParticle"] = Content.Load<Texture2D>("SparkParticle");
            staticTextureManager["FeatherParticle"] = Content.Load<Texture2D>("FeatherParticle");
            foreach (ChickenState state in Enum.GetValues(typeof(ChickenState)))
            {
                staticTextureManager["Chicken_" + state.ToString()] = Content.Load<Texture2D>("Chicken_" + state.ToString());
            }

            foreach (FlowerState state in Enum.GetValues(typeof(FlowerState)))
            {
                staticTextureManager["Flower_" + state.ToString()] = Content.Load<Texture2D>("Flower_" + state.ToString());
            }

            foreach (WateringCanState state in Enum.GetValues(typeof(WateringCanState)))
            {
                staticTextureManager["WateringCan_" + state.ToString()] = Content.Load<Texture2D>("WateringCan_" + state.ToString());
            }

            foreach (TreeState state in Enum.GetValues(typeof(TreeState)))
            {
                if(state == TreeState.DirtThatWillBeSapling)
                {
                    staticTextureManager["Tree_" + state.ToString()] = Content.Load<Texture2D>("Tree_Dirt");
                } else
                {
                    staticTextureManager["Tree_" + state.ToString()] = Content.Load<Texture2D>("Tree_" + state.ToString());
                }
                
            }

            foreach (CampfireState state in Enum.GetValues(typeof(CampfireState)))
            {
                if(state == CampfireState.PileOfAsh)
                {
                    staticTextureManager["Campfire_" + state.ToString()] = Content.Load<Texture2D>("Campfire_UnlitCampfire"); //temporary until sprite is made
                } else
                {
                    staticTextureManager["Campfire_" + state.ToString()] = Content.Load<Texture2D>("Campfire_" + state.ToString());
                }
                
            }

            foreach (TorchState state in Enum.GetValues(typeof(TorchState)))
            {
                staticTextureManager["Torch_" + state.ToString()] = Content.Load<Texture2D>("Torch_" + state.ToString());
            }

            foreach (ShovelState state in Enum.GetValues(typeof(ShovelState)))
            {
                staticTextureManager["Shovel_" + state.ToString()] = Content.Load<Texture2D>("Shovel_" + state.ToString());
            }

            foreach(AxeState state  in Enum.GetValues(typeof(AxeState)))
            {
                staticTextureManager["Axe_" + state.ToString()] = Content.Load<Texture2D>("Axe_" + state.ToString());
            }
            //Recepticle debugRecepticle = new Recepticle(TimeState.Future, "Chicken_Chicken", new Rectangle(192, 192, 64, 64));

            tileTextures = new Dictionary<string, Texture2D>();
            //floors
            tileTextures.Add("floor", Content.Load<Texture2D>("FloorPH"));
            tileTextures.Add("PastFloor", Content.Load<Texture2D>("PastFloor"));
            tileTextures.Add("FutureFloor0", Content.Load<Texture2D>("FutureFloor0"));
            tileTextures.Add("FutureFloor1", Content.Load<Texture2D>("FutureFloor1"));
            tileTextures.Add("FutureFloor2", Content.Load<Texture2D>("FutureFloor2"));
            tileTextures.Add("FutureFloor3", Content.Load<Texture2D>("FutureFloor3"));
            tileTextures.Add("FutureFloor4", Content.Load<Texture2D>("FutureFloor4"));
            //exterior corner
            tileTextures.Add("PastExtCorner", Content.Load<Texture2D>("PastExtWallCorner"));
            tileTextures.Add("FutureExtCorner0", Content.Load<Texture2D>("FutureExtWallCorner0"));
            tileTextures.Add("FutureExtCorner1", Content.Load<Texture2D>("FutureExtWallCorner1"));
            tileTextures.Add("FutureExtCorner2", Content.Load<Texture2D>("FutureExtWallCorner2"));
            tileTextures.Add("FutureExtCorner3", Content.Load<Texture2D>("FutureExtWallCorner3"));
            tileTextures.Add("FutureExtCorner4", Content.Load<Texture2D>("FutureExtWallCorner4"));
            //exterior walls
            tileTextures.Add("PastExtWallHoriz", Content.Load<Texture2D>("PastExtWallHoriz"));
            tileTextures.Add("FutureExtWallHoriz0", Content.Load<Texture2D>("FutureExtWallHoriz0"));
            tileTextures.Add("FutureExtWallHoriz1", Content.Load<Texture2D>("FutureExtWallHoriz1"));
            tileTextures.Add("FutureExtWallHoriz2", Content.Load<Texture2D>("FutureExtWallHoriz2"));
            tileTextures.Add("FutureExtWallHoriz3", Content.Load<Texture2D>("FutureExtWallHoriz3"));
            tileTextures.Add("FutureExtWallHoriz4", Content.Load<Texture2D>("FutureExtWallHoriz4"));
            tileTextures.Add("PastExtWallVert", Content.Load<Texture2D>("PastExtWallVert"));
            tileTextures.Add("FutureExtWallVert0", Content.Load<Texture2D>("FutureExtWallVert0"));
            tileTextures.Add("FutureExtWallVert1", Content.Load<Texture2D>("FutureExtWallVert1"));
            tileTextures.Add("FutureExtWallVert2", Content.Load<Texture2D>("FutureExtWallVert2"));
            tileTextures.Add("FutureExtWallVert3", Content.Load<Texture2D>("FutureExtWallVert3"));
            tileTextures.Add("FutureExtWallVert4", Content.Load<Texture2D>("FutureExtWallVert4"));
            //exterior doors
            tileTextures.Add("PastLockedDoorExtHoriz", Content.Load<Texture2D>("PastLockedDoorExtHoriz"));
            tileTextures.Add("FutureLockedDoorExtHoriz0", Content.Load<Texture2D>("FutureLockedDoorExtHoriz0"));
            tileTextures.Add("FutureLockedDoorExtHoriz1", Content.Load<Texture2D>("FutureLockedDoorExtHoriz1"));
            tileTextures.Add("FutureLockedDoorExtHoriz2", Content.Load<Texture2D>("FutureLockedDoorExtHoriz2"));
            tileTextures.Add("FutureLockedDoorExtHoriz3", Content.Load<Texture2D>("FutureLockedDoorExtHoriz3"));
            tileTextures.Add("FutureLockedDoorExtHoriz4", Content.Load<Texture2D>("FutureLockedDoorExtHoriz4"));
            tileTextures.Add("PastLockedDoorExtVert", Content.Load<Texture2D>("PastLockedDoorExtVert"));
            tileTextures.Add("FutureLockedDoorExtVert0", Content.Load<Texture2D>("FutureLockedDoorExtVert0"));
            tileTextures.Add("FutureLockedDoorExtVert1", Content.Load<Texture2D>("FutureLockedDoorExtVert1"));
            tileTextures.Add("FutureLockedDoorExtVert2", Content.Load<Texture2D>("FutureLockedDoorExtVert2"));
            tileTextures.Add("FutureLockedDoorExtVert3", Content.Load<Texture2D>("FutureLockedDoorExtVert3"));
            tileTextures.Add("FutureLockedDoorExtVert4", Content.Load<Texture2D>("FutureLockedDoorExtVert4"));
            tileTextures.Add("PastOpenDoorExtHoriz", Content.Load<Texture2D>("PastOpenDoorExtHoriz"));
            tileTextures.Add("FutureOpenDoorExtHoriz0", Content.Load<Texture2D>("FutureOpenDoorExtHoriz0"));
            tileTextures.Add("FutureOpenDoorExtHoriz1", Content.Load<Texture2D>("FutureOpenDoorExtHoriz1"));
            tileTextures.Add("FutureOpenDoorExtHoriz2", Content.Load<Texture2D>("FutureOpenDoorExtHoriz2"));
            tileTextures.Add("FutureOpenDoorExtHoriz3", Content.Load<Texture2D>("FutureOpenDoorExtHoriz3"));
            tileTextures.Add("FutureOpenDoorExtHoriz4", Content.Load<Texture2D>("FutureOpenDoorExtHoriz4"));
            tileTextures.Add("PastOpenDoorExtVert", Content.Load<Texture2D>("PastOpenDoorExtVert"));
            tileTextures.Add("FutureOpenDoorExtVert0", Content.Load<Texture2D>("FutureOpenDoorExtVert0"));
            tileTextures.Add("FutureOpenDoorExtVert1", Content.Load<Texture2D>("FutureOpenDoorExtVert1"));
            tileTextures.Add("FutureOpenDoorExtVert2", Content.Load<Texture2D>("FutureOpenDoorExtVert2"));
            tileTextures.Add("FutureOpenDoorExtVert3", Content.Load<Texture2D>("FutureOpenDoorExtVert3"));
            tileTextures.Add("FutureOpenDoorExtVert4", Content.Load<Texture2D>("FutureOpenDoorExtVert4"));
            //interior corner
            tileTextures.Add("PastIntWallCorner", Content.Load<Texture2D>("PastIntWallCorner"));
            tileTextures.Add("FutureIntWallCorner0", Content.Load<Texture2D>("FutureIntWallCorner0"));
            tileTextures.Add("FutureIntWallCorner1", Content.Load<Texture2D>("FutureIntWallCorner1"));
            tileTextures.Add("FutureIntWallCorner2", Content.Load<Texture2D>("FutureIntWallCorner2"));
            tileTextures.Add("FutureIntWallCorner3", Content.Load<Texture2D>("FutureIntWallCorner3"));
            tileTextures.Add("FutureIntWallCorner4", Content.Load<Texture2D>("FutureIntWallCorner4"));
            //interior walls
            tileTextures.Add("PastIntWallHoriz", Content.Load<Texture2D>("PastIntWallHoriz"));
            tileTextures.Add("FutureIntWallHoriz0", Content.Load<Texture2D>("FutureIntWallHoriz0"));
            tileTextures.Add("FutureIntWallHoriz1", Content.Load<Texture2D>("FutureIntWallHoriz1"));
            tileTextures.Add("FutureIntWallHoriz2", Content.Load<Texture2D>("FutureIntWallHoriz2"));
            tileTextures.Add("FutureIntWallHoriz3", Content.Load<Texture2D>("FutureIntWallHoriz3"));
            tileTextures.Add("FutureIntWallHoriz4", Content.Load<Texture2D>("FutureIntWallHoriz4"));
            tileTextures.Add("PastIntWallVert", Content.Load<Texture2D>("PastIntWallVert"));
            tileTextures.Add("FutureIntWallVert0", Content.Load<Texture2D>("FutureIntWallVert0"));
            tileTextures.Add("FutureIntWallVert1", Content.Load<Texture2D>("FutureIntWallVert1"));
            tileTextures.Add("FutureIntWallVert2", Content.Load<Texture2D>("FutureIntWallVert2"));
            tileTextures.Add("FutureIntWallVert3", Content.Load<Texture2D>("FutureIntWallVert3"));
            tileTextures.Add("FutureIntWallVert4", Content.Load<Texture2D>("FutureIntWallVert4"));
            //interior doors
            tileTextures.Add("PastLockedDoorIntHoriz", Content.Load<Texture2D>("PastLockedDoorIntHoriz"));
            tileTextures.Add("FutureLockedDoorIntHoriz0", Content.Load<Texture2D>("FutureLockedDoorIntHoriz0"));
            tileTextures.Add("FutureLockedDoorIntHoriz1", Content.Load<Texture2D>("FutureLockedDoorIntHoriz1"));
            tileTextures.Add("FutureLockedDoorIntHoriz2", Content.Load<Texture2D>("FutureLockedDoorIntHoriz2"));
            tileTextures.Add("FutureLockedDoorIntHoriz3", Content.Load<Texture2D>("FutureLockedDoorIntHoriz3"));
            tileTextures.Add("FutureLockedDoorIntHoriz4", Content.Load<Texture2D>("FutureLockedDoorIntHoriz4"));
            tileTextures.Add("PastLockedDoorIntVert", Content.Load<Texture2D>("PastLockedDoorIntVert"));
            tileTextures.Add("FutureLockedDoorIntVert0", Content.Load<Texture2D>("FutureLockedDoorIntVert0"));
            tileTextures.Add("FutureLockedDoorIntVert1", Content.Load<Texture2D>("FutureLockedDoorIntVert1"));
            tileTextures.Add("FutureLockedDoorIntVert2", Content.Load<Texture2D>("FutureLockedDoorIntVert2"));
            tileTextures.Add("FutureLockedDoorIntVert3", Content.Load<Texture2D>("FutureLockedDoorIntVert3"));
            tileTextures.Add("FutureLockedDoorIntVert4", Content.Load<Texture2D>("FutureLockedDoorIntVert4"));
            tileTextures.Add("PastOpenDoorIntHoriz", Content.Load<Texture2D>("PastOpenDoorIntHoriz"));
            tileTextures.Add("FutureOpenDoorIntHoriz0", Content.Load<Texture2D>("FutureOpenDoorIntHoriz0"));
            tileTextures.Add("FutureOpenDoorIntHoriz1", Content.Load<Texture2D>("FutureOpenDoorIntHoriz1"));
            tileTextures.Add("FutureOpenDoorIntHoriz2", Content.Load<Texture2D>("FutureOpenDoorIntHoriz2"));
            tileTextures.Add("FutureOpenDoorIntHoriz3", Content.Load<Texture2D>("FutureOpenDoorIntHoriz3"));
            tileTextures.Add("FutureOpenDoorIntHoriz4", Content.Load<Texture2D>("FutureOpenDoorIntHoriz4"));
            tileTextures.Add("PastOpenDoorIntVert", Content.Load<Texture2D>("PastOpenDoorIntVert"));
            tileTextures.Add("FutureOpenDoorIntVert0", Content.Load<Texture2D>("FutureOpenDoorIntVert0"));
            tileTextures.Add("FutureOpenDoorIntVert1", Content.Load<Texture2D>("FutureOpenDoorIntVert1"));
            tileTextures.Add("FutureOpenDoorIntVert2", Content.Load<Texture2D>("FutureOpenDoorIntVert2"));
            tileTextures.Add("FutureOpenDoorIntVert3", Content.Load<Texture2D>("FutureOpenDoorIntVert3"));
            tileTextures.Add("FutureOpenDoorIntVert4", Content.Load<Texture2D>("FutureOpenDoorIntVert4"));
            //interior "T-wall"
            tileTextures.Add("PastTWallHoriz", Content.Load<Texture2D>("PastTWallHoriz"));
            tileTextures.Add("FutureTWallHoriz0", Content.Load<Texture2D>("FutureTWallHoriz0"));
            tileTextures.Add("FutureTWallHoriz1", Content.Load<Texture2D>("FutureTWallHoriz1"));
            tileTextures.Add("FutureTWallHoriz2", Content.Load<Texture2D>("FutureTWallHoriz2"));
            tileTextures.Add("FutureTWallHoriz3", Content.Load<Texture2D>("FutureTWallHoriz3"));
            tileTextures.Add("FutureTWallHoriz4", Content.Load<Texture2D>("FutureTWallHoriz4"));
            tileTextures.Add("PastTWallVert", Content.Load<Texture2D>("PastTWallVert"));
            tileTextures.Add("FutureTWallVert0", Content.Load<Texture2D>("FutureTWallVert0"));
            tileTextures.Add("FutureTWallVert1", Content.Load<Texture2D>("FutureTWallVert1"));
            tileTextures.Add("FutureTWallVert2", Content.Load<Texture2D>("FutureTWallVert2"));
            tileTextures.Add("FutureTWallVert3", Content.Load<Texture2D>("FutureTWallVert3"));
            tileTextures.Add("FutureTWallVert4", Content.Load<Texture2D>("FutureTWallVert4"));
            //interior "Cross" wall
            tileTextures.Add("PastIntWallCross", Content.Load<Texture2D>("PastIntWallCross"));
            tileTextures.Add("FutureIntWallCross0", Content.Load<Texture2D>("FutureIntWallCross0"));
            tileTextures.Add("FutureIntWallCross1", Content.Load<Texture2D>("FutureIntWallCross1"));
            tileTextures.Add("FutureIntWallCross2", Content.Load<Texture2D>("FutureIntWallCross2"));
            tileTextures.Add("FutureIntWallCross3", Content.Load<Texture2D>("FutureIntWallCross3"));
            tileTextures.Add("FutureIntWallCross4", Content.Load<Texture2D>("FutureIntWallCross4"));
            //placeholders
            tileTextures.Add("block", Content.Load<Texture2D>("BlockPH"));
            tileTextures.Add("door", Content.Load<Texture2D>("DoorPH"));


            particleManager = new ParticleManager(spriteBatch);


            uiManager = new UIManager(player, spriteBatch);
            levelManager = new LevelManager(tileTextures, player);
            collisionManager = new CollisionManager(player, levelManager);
            animationManager = new AnimationManager(levelManager, player, uiManager, new AnimatedTexture(2, 32, 32, Content.Load<Texture2D>("timeTravelEffect")), collisionManager, spriteBatch, particleManager, Content.Load<Texture2D>("regularClock"));
            inputManager = new InputManager(player, levelManager, uiManager, collisionManager, animationManager, particleManager);
            uiManager.InputManager = inputManager;
            uiManager.Add(optionsText);


            //Buttons
            UIButton start = new UIButton(Content.Load<Texture2D>("start"), Content.Load<Texture2D>("startHover"), new Rectangle(441, 400, 142, 64));
            UIButton pause = new UIButton(Content.Load<Texture2D>("pause"), Content.Load<Texture2D>("pauseHover"), new Rectangle(GraphicsDevice.Viewport.Width - 32, 0, 32, 32));
            UIButton resume = new UIButton(Content.Load<Texture2D>("resume"), Content.Load<Texture2D>("resumeHover"), new Rectangle(441, 400, 142, 64));
            UIButton logo = new UIButton(Content.Load<Texture2D>("deltaTimelogo"), Content.Load<Texture2D>("deltaTimelogo"), new Rectangle(296, 100, 432, 192));
            UIButton options = new UIButton(Content.Load<Texture2D>("options"), Content.Load<Texture2D>("optionsHover"), new Rectangle(441, 500, 142, 64));
            UIButton back = new UIButton(Content.Load<Texture2D>("back"), Content.Load<Texture2D>("backHover"), new Rectangle(1, 1, 32, 32));
            UIButton controls = new UIButton(Content.Load<Texture2D>("controls"), Content.Load<Texture2D>("controlsHover"), new Rectangle(441, 600, 142, 64));
            UIButton controlsMenu = new UIButton(Content.Load<Texture2D>("controlsMenu"), Content.Load<Texture2D>("controlsMenu"), new Rectangle(137, 99, 750, 570));
            UIButton blankCheckBox = new UIButton(Content.Load<Texture2D>("blankCheckBox"), Content.Load<Texture2D>("blankCheckBox"), new Rectangle(480, 332, 64, 64));
            UIButton clickedCheckBox = new UIButton(Content.Load<Texture2D>("clickedCheckBox"), Content.Load<Texture2D>("clickedCheckBox"), new Rectangle(480, 332, 64, 64));
            UIButton end = new UIButton(Content.Load<Texture2D>("end"), Content.Load<Texture2D>("end"), new Rectangle(384, 256, 254, 256));

            uiManager.Add("start", start);
            uiManager.Add("pause", pause);
            uiManager.Add("resume", resume);
            uiManager.Add("logo", logo);
            uiManager.Add("options", options);
            uiManager.Add("back", back);
            uiManager.Add("controls", controls);
            uiManager.Add("controlsMenu", controlsMenu);
            uiManager.Add("blankCheckBox", blankCheckBox);
            uiManager.Add("clickedCheckBox", clickedCheckBox);
            uiManager.Add("End", end);

            //Axe debugAxe = new Axe(new Rectangle(256, 256, 64, 64), AxeState.Axe, 1, levelManager.ThisLevel);
            //levelManager.ThisLevel.AddInteractable(debugAxe);

            pastClock = new AnimatedTexture(2, 32, 32, Content.Load<Texture2D>("pastClock"));
            futureClock = new AnimatedTexture(2, 32, 32, Content.Load<Texture2D>("futureClock"));
            regularClock = new AnimatedTexture(2, 32, 32, Content.Load<Texture2D>("regularClock"));
            interactableBox = new AnimatedTexture(0, 32, 32, Content.Load<Texture2D>("InteractableBorder"));

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            //Always update input.
            inputManager.Update();
            particleManager.GT = gameTime;
            particleManager.Update();
            //Finite State Machine for updating the GameState specifics
            switch (gameState)
            {
                case GameState.Start:
                    {
                        break;
                    }
                case GameState.PauseMenu:
                    {
                        break;
                    }
                case GameState.OptionsMenu:
                    {
                        break;                        
                    }
                case GameState.ControlsMenu:
                    {
                        break;                        
                    }
                case GameState.End:
                    {
                        break;
                    }
                default:
                    {
                        //Only update collision manager when the player is in a level.
                        collisionManager.Update();
                        //Update the current level;
                        levelManager.ThisLevel.Update(gameTime);
                        break;
                    }
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
            
            //Only need to call Draw on the AnimationManager.
            animationManager.Draw(gameTime);
            particleManager.Draw();
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
