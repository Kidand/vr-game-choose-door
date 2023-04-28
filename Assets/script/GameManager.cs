using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool isGameStarted = false;
    public List<GameObject> objectsToHideOnStart;
    public string initialScene; // 添加一个变量来存储初始场景名称

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            initialScene = SceneManager.GetActiveScene().name; // 在Awake方法中设置初始场景名称
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [ContextMenu("StartGame")] // 添加这一行
    public void StartGame()
    {
        // 在这里实现游戏开始的逻辑，例如加载场景、初始化游戏对象等
        Debug.Log("Game started!");

    }

    public void LoadObj()
    {
        Debug.Log("Load Game Object");
        isGameStarted = true;

        // 遍历 objectsToHideOnStart 列表并将每个对象设置为非激活状态
        foreach (GameObject obj in objectsToHideOnStart)
        {
            obj.SetActive(false);
        }

        // 获取 DoorSpawner 组件并调用 SpawnDoor() 方法
        // DoorSpawner doorSpawner = FindObjectOfType<DoorSpawner>();
        // if (doorSpawner != null)
        // {
        //     doorSpawner.SpawnDoor();
        // }
        // else
        // {
        //     Debug.LogError("DoorSpawner not found in the scene.");
        // }

        // 获取 LeftTime 组件并启用它
        LeftTime leftTime = FindObjectOfType<LeftTime>();
        if (leftTime != null)
        {
            leftTime.enabled = true;
        }
        else
        {
            Debug.LogError("LeftTime not found in the scene.");
        }
    }
}

