using UnityEngine;
using UnityEditor;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    #region  Properties
    /// <summary>
    /// Returns the instance of this singleton.
    /// </summary>
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (T)FindObjectOfType(typeof(T));
                if (instance == null)
                {
                    Debug.LogError("An instance of " + typeof(T) + " is needed in the scene, but there is none.");
                }
            }
            return instance;
        }
    }
    #endregion

    #region Methods
    protected void Awake()
    {
        if (GetType() != typeof(T))
            DestroySelf();

        if (instance == null)
        {
            instance = this as T;
            //DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
            DestroySelf();
    }

//    protected void OnValidate()
//    {
//        if (GetType() != typeof(T))
//        {
//            Debug.LogError("Singleton<" + typeof(T) + "> has a wrong Type Parameter. " +
//                "Try Singleton<" + GetType() + "> instead.");
//#if UNITY_EDITOR
//            UnityEditor.EditorApplication.delayCall -= DestroySelf;
//            UnityEditor.EditorApplication.delayCall += DestroySelf;
//#endif
//        }

//        if (instance == null)
//            instance = this as T;
//        else if (instance != this)
//        {
//            //is a prefab
//            if (PrefabUtility.GetCorrespondingObjectFromSource(gameObject) == null && PrefabUtility.GetPrefabObject(gameObject) != null)
//                return;
//            Debug.LogError("Singleton<" + GetType() + "> already has an instance on scene. Component will be destroyed.");
//#if UNITY_EDITOR
//            UnityEditor.EditorApplication.delayCall -= DestroySelf;
//            UnityEditor.EditorApplication.delayCall += DestroySelf;
//#endif
//        }
//    }

    private void DestroySelf()
    {
        if (Application.isPlaying)
            Destroy(this);
        else
            DestroyImmediate(this);
    }
    #endregion

    protected static T instance;
}
