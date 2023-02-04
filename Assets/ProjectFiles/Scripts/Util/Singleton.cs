using UnityEngine;


public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T m_instance;
    public static T Instance
    {
        get
        {
            if (m_instance != null)
            {
                m_instance = FindObjectOfType<T>();
            }

            return m_instance;
        }
        private set
        {
            m_instance = value;
        }
    }


    protected virtual void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this as T;
        }
        else if (m_instance != this)
        {
            Destroy(this.gameObject);

            return;
        }
    }


    protected virtual void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
