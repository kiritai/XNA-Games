using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace XNA_TD
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Level level = new Level();
        WaveManager waveManager;
        Player player;
        Toolbar toolBar;
        Button towerArrowButton;
        Button towerSpikeButton;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = level.Width * 32;
            graphics.PreferredBackBufferHeight = 32 + level.Height * 32;
            graphics.ApplyChanges();
            IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

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

            // TODO: use this.Content to load your game content here
            // > Texture variables
            Texture2D grass = Content.Load<Texture2D>("Textures\\Map\\Grass");
            Texture2D path = Content.Load<Texture2D>("Textures\\Map\\Path");
            Texture2D enemyGhost = Content.Load<Texture2D>("Textures\\Enemies\\Enemy-Ghost");
            Texture2D[] towerTextures = new Texture2D[]
            {
                Content.Load<Texture2D>("Textures\\Towers\\Tower-Arrow"),
                Content.Load<Texture2D>("Textures\\Towers\\Tower-Spike")
            };
            Texture2D towerTextureArrowBullet = Content.Load<Texture2D>("Textures\\Towers\\Tower-Bullet");
            Texture2D toolBarBottom = Content.Load<Texture2D>("GUI\\ToolBarBottom");
            Texture2D towerArrowButtonNormal = Content.Load<Texture2D>("GUI\\Tower-Arrow\\arrow-button-normal");
            Texture2D towerArrowButtonHover = Content.Load<Texture2D>("GUI\\Tower-Arrow\\arrow-button-hover");
            Texture2D towerArrowButtonPressed = Content.Load<Texture2D>("GUI\\Tower-Arrow\\arrow-button-pressed");
            Texture2D towerSpikeButtonNormal = Content.Load<Texture2D>("GUI\\Tower-Spike\\spike-button-normal");
            Texture2D towerSpikeButtonHover = Content.Load<Texture2D>("GUI\\Tower-Spike\\spike-button-hover");
            Texture2D towerSpikeButtonPressed = Content.Load<Texture2D>("GUI\\Tower-Spike\\spike-button-pressed");

            // > Audio
            Song backgroundMusic = Content.Load<Song>("Audio\\Background\\Background_Music");

            // > Spritefonts
            SpriteFont fontArial = Content.Load<SpriteFont>("GUI\\Arial");

            // > Add textures to level
            level.AddTexture(grass);
            level.AddTexture(path);

            // > Create tower
            player = new Player(level, towerTextures, towerTextureArrowBullet);

            // > Create enemy
            waveManager = new WaveManager(player, level, 24, enemyGhost);

            // > Everything related to the toolbar
            toolBar = new Toolbar(toolBarBottom, fontArial, new Vector2(0, level.Height * 32));
            towerArrowButton = new Button(towerArrowButtonNormal, towerArrowButtonHover, 
                towerArrowButtonPressed, new Vector2(0, level.Height * 32));
            towerSpikeButton = new Button(towerSpikeButtonNormal, towerSpikeButtonHover,
                towerSpikeButtonPressed, new Vector2(32, level.Height * 32));

            // > Event handler attaching
            towerArrowButton.Clicked += new EventHandler(towerArrowButton_Clicked);
            towerSpikeButton.Clicked += new EventHandler(towerSpikeButton_Clicked);

            // > Start music
            MediaPlayer.Play(backgroundMusic);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
            // Allows the game to exit
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            // TODO: Add your update logic here
            waveManager.Update(gameTime);
            player.Update(gameTime, waveManager.Enemies);
            towerArrowButton.Update(gameTime);
            towerSpikeButton.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            level.Draw(spriteBatch);
            waveManager.Draw(spriteBatch);
            player.Draw(spriteBatch);
            toolBar.Draw(spriteBatch, player);
            towerArrowButton.Draw(spriteBatch);
            towerSpikeButton.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void towerArrowButton_Clicked(object sender, EventArgs e)
        {
            player.NewTowerType = "Arrow Tower";
        }

        private void towerSpikeButton_Clicked(object sender, EventArgs e)
        {
            player.NewTowerType = "Spike Tower";
        }
    }
}
