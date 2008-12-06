using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace XnaGamesInfrastructure.ObjectModel.Animations.ConcreteAnimations
{
    public class WaypointsAnymation : SpriteAnimation
    {
        private float m_VelocityPerSecond;
        private Vector2[] m_Waypoints;
        private int m_CurrentWaypoint = 0;
        private bool m_Loop = false;

        // CTORs
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

        public override void Reset(TimeSpan i_AnimationLength)
        {
            base.Reset(i_AnimationLength);

            this.BoundSprite.PositionOfOrigin = m_OriginalSpriteInfo.PositionForDraw;
        }

        protected override void DoFrame(GameTime i_GameTime)
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

            if (ReachedCurrentWaypoint())
            {
                LookAtNextWayPoint();
            }
        }

        private void LookAtNextWayPoint()
        {
            if (OnLastWaypoint() && !m_Loop)
            {
                // No more waypoints, so this animation is finished
                base.IsFinished = true;
            }
            else
            {
                // We have more waypoints to go. NEXT!
                m_CurrentWaypoint++;
                m_CurrentWaypoint %= m_Waypoints.Length;
            }
        }

        private bool OnLastWaypoint()
        {
            return (m_CurrentWaypoint == m_Waypoints.Length - 1);
        }

        private bool ReachedCurrentWaypoint()
        {
            return (this.BoundSprite.PositionForDraw == m_Waypoints[m_CurrentWaypoint]);
        }
    }
}
