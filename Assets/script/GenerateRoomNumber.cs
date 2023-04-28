using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GenerateRoomNumber : MonoBehaviour
{
    public TextMeshProUGUI[] numbers; // 在Unity编辑器中为这个数组分配4个TextMeshProUGUI对象
    public GameObject[] doors; // 在Unity编辑器中为这个数组分配4个门对象


    private void Start()
    {
        if (GlobalVariables.Instance != null)
        {
            GlobalVariables.Instance.OnValuesAssigned += GenerateNumbers;
        }
        else
        {
            Debug.LogError("GlobalVariables instance not found.");
        }
    }

    private void OnDestroy()
    {
        if (GlobalVariables.Instance != null)
        {
            GlobalVariables.Instance.OnValuesAssigned -= GenerateNumbers;
        }
    }

    private void GenerateNumbers()
    {
        List<int> availableNumbers = new List<int> { 0, 1, 2, 3 };
        List<Color> availableColors = new List<Color> { Color.red, Color.yellow, Color.blue, Color.green };

        Dictionary<string, Color> colorMap = new Dictionary<string, Color>
        {
            { "red", Color.red },
            { "yellow", Color.yellow },
            { "blue", Color.blue },
            { "green", Color.green }
        };

        // 随机分配数字和颜色
        for (int i = 0; i < 3; i++) // 只生成前三个数字
        {
            int randomNumIndex = Random.Range(0, availableNumbers.Count);
            int randomNumber = availableNumbers[randomNumIndex];

            int randomColorIndex = Random.Range(0, availableColors.Count);
            Color randomColor = availableColors[randomColorIndex];

            // 如果随机颜色与GlobalVariables提供的颜色相同，确保数字不同
            if (randomColor == colorMap[GlobalVariables.Instance.message1Result] && randomNumber == GlobalVariables.Instance.message2Result)
            {
                availableNumbers.RemoveAt(randomNumIndex);
                randomNumIndex = Random.Range(0, availableNumbers.Count);
                randomNumber = availableNumbers[randomNumIndex];
            }

            // 如果随机颜色与GlobalVariables提供的颜色不同，则从列表中移除
            if (randomColor != colorMap[GlobalVariables.Instance.message1Result])
            {
                availableColors.RemoveAt(randomColorIndex);
            }

            availableNumbers.RemoveAt(randomNumIndex); // 移除已经使用的数字

            numbers[i].text = randomNumber.ToString();
            numbers[i].color = randomColor;

            doors[i].name = $"Door_{randomNumber}_{randomColor}"; // 给每个门分配一个唯一的名字
        }

        // 从GlobalVariables脚本中获取第四个数字的值和颜色
        numbers[3].text = GlobalVariables.Instance.message2Result.ToString();
        numbers[3].color = colorMap[GlobalVariables.Instance.message1Result];

        // 随机调整数字的顺序
        for (int i = 0; i < numbers.Length; i++)
        {
            int randomIndex = Random.Range(0, numbers.Length);
            TextMeshProUGUI temp = numbers[i];
            numbers[i] = numbers[randomIndex];
            numbers[randomIndex] = temp;
        }
    }
}
