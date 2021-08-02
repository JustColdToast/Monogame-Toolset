using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

// Kanwarpal Brar
// This module provides functionality for playing a specific animation (of type Animation), given a location to draw, among other display properties
//  All information needed regarding frames and timing is self-contained in the Animation object itself
// Note: This is heavily based on the Monogame Sample 2D Platformer AnimationPlayer, with edits made to accommodate the different Animation class I use

// -- Note to self -- : Remember to distinctly separate the client and module: client should not be accessing module fields without going through functions that moderate behavior

/*
    Ideally, we pass a pointer to a vector position in our draw function, or call it on each draw (by passing a location on each call)
    
    Additionally, we want to have toggles on specific frames, so the Animation can be asked to return true or false (perhaps some generic structure or null as well) when
     a specific frame in the target animation is reached
     This eliminates the need for manually tracking the current frame in animation by time elapsed in the calling class, but means that each draw call must be made to return to a 
     variable that holds this toggle value
*/

namespace Platform
{
    struct AnimationPlayer
    {
        private Animation animation;
        public Animation Animation
        {
            get { return animation; }
        }

        private int frameIndex;
        public int FrameIndex
        {
            get { return frameIndex; }
        }

        private float time;  // Time variable internally used by AnimationPlayer to track time elapsed since last action

        // Returns the center of the animation frame
        public Vector2 Origin
        {
            get { return new Vector2(this.Animation.Size / 2.0f, Animation.Size / 2.0f); }
        }

        // This must be called first, before the AnimationPlayer can begin drawing the animation (FrameIndex setup is imperative)
        public void PlayAnimation(Animation animation)
        {
            if (Animation == animation)
            {
                return;
            }
            this.animation = animation;
            this.frameIndex = 0;
        }

        // Draw call made on each main file draw call (must be passed game times, etc)
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position, SpriteEffects spriteEffects)
        {
            Debug.Assert(animation != null);  // Note, we can reference this.animation, or AnimationPlayer.Animation, which is equivalent by the property get statement

            time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (time > Animation.Timing(FrameIndex))  // If we have elapsed enough time for the next frame
            {
                time = 0;
                frameIndex++;
                if (frameIndex > Animation.FrameCount)
                {
                    switch (Animation.IsLooping)  // Switch case instead of if-else for fun
                    {
                        case true:
                            frameIndex = 0;
                            break;
                        case false:
                            frameIndex = Animation.FrameCount - 1;
                            break;
                    }
                }
            }

            Rectangle source = new Rectangle(FrameIndex * Animation.Size, 0, Animation.Size, Animation.Size);

            // Draw the Current Frame, given frame
            spriteBatch.Draw(Animation.Texture, position, source, Color.White, 0.0f, Origin, Vector2.One, spriteEffects, 0.0f);
        }
    }
}
