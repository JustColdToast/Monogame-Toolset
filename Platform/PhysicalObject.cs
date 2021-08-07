using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace Platform
    // Kanwarpal Brar
    // This class represents a basic physical object. All physical objects have an attached Animation (a physical object needs an appearance)
    //  as well as Physics-based parameters: velocity, acceleration, position.
    //  Most important is the animation/hitbox based collision functionality: a physical object should be able to tell
    //  when it is touching another physical object

    // Collision detection is based off of Hitbox
    //  -- note -- There is potential for future expansion of collision based on a constantly updated "collision animation frame"
    //   where instead we check if two animation frames are overlapping
    
    // The intended use case is that this object is inherited from and customized depending on what type of physical object is needed
    //  Ex: A Player is a Physical Object, but would have other fields (such as different animations)
{
    class PhysicalObject
    {
        // Physics/Position attributes
        private Vector2 moveAcceleration = new Vector2(0,0);
        private Vector2 maxMoveSpeed;
        private Vector2 velocity = new Vector2(0, 0);
        private Vector2 position;

        // Basic Physical Appearance attributes
        private Animation currentAnimation;  // Note, the collision bounding box is based on currentAnimation

        // Collision attributes
        private Rectangle boxCollider;  // Box Collider should be recalculated on each change of currentAnimation
        private bool isGrounded;

        // Physical Object constructor
        public PhysicalObject(Vector2 startPos, Vector2 maxSpeed, Animation startAnimation)
        {
            // TODO constructor
        }
    }
}
