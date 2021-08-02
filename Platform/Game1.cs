using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Platform
{
    public class Game1 : Game
    {
        // Instance variables, these two are used for drawing graphics and creating sprite batches
        private Texture2D ballTexture;  // We declare the variable that refers to our main character ball texture
        // We create variables for the ball's properties (Could probably bundle this in a "ball Object" which inherits from a some Physics object class)
        private Vector2 ballPosition;
        private float ballSpeed;

        // Additional Sprite Texture variables
        private Texture2D background;
        private Texture2D shuttle;
        private Texture2D earth;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // Screen Center
        private Vector2 screenCenter;

        // Player data
        private Player player;
        private Texture2D playerTexture;

        public Game1()  // This is the primary Game constructor, initializes our variables above
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()  // Use this to initialize any non-graphics related services. Runs after the constructor
        {
            // TODO: Add your initialization logic here
            // We are going to setup the ball's initial position and speed
            ballPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);  // This is us centering the ball's position
            ballSpeed = 100f;

            screenCenter = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);
            base.Initialize();
        }

        protected override void LoadContent()  // Called once per game, this loads any assets needed for the game (I think?)
        {
            // We use Spritebatches to draw multiple textures at the same time: it is inefficient to draw each texture separately
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            // Now we Load the ball.png into our ballTexture as a sprite
            ballTexture = Content.Load<Texture2D>("ball");  // "ball" refers to "ball.png" added to the MGCB Editor

            // We also want to load in the textures for our other assets (Remember to add them to the MGCB Editor before)
            background = Content.Load<Texture2D>("stars");
            shuttle = Content.Load<Texture2D>("shuttle");
            earth = Content.Load<Texture2D>("earth");

            playerTexture = Content.Load<Texture2D>("Spears");
            player = new Player(playerTexture, 1, 8, 1, 1);
        }

        private void Ball_update(KeyboardState kstate, GameTime gameTime)
        {
            // Should probably generalize this key's set by a variable, so that custom input can be assigned.
            if (kstate.IsKeyDown(Keys.W))
            {
                ballPosition.Y -= ballSpeed * (float)(gameTime.ElapsedGameTime.TotalSeconds);  // We multiply by the time elapsed to adjust speed based on time and not frames
            }

            if (kstate.IsKeyDown(Keys.S))
            {
                ballPosition.Y += ballSpeed * (float)(gameTime.ElapsedGameTime.TotalSeconds);
            }

            if (kstate.IsKeyDown(Keys.A))
            {
               ballPosition.X -= ballSpeed * (float)(gameTime.ElapsedGameTime.TotalSeconds);
            }

            if (kstate.IsKeyDown(Keys.D))
            {
                ballPosition.X += ballSpeed * (float)(gameTime.ElapsedGameTime.TotalSeconds);
            }
        }

        protected override void Update(GameTime gameTime)  // Update method, self-explanatory: called multiple times a second as long as game is running (use to update game-state)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            var kstate = Keyboard.GetState();

            // TODO: Add your update logic here
            //Ball_update(kstate, gameTime);
            player.Update(kstate, gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)  // This method is called frequently similar to Update, but is used for drawing/graphics related operations (separate game and FPS)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            // Now we need to create a SpriteBatch for our ballTexture, add the texture, then close the batch to commit textures to drawing
            //  We could add multiple items to the same batch, and they will be drawn on top of each other
            _spriteBatch.Begin();  // Open up drawing for the sprite


            // Add in our drawing operations (all the sprites we want to draw in this batch)
            _spriteBatch.Draw(background, screenCenter, null, Color.White, 0f, new Vector2(background.Width / 2, background.Height / 2),
                Vector2.One, SpriteEffects.None, 0f);

            _spriteBatch.Draw(earth, screenCenter, null, Color.White);

            //  The Ball's position is provided below as a Vector2 in ballPosition
            //  We need to perform additional modifications to center the ball.png sprite's anchor to the actual center of the texture
            _spriteBatch.Draw(ballTexture, ballPosition, null, Color.White, 0f, new Vector2(ballTexture.Width / 2, ballTexture.Height / 2),
                Vector2.One, SpriteEffects.None, 0f);  // Here we are representing x,y positions by the head of a 2D vector

            _spriteBatch.Draw(shuttle, ballPosition, null, Color.White, 0f, new Vector2(shuttle.Width / 2, shuttle.Height / 2),
                Vector2.One, SpriteEffects.None, 0f);

            _spriteBatch.End();  // We close the batch, and now it will be drawn on each Draw Cll

            player.Draw(_spriteBatch, screenCenter);



            base.Draw(gameTime);
        }
    }
}
