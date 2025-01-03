 using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

[ExecuteAlways]
public abstract class SingletonComponent<T> : MonoBehaviour where T : MonoBehaviour
{
    static T _instance;

    public virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (_instance != this)
        {
            Destroy(this.gameObject);
        }
    }



    public static T Instance 
    {
        get
        {
            if (_instance != null)
            {
                return _instance;
            }
            else
            {
                _instance = FindAnyObjectByType<T>();
                if (_instance == null)
                {
                    var holder = new GameObject($"{typeof(T).Name}_Singleton", typeof(T));
                    _instance = holder.GetComponent<T>();
                    if(Application.isPlaying)
                    DontDestroyOnLoad( holder.gameObject );
                }
                else
                {
                    if(Application.isPlaying)
                    Destroy(_instance.gameObject);
                    else
                        DestroyImmediate(_instance.gameObject);
                }
                return _instance;
            }
        }
    }
    private void OnValidate()
    {
        // Ensure no duplicates exist before play mode
        var instances = FindObjectsOfType<T>();
        if (instances.Length > 1)
        {
            foreach (var instance in instances)
            {
                if (_instance == null)
                {
                    // Keep the first found instance as the singleton
                    _instance = instance;
                }
                else if (instance != _instance)
                {
                    // Destroy extra instances
                    Debug.LogWarning($"Destroying duplicate singleton instance of {typeof(T).Name}: {instance.gameObject.name}");
                    EditorApplication.delayCall+= () =>
                    {
                        DestroyImmediate(_instance);
                        
                    };
                    break;
                }
            }
        }
    }
}
