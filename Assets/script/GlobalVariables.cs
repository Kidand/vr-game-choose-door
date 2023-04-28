using UnityEngine;
using System;

public class GlobalVariables : MonoBehaviour
{
    public static GlobalVariables Instance; // 单例模式，确保只有一个GlobalVariables实例

    public event Action OnValuesAssigned;

    public string message1Result; // 存储颜色的结果
    public int message2Result; // 存储数字的结果

    private int totalScore = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 保持全局变量对象始终存在
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AssignValues(string result, int number)
    {
        message1Result = result;
        message2Result = number;

        OnValuesAssigned?.Invoke();
    }

    public void UpdateTotalScore(int score)
    {
        totalScore = score;

        OnValuesAssigned?.Invoke();
    }
}
