using UnityEngine;

public class Cameras : MonoBehaviour
{
    private void Awake()
    {
        int numCamera = FindObjectsOfType<Cameras>().Length;
        if (numCamera > 1) {
            Destroy(gameObject);
        }
        else {
            DontDestroyOnLoad(gameObject);
        }
    }
}
