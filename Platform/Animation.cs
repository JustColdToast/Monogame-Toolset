using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

// Kanwarpal Brar
// This class represents a set of frames (in a texture atlas) which together make up a single animation
//  It is best used for cosmetic animations (nothing that requires movement that cannot be separated from animation)

// -- Note to self -- : Remember to distinctly separate the client and module: client should not be accessing module fields without going through functions that moderate behavior

/*
    The basic properties of each Animation are the [Height] and [Width] of each frame (assuming consistent)
    as well as the [frameTime] (which is the amount of time between frames changing)

    Ideally, we want to also be able to provide custom timings (say, we want a specific frame to last longer than another)

    Animations are aligned in strips, horizontally, and may be provided as a sequence alongside the strip (0 to n, left to right, for n - 1 different frames) [repeating allowed]

    We assume that each frame is square: same width and height (this simplifies the math a little bit)
*/

namespace Platform
{
    class Animation
    {
        // Our variables are made private, with publicly accessible getters and setters
        private float frameTime;
        public float FrameTime
        {
            get
            {
                return frameTime;
            }
            set
            {
                frameTime = value;  // In this context, value represents whatever is being passed to frameTime
            }
        }

        // The below texture should be a texture atlas
        private Texture2D texture;  // Internally, we choose to modify the private variables, not use the setters/getters (client must use them though)
        public Texture2D Texture
        {
            get
            {
                return texture;
            }
        }

        // Func<int, int> customFrameTime = x => { return x; };  // This is how to make a lambda function [I'm leaving this in because I plan to use it later]

        private bool isLooping;
        public bool IsLooping
        {
            get { return isLooping; }
            set { isLooping = value; }
        }

        // Internal variables assigned by constructor
        private int frameCount;
        public int FrameCount
        {
            get { return frameCount; }
        }
        private int frameSize;  // Represents the square side length of a single frame
        public int Size
        {
            get { return Texture.Height; }
        }

        private int[] frameSequence;  // I might consider exposing this publicly, but for now it is defined upon construction and never changed
        private float[] frameTimings;

        public Animation(Texture2D texturein, float frametime, bool looping, int[] sequence = null)
        {
            frameTime = frametime;
            texture = texturein;
            isLooping = looping;
            frameSize = texture.Height;
            frameCount = (int)(texture.Width / frameSize);

            if (sequence == null)  // If no specific sequence was provided, then we just go from left to right all the way
            {
                // It's cool like a list comprehension from python
                frameSequence = (from number in Enumerable.Range(0, frameCount - 1) select number).ToArray();  // Generate the sequence from 0 to frameCount
            }
            else  // Otherwise we used the actual provided animation sequence
            {
                Debug.Assert(sequence.Length <= frameCount);  // Length of sequence should not exceed the number of frames
                frameSequence = sequence;
            }
        }

        // This method is construction in the case custom frame timings are needed
        public Animation(Texture2D texturein, float[] timings, bool looping, int[] sequence = null)
        {
            frameTime = 0;  // Set the frameTime to zero if we are provided timings
            texture = texturein;
            isLooping = looping;
            frameSize = texture.Height;
            frameCount = (int)(texture.Width / frameSize);

            if (sequence == null)  // If no specific sequence was provided, then we just go from left to right all the way
            {
                // It's cool like a list comprehension from python
                frameSequence = (from number in Enumerable.Range(0, frameCount - 1) select number).ToArray();  // Generate the sequence from 0 to frameCount
            }
            else  // Otherwise we used the actual provided animation sequence
            {
                Debug.Assert(sequence.Length <= frameCount);  // Length of sequence should not exceed the number of frames
                frameSequence = sequence;
            }
            Debug.Assert(timings != null);
            Debug.Assert(timings.Length == frameSequence.Length);  // We should have a timing for each frame (including last)

            frameTimings = timings;
        }

        // Timing(frame) will return the frameTime associated with the target frame number, where frame corresponds to a frame in
        //  frameSequence (in terms of index)
        // Requires: 0 <= frame < frameTimings.Length
        public float Timing(int frame)
        {
            Debug.Assert(0 <= frame && frame < frameSequence.Length);
            if (frameTimings != null)  // If we have custom timings, then we return the corresponding timing
            {
                return frameTimings[frame];
            }
            else  // Otherwise, we just return the standard frameTime
            {
                return frameTime;
            }
        }
    }
}
