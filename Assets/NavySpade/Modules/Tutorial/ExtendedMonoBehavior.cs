using System.Collections.Generic;
using System.Linq;
using NavySpade.Modules.Extensions.UnityTypes;


namespace NavySpade.Modules.Tutorial{
    public class ExtendedMonoBehavior<T> : ExtendedMonoBehavior where T : ExtendedMonoBehavior<T>
    {
        #region ecs

        public static T Instance => All.FirstOrDefault();

        public static HashSet<T> All { get; private set; } = new HashSet<T>();
        public static HashSet<T> Active { get; private set; } = new HashSet<T>();

        protected virtual void Awake()
        {
            if (All == null)
                All = new HashSet<T>();

            All.Add((T) this);
        }

        protected virtual void OnEnable()
        {
            if (Active == null)
                Active = new HashSet<T>();
            
            Active.Add((T) this);
        }

        protected virtual void OnDisable()
        {
            Active.Remove((T) this);
        }

        protected virtual void OnDestroy()
        {
            All.Remove((T) this);
        }

        #endregion
    }
}