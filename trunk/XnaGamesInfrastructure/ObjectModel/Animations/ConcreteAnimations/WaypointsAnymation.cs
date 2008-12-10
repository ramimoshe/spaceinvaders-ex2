using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace XnaGamesInfrastructure.ObjectModel.Animations.ConcreteAnimations
{
    /// <summary>
    /// A wayport animation that moves a given sprite according to
    /// given positions
    /// </summary>
    public class WaypointsAnymation : SpriteAnimation
    {
        private float m_VelocityPerSecond;
        private Vector2[] m_Waypoints;
        private int m_CurrentWaypoint = 0;
        private bool m_Loop = false;

        public WaypointsAnymation(
            float i_VelocityPerSecond,
            TimeSpan i_AnimationLength,
            bool i_Loop,
            params Vector2[] i_Waypoints)

            : this("Waypoints", i_VelocityPerSecond, i_AnimationLength, i_Loop, i_Waypoints)
        {
        }

        public WaypointsAnymation(
            string i_Name,
            float i_VelocityPerSecond,
            TimeSpan i_AnimationLength,
            bool i_Loop,
            params Vector2[] i_Waypoints)

            : base(i_Name, i_AnimationLength)
        {
            this.m_VelocityPerSecond = i_VelocityPerSecond;
            this.m_Waypoints = i_Waypoints;
            m_Loop = i_Loop;
            m_ResetAfterFinish = false;
        }

        /// <summary>
        /// Resets the animation to original state
        /// </summary>
        /// <param name="i_AnimationLength">Animation length</param>
        public override void    Reset(TimeSpan i_AnimationLength)
        {
            base.Reset(i_AnimationLength);

            this.BoundSprite.PositionOfOrigin = m_OriginalSpriteInfo.PositionForDraw;
        }

        /// <summary>
        /// Perform the animation next move
        /// </summary>
        /// <param name="i_GameTime">A snapshot to the game time</param>
        protected override void     DoFrame(GameTime i_GameTime)
        {
            // This offset is how much we need to move based on how much time 
            // has elapsed.
            float maxDistance = (float)i_GameTime.ElapsedGameTime.TotalSeconds * m_VelocityPerSecond;

            // The vector that is left to get to the current waypoint
            Vector2 remainingVector = m_Waypoints[m_CurrentWaypoint] - this.BoundSprite.PositionForDraw;
            if (remainingVector.Length() > maxDistance)
            {
                // The vector is longer than we can travel,
                // so limit to our maximum travel distance
                remainingVector.Normalize();
                remainingVector *= maxDistance;
            }

            // Move
            this.BoundSprite.PositionOfOrigin += remainingVector;

            if (reachedCurrentWaypoint())
            {
                LookAtNextWayPoint();
            }
        }

        /// <summary>
        /// Checks the next waypoint we need to move the sprite to, in case
        /// it's the last one and the animation is finite will finish
        /// </summary>
        private void    LookAtNextWayPoint()
        {
            if (onLastWaypoint() && !m_Loop)
            {
                // No more waypoints, so this animation is finished
                this.IsFinished = true;
            }
            else
            {
                // We have more waypoints to go. NEXT!
                m_CurrentWaypoint++;
                m_CurrentWaypoint %= m_Waypoints.Length;
            }
        }

        /// <summary>
        /// Return an indication whether we reached the last waypoint
        /// </summary>
        /// <returns>Indication whether we reached the last way point</returns>
        private bool    onLastWaypoint()
        {
            return m_CurrentWaypoint == m_Waypoints.Length - 1;
        }

        /// <summary>
        /// Return an indication whether we reached the next way point
        /// </summary>
        /// <returns>Indication whether we reached the next way point</returns>
        private bool    reachedCurrentWaypoint()
        {
            return this.BoundSprite.PositionForDraw == m_Waypoints[m_CurrentWaypoint];
        }
    }
}
