using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XnaGamesInfrastructure.ObjectInterfaces
{
    /// <summary>
    /// This delegate is used by ICollidable object to inform it's observer 
    /// a collision occured.
    /// </summary>
    /// <param name="i_Collidable">The IColidable object passes itself to observer</param>
    public delegate void PositionChangedDelegate(ICollidable i_Collidable);

    /// <summary>
    /// Enumeration containing all the available collision check types
    /// </summary>
    public enum eCollidableCheckType
    {
        RectangleCollision,
        PixelCollision
    }

    /// <summary>
    /// Used to inform CollisionManager a collision has occured
    /// </summary>
    public interface ICollidable
    {
        /// <summary>
        /// This method checks whether a collision occured between this 
        /// ICollidable object and the other object.
        /// </summary>
        /// <param name="i_OtherComponent"></param>
        /// <returns></returns>
        bool    CheckForCollision(ICollidable i_OtherComponent);
        
        /// <summary>
        /// Describes ICollidable object's bounds as a rectangle
        /// </summary>
        Rectangle Bounds
        {
            get;
        }
        
        /// <summary>
        /// Defines whether object is visible (used by DrawableGameComponent to hide
        /// object)
        /// </summary>
        bool Visible
        {
            get;
        }

        /// <summary>
        /// Read only property to the component texture color array
        /// </summary>
        Color[] ColorData
        {
            get;
        }

        /// <summary>
        /// Read only property to the component texture
        /// </summary>
        Texture2D Texture
        {
            get;
        }

        /// <summary>
        /// Read only property to the component motion vector
        /// </summary>
        Vector2 MotionVector
        {
            get;
        }

        /// <summary>
        /// Property that states the collision check that the component 
        /// needs
        /// </summary>
        eCollidableCheckType    CollisionCheckType
        {
            get;
        }
        
        /// <summary>
        /// Read only property that marks if the object is in a dying state
        /// </summary>
        bool    Dying
        {
            get;
        }

        /// <summary>
        /// Handles a rectangle collision in a specific way for each ICollidable class.
        /// Called by CollisionManager
        /// </summary>
        /// <param name="i_OtherComponent">The object which caused the collision</param>
        void    Collided(ICollidable i_OtherComponent);

        /// <summary>
        /// Invoked in order to notify observer (CollisionManager) before object 
        /// is disposed 
        /// </summary>
        event EventHandler Disposed;

        /// <summary>
        /// Ivoked in order to notify observer (CollisionManager) when object's 
        /// position was changed
        /// </summary>
        event PositionChangedDelegate PositionChanged;
    }
}
