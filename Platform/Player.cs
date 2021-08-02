using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Platform
{
    public class Player
    {
        public Texture2D Texture { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        private int currentFrame;
        private int[] blockSequence = {7, 0, 7, 0};
        private int blockIndex = 0;
        private bool blocking = false;
        private bool attacking = false;
        private float frameTime = 0.15f;
        private int totalFrames;
        private int blockSpeed;
        private int attackSpeed;

        private float time = 0f;
        private const float cooldownTimeMax = 1.0f;
        private float cooldownTime = 0;

        public Player(Texture2D texture, int rows, int columns, int blockspeed, int attackspeed)
        {
            Texture = texture;
            Rows = rows;
            Columns = columns;
            currentFrame = 0;
            totalFrames = Rows * Columns;
            blockSpeed = blockspeed;
            attackSpeed = attackspeed;
        }


        private void Input_Update(KeyboardState kstate, GameTime gameTime)
        {
            if (!attacking && !blocking && cooldownTime <= 0)
            {
                if (kstate.IsKeyDown(Keys.W))
                {
                    blocking = true;
                    blockSequence[1] = 4;
                    cooldownTime = cooldownTimeMax;
                }
                else if (kstate.IsKeyDown(Keys.S))
                {
                    blocking = true;
                    blockSequence[1] = 3;
                    cooldownTime = cooldownTimeMax;
                }
                else if (kstate.IsKeyDown(Keys.A))
                {
                    blocking = true;
                    blockSequence[1] = 2;
                    cooldownTime = cooldownTimeMax;
                }
                else if (kstate.IsKeyDown(Keys.D))
                {
                    blocking = true;
                    blockSequence[1] = 1;
                    cooldownTime = cooldownTimeMax;
                }

            }
        }

        public void Update(KeyboardState kstate, GameTime gameTime)
        {
            Input_Update(kstate, gameTime);
            // This is where we handle input and changing frames (by modifying currentFrame)
            if (blocking)
            {
                time += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (time >= frameTime)
                {
                    time = 0;
                    currentFrame = blockSequence[blockIndex];
                    ++blockIndex;
                    if (blockIndex > 3)  // A block sequence consists of at most 4 (index 3) different frames, if we reach a 5th frame, that means we've returned to normal
                    {
                        blocking = false;
                        blockIndex = 0;
                        time = 0;
                    } 
                }
            }
            if (cooldownTime > 0)
            {
                cooldownTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 location)
        {
            // This is the method called to draw the character sprite and it's location
            int width = Texture.Width / Columns;
            int height = Texture.Height / Rows;
            int row = (int)((float)currentFrame / (float)Columns);
            int column = currentFrame % Columns;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);

            spriteBatch.Begin();
            spriteBatch.Draw(Texture, location, sourceRectangle, Color.White, 0f, new Vector2(sourceRectangle.Width / 2, sourceRectangle.Height / 2),
                new Vector2(1.15f, 1.15f), SpriteEffects.None, 0f);
            spriteBatch.End();
        }
    }
}
