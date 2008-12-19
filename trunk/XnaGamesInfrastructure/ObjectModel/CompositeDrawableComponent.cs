using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace XnaGamesInfrastructure.ObjectModel
{
    /// <summary>
    /// A class to assist with being able to nest game components inside of each other, provides support for all of the
    /// same functionality the game object performs on components with the addition of being neutral to where it resides
    /// in the hierarchy.
    /// </summary>
    public abstract class CompositeDrawableComponent<ComponentType> : 
        DrawableGameComponent, ICollection<ComponentType>
        where ComponentType : class, IGameComponent
    {
        // the entire collection, for general collection methods (count, foreach, etc.):
        private Collection<ComponentType> m_Components = new Collection<ComponentType>();

        // selective holders for specific operations each frame:
        private List<ComponentType> m_UninitializedComponents = new List<ComponentType>();
        protected List<IUpdateable> m_UpdateableComponents = new List<IUpdateable>();
        protected List<IDrawable> m_DrawableComponents = new List<IDrawable>();
        protected List<Sprite> m_Sprites = new List<Sprite>();
        
        public event EventHandler<GameComponentEventArgs<ComponentType>> ComponentAdded;

        public event EventHandler<GameComponentEventArgs<ComponentType>> ComponentRemoved;

        /// <summary>
        /// Called when a new component is added to m_Components, to handle 
        /// insertions for additional collections
        /// </summary>
        /// <param name="e">Argument including the removed component</param>
        protected virtual void  OnComponentAdded(GameComponentEventArgs<ComponentType> e)
        {
            if (m_IsInitialized)
            {
                InitializeComponent(e.GameComponent);
            }
            else
            {
                m_UninitializedComponents.Add(e.GameComponent);
            }

            // If the new component implements IUpdateable:
            // 1. find a spot for it on the updateable list 
            // 2. hook it's UpdateOrderChanged event
            IUpdateable updatable = e.GameComponent as IUpdateable;
            if (updatable != null)
            {
                if (InsertSorted(updatable))
                {
                    updatable.UpdateOrderChanged += new EventHandler(ChildUpdateOrderChanged);
                }
            }

            // If the new component implements IDrawable:
            // 1. find a spot for it on the drawable lists (IDrawble/Sprite) 
            // 2. hook it's DrawOrderChanged event
            IDrawable drawable = e.GameComponent as IDrawable;
            if (drawable != null)
            {
                if (InsertSorted(drawable))
                {
                    drawable.DrawOrderChanged += new EventHandler(ChildDrawOrderChanged);
                }
            }

            // raise the Added event:
            if (ComponentAdded != null)
            {
                ComponentAdded(this, e);
            }
        }

        /// <summary>
        /// Called when a new component is removed from m_Components, to handle 
        /// removal of component from additional collections
        /// </summary>
        /// <param name="e">Argument including the removed component</param>
        protected virtual void  OnComponentRemoved(GameComponentEventArgs<ComponentType> e)
        {
            if (!m_IsInitialized)
            {
                m_UninitializedComponents.Remove(e.GameComponent);
            }

            IUpdateable updatable = e.GameComponent as IUpdateable;
            if (updatable != null)
            {
                m_UpdateableComponents.Remove(updatable);
                updatable.UpdateOrderChanged -= ChildUpdateOrderChanged;
            }

            Sprite sprite = e.GameComponent as Sprite;
            if (sprite != null)
            {
                m_Sprites.Remove(sprite);
                sprite.DrawOrderChanged -= ChildDrawOrderChanged;
            }
            else
            {
                IDrawable drawable = e.GameComponent as IDrawable;
                if (drawable != null)
                {
                    m_DrawableComponents.Remove(drawable);
                    drawable.DrawOrderChanged -= ChildDrawOrderChanged;
                }
            }

            // raise the Removed event:
            if (ComponentRemoved != null)
            {
                ComponentRemoved(this, e);
            }
        }

        /// <summary>
        /// When the update order of a component in this manager changes, will need to find a new place for it
        /// on the list of updateable components.
        /// </summary>
        private void    ChildUpdateOrderChanged(object sender, EventArgs e)
        {
            IUpdateable updatable = sender as IUpdateable;
            m_UpdateableComponents.Remove(updatable);

            InsertSorted(updatable);
        }

        /// <summary>
        /// When the draw order of a component in this manager changes, will need to find a new place for it
        /// on the list of drawable components.
        /// </summary>
        private void    ChildDrawOrderChanged(object sender, EventArgs e)
        {
            IDrawable drawable = sender as IDrawable;

            Sprite sprite = sender as Sprite;
            if (sprite != null)
            {
                m_Sprites.Remove(sprite);
            }
            else
            {
                m_DrawableComponents.Remove(drawable);
            }

            InsertSorted(drawable);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="i_Game">The hosting game</param>
        public  CompositeDrawableComponent(Game i_Game)
            : base(i_Game)
        {
        }

        /// <summary>
        /// Inserts a new updatable component to updateable component using a sort
        /// </summary>
        /// <param name="i_Updatable">The new component</param>
        /// <returns>True, if component is indeed new, else false</returns>
        private bool    InsertSorted(IUpdateable i_Updatable)
        {
            bool inserted = false;
            int index = m_UpdateableComponents.BinarySearch(i_Updatable, UpdateableComparer.Default);
            if (index < 0)
            {
                m_UpdateableComponents.Insert(~index, i_Updatable);
                inserted = true;
            }

            return inserted;
        }

        /// <summary>
        /// Inserts a new updatable component to Drawable component using a sort
        /// </summary>
        /// <param name="i_Updatable">The new component</param>
        /// <returns>True, if component is indeed new, else false</returns>
        private bool    InsertSorted(IDrawable i_Drawable)
        {
            bool inserted = false;
            Sprite sprite = i_Drawable as Sprite;
            if (sprite != null)
            {
                int index = m_Sprites.BinarySearch(sprite, DrawableComparer<Sprite>.Default);
                if (index < 0)
                {
                    m_Sprites.Insert(~index, sprite);
                    inserted = true;
                }
            }
            else
            {
                int index = m_DrawableComponents.BinarySearch(i_Drawable, DrawableComparer<IDrawable>.Default);
                if (index < 0)
                {
                    m_DrawableComponents.Insert(~index, i_Drawable);
                    inserted = true;
                }
            }

            return inserted;
        }

        private bool m_IsInitialized;

        /// <summary>
        /// initialize any component that haven't been initialized yet
        /// and remove it from the list of uninitialized components
        /// </summary>
        public override void    Initialize()
        {
            if (!m_IsInitialized)
            {
                // Initialize any un-initialized game components
                while (m_UninitializedComponents.Count > 0)
                {
                    InitializeComponent(m_UninitializedComponents[0]);
                }

                base.Initialize();

                m_IsInitialized = true;
            }
        }

        /// <summary>
        /// This method initializes an un-initialized component contained in 
        /// this component.
        /// </summary>
        /// <param name="i_Component">The un-initialized component</param>
        private void    InitializeComponent(ComponentType i_Component)
        {
            if (i_Component is Sprite)
            {
                (i_Component as Sprite).SpriteBatch = m_SpriteBatch;
            }

            i_Component.Initialize();
            m_UninitializedComponents.Remove(i_Component);
        }

        /// <summary>
        /// This method loads the content for this composite component, and it's sub-sprites,
        /// and initialize a new SpriteBatch, for used by each one of these sprites
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            m_SpriteBatch = new SpriteBatch(this.GraphicsDevice);

            foreach (Sprite sprite in m_Sprites)
            {
                sprite.SpriteBatch = m_SpriteBatch;
            }
        }

        /// <summary>
        /// Overrides default Update behaviour performs update of all child components
        /// </summary>
        /// <param name="gameTime">time since last call</param>
        public override void    Update(GameTime gameTime)
        {
            for (int i = 0; i < m_UpdateableComponents.Count; i++)
            {
                IUpdateable updatable = m_UpdateableComponents[i];
                if (updatable.Enabled)
                {
                    updatable.Update(gameTime);
                }
            }
        }

        /// <summary>
        /// Draws all drawable child components using shared SpriteBatch
        /// </summary>
        /// <param name="gameTime">time since last call</param>
        public override void    Draw(GameTime gameTime)
        {
            foreach (IDrawable drawable in m_DrawableComponents)
            {
                if (drawable.Visible)
                {
                    drawable.Draw(gameTime);
                }
            }

            m_SpriteBatch.Begin(m_SpritesBlendMode, m_SpritesSortMode, SaveStateMode.SaveState, m_TransformationMatrix);
            foreach (Sprite sprite in m_Sprites)
            {
                if (sprite.Visible)
                {
                    sprite.Draw(gameTime);
                }
            }

            m_SpriteBatch.End();
        }

        /// <summary>
        /// Performs dispose of all sub-components
        /// </summary>
        /// <param name="disposing">Tells whther object is to be disposed</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Dispose of components in this manager
                for (int i = 0; i < Count; i++)
                {
                    IDisposable disposable = m_Components[i] as IDisposable;
                    if (disposable != null)
                    {
                        disposable.Dispose();
                    }
                }
            }

            base.Dispose(disposing);
        }

        #region ICollection<ComponentType> Implementations
        
        /// <summary>
        /// Adds a new component to Composite coponent
        /// </summary>
        /// <param name="i_Component">The new component</param>
        public virtual void Add(ComponentType i_Component)
        {
            this.InsertItem(m_Components.Count, i_Component);
        }

        /// <summary>
        /// Inserts a new item to the component at a specified index
        /// </summary>
        /// <param name="i_Idx">Item index</param>
        /// <param name="i_Component">New Component</param>
        protected virtual void  InsertItem(int i_Idx, ComponentType i_Component)
        {
            if (m_Components.IndexOf(i_Component) != -1)
            {
                throw new ArgumentException("Duplicate components are not allowed in the same GameComponentManager.");
            }

            if (i_Component != null)
            {
                m_Components.Insert(i_Idx, i_Component);

                OnComponentAdded(new GameComponentEventArgs<ComponentType>(i_Component));
            }
        }

        /// <summary>
        /// Clears all sub-components
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < Count; i++)
            {
                OnComponentRemoved(new GameComponentEventArgs<ComponentType>(m_Components[i]));
            }

            m_Components.Clear();
        }

        /// <summary>
        /// Checks if composite component cotains the specifed compnent
        /// </summary>
        /// <param name="i_Component">the specifed compnent</param>
        /// <returns>True if component is contained, else false</returns>
        public bool Contains(ComponentType i_Component)
        {
            return m_Components.Contains(i_Component);
        }

        /// <summary>
        /// Copies a component to a specified index
        /// </summary>
        /// <param name="io_ComponentsArray">the specifed compnent</param>
        /// <param name="i_ArrayIndex">item index</param>
        public void CopyTo(ComponentType[] io_ComponentsArray, int i_ArrayIndex)
        {
            m_Components.CopyTo(io_ComponentsArray, i_ArrayIndex);
        }

        /// <summary>
        /// Returns item count
        /// </summary>
        public int  Count
        {
            get 
            { 
                return m_Components.Count; 
            }
        }

        /// <summary>
        /// Returns if read only
        /// </summary>
        public bool IsReadOnly
        {
            get 
            { 
                return false; 
            }
        }

        /// <summary>
        /// Removes a component from composite component
        /// </summary>
        /// <param name="i_Component">the specifed compnent</param>
        /// <returns>true, if component exists in composite component</returns>
        public virtual bool Remove(ComponentType i_Component)
        {
            bool removed = m_Components.Remove(i_Component);

            if (i_Component != null && removed)
            {
                OnComponentRemoved(new GameComponentEventArgs<ComponentType>(i_Component));
            }

            return removed;
        }

        /// <summary>
        /// Gets collection enumerator
        /// </summary>
        /// <returns>Enumrator</returns>
        public IEnumerator<ComponentType>   GetEnumerator()
        {
            return m_Components.GetEnumerator();
        }

        /// <summary>
        /// Gets collection enumerator
        /// </summary>
        /// <returns>Enumrator</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)m_Components).GetEnumerator();
        }

        #endregion ICollection<ComponentType> Implementations

        #region SpriteBatch Advanced Support
        protected SpriteBatch m_SpriteBatch;

        /// <summary>
        /// Gets / Sets SpriteBatch
        /// </summary>
        public SpriteBatch  SpriteBatch
        {
            get
            {
                return m_SpriteBatch;
            }

            set 
            { 
                m_SpriteBatch = value; 
            }
        }

        /// <summary>
        /// Gets / sets composite component transformation matrix
        /// </summary>
        protected Matrix m_TransformationMatrix = Matrix.Identity;

        public Matrix   TransformationMatrix
        {
            get 
            { 
                return m_TransformationMatrix; 
            }

            set 
            { 
                m_TransformationMatrix = value; 
            }
        }

        protected SpriteBlendMode m_SpritesBlendMode = SpriteBlendMode.AlphaBlend;

        /// <summary>
        /// Gets / Sets SpriteBlendMode for composite component
        /// </summary>
        public SpriteBlendMode  SpritesBlendMode
        {
            get 
            { 
                return m_SpritesBlendMode; 
            }

            set 
            { 
                m_SpritesBlendMode = value; 
            }
        }

        protected SpriteSortMode m_SpritesSortMode = SpriteSortMode.Deferred;

        /// <summary>
        /// Gets / Sets SpriteSortMode
        /// </summary>
        public SpriteSortMode   SpritesSortMode
        {
            get 
            { 
                return m_SpritesSortMode; 
            }

            set 
            { 
                m_SpritesSortMode = value; 
            }
        }
        #endregion SpriteBatch Advanced Support

        #region Helping Properties

        /// <summary>
        /// Gets viewport center vector
        /// </summary>
        protected Vector2   CenterOfViewPort
        {
            get
            {
                return new Vector2((float)Game.GraphicsDevice.Viewport.Width / 2, (float)Game.GraphicsDevice.Viewport.Height / 2);
            }
        }

        /// <summary>
        /// Gets the content manager
        /// </summary>
        public ContentManager   ContentManager
        {
            get 
            { 
                return this.Game.Content; 
            }
        }
        #endregion Helping Properties
    }

    /// <summary>
    /// A comparer designed to assist with sorting IUpdateable interfaces.
    /// </summary>
    public sealed class UpdateableComparer : IComparer<IUpdateable>
    {
        /// <summary>
        /// A static copy of the comparer to avoid the GC.
        /// </summary>
        public static readonly UpdateableComparer Default;

        /// <summary>
        /// Default CTOR initializes single instance
        /// </summary>
        static  UpdateableComparer() 
        { 
            Default = new UpdateableComparer(); 
        }

        /// <summary>
        /// Private CTOR aviods class instantiation
        /// </summary>
        private UpdateableComparer() 
        { 
        }

        /// <summary>
        /// Compares x and y
        /// </summary>
        /// <param name="x">First IUpdatable</param>
        /// <param name="y">Second IUpdatable</param>
        /// <returns>0 if equal, 1 if x > y, -1 if x < y</returns>
        public int  Compare(IUpdateable x, IUpdateable y)
        {
            const int k_XBigger = 1;
            const int k_Equal = 0;
            const int k_YBigger = -1;

            int retCompareResult = k_YBigger;

            if (x == null && y == null)
            {
                retCompareResult = k_Equal;
            }
            else if (x != null)
            {
                if (y == null)
                {
                    retCompareResult = k_XBigger;
                }
                else if (x.Equals(y))
                {
                    return k_Equal;
                }
                else if (x.UpdateOrder > y.UpdateOrder)
                {
                    return k_XBigger;
                }
            }

            return retCompareResult;
        }
    }

    /// <summary>
    /// A comparer designed to assist with sorting IDrawable interfaces.
    /// </summary>
    public sealed class DrawableComparer<TDrawble> : IComparer<TDrawble>
        where TDrawble : class, IDrawable
    {
        /// <summary>
        /// A static copy of the comparer to avoid the GC.
        /// </summary>
        public static readonly DrawableComparer<TDrawble> Default;

        /// <summary>
        /// Static CTOR - creates single instance
        /// </summary>
        static  DrawableComparer() 
        { 
            Default = new DrawableComparer<TDrawble>(); 
        }

        /// <summary>
        /// Private CTOR - prevents Instantiation
        /// </summary>
        private DrawableComparer() 
        { 
        }

        #region IComparer<IDrawable> Members

        /// <summary>
        /// Compares x and y drawable components
        /// </summary>
        /// <param name="x">First TDrawable</param>
        /// <param name="y">Second  TDrawable</param>
        /// <returns>0 if eqal, 1 if x > y, -1 if x < y</returns>
        public int  Compare(TDrawble x, TDrawble y)
        {
            const int k_XBigger = 1;
            const int k_Equal = 0;
            const int k_YBigger = -1;

            int retCompareResult = k_YBigger;

            if (x == null && y == null)
            {
                retCompareResult = k_Equal;
            }
            else if (x != null)
            {
                if (y == null)
                {
                    retCompareResult = k_XBigger;
                }
                else if (x.Equals(y))
                {
                    return k_Equal;
                }
                else if (x.DrawOrder > y.DrawOrder)
                {
                    return k_XBigger;
                }
            }

            return retCompareResult;
        }

        #endregion
    }

    /// <summary>
    /// Arguments used with events from the GameComponentCollection.
    /// </summary>
    /// <typeparam name="ComponentType"></typeparam>
    public class GameComponentEventArgs<ComponentType> : EventArgs
        where ComponentType : class, IGameComponent
    {
        private ComponentType m_Component;

        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="gameComponent">Component raising the arguments</param>
        public  GameComponentEventArgs(ComponentType gameComponent)
        {
            m_Component = gameComponent;
        }

        /// <summary>
        /// Gets game component
        /// </summary>
        public  ComponentType    GameComponent
        {
            get { return m_Component; }
        }
    }
}