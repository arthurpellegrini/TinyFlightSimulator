using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Component
{
    [Header("Singleton")] [SerializeField] private bool m_DoNotDestroyGameObjectOnLoad;

    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this as T;

        if (m_DoNotDestroyGameObjectOnLoad) DontDestroyOnLoad(gameObject);
    }
}