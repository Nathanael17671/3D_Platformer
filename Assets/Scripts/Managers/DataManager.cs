using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance; // Static reference to the singleton instance
    [SerializeField][Range(5.0f, 120.0f)]public float sensitivity = 50.0f;
    [SerializeField][Range(0.0f, 100.0f)]public float musicVolume = 50.0f;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject); // Destroy new instances if one already exists
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Keep this object alive
    }
}
