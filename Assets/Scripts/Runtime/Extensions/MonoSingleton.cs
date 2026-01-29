namespace RunTime.Extensions
{
    using UnityEngine;

namespace RunTime.Utilities
{

    public abstract class MonoSingleton<T> : MonoBehaviour where T : Component
    {
        #region Private Fields
        
      
        private static T _instance;
        
     
        private static bool _isQuitting = false;
        
        #endregion

        #region Public Properties

        public static T Instance
        {
            get
            {
               
                if (_isQuitting)
                {
                    Debug.LogWarning($"[MonoSingleton] Instance '{typeof(T)}' already destroyed. Returning null.");
                    return null;
                }

               
                if (_instance != null) return _instance;

              
                _instance = FindObjectOfType<T>();
                
                if (_instance != null) return _instance;

            
                GameObject obj = new GameObject();
                obj.name = typeof(T).Name; 
                _instance = obj.AddComponent<T>();
                    
                Debug.Log($"[MonoSingleton] Auto-created instance of {typeof(T)}");

                return _instance;
            }
        }
        
        #endregion

        #region Lifecycle Methods

        protected virtual void Awake()
        {
            if (!Application.isPlaying) return;

         
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

           
            _instance = this as T;
            
          
        }
        
   
        protected virtual void OnApplicationQuit()
        {
           
            _instance = null;
        }

        #endregion
    }
}
}