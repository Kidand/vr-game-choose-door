using UnityEngine;

public class KeepButtonOnLoad : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        // 获取所有子物体并在它们上面调用 DontDestroyOnLoad()
        foreach (Transform child in transform.GetComponentsInChildren<Transform>())
        {
            DontDestroyOnLoad(child.gameObject);
        }
    }
}
