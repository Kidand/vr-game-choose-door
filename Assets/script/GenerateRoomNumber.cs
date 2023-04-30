using System.Collections.Generic;
using TMPro;
using UnityEngine;
using HurricaneVR.Framework.Components;

public class GenerateRoomNumber : MonoBehaviour
{
    public TextMeshProUGUI[] numbers; // 在Unity编辑器中为这个数组分配4个TextMeshProUGUI对象
    public GameObject[] doors; // 在Unity编辑器中为这个数组分配4个门对象
    private Dictionary<int, TextMeshProUGUI> doorNumberMapping;
    public GameObject[] cubes; // 在Unity编辑器中为这个数组分配4个立方体对象

    private void Start()
    {
        doorNumberMapping = new Dictionary<int, TextMeshProUGUI>();
        if (GlobalVariables.Instance != null)
        {
            GlobalVariables.Instance.OnValuesAssigned += GenerateNumbers;
        }
        else
        {
            Debug.LogError("GlobalVariables instance not found.");
        }

        for (int i = 0; i < doors.Length; i++)
        {
            HVRPhysicsDoor tracker = doors[i].GetComponent<HVRPhysicsDoor>();
            int capturedIndex = i; // 创建一个局部变量并捕获循环变量的当前值
            if (tracker)
            {
                tracker.DoorOpenedE.AddListener(() => OnDoorOpened(capturedIndex));
                tracker.DoorClosedE.AddListener(() => OnDoorClosed(capturedIndex));
            }
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

        // // 随机分配数字和颜色
        // for (int i = 0; i < 3; i++) // 只生成前三个数字
        // {
        //     int randomNumIndex = Random.Range(0, availableNumbers.Count);
        //     int randomNumber = availableNumbers[randomNumIndex];

        //     int randomColorIndex = Random.Range(0, availableColors.Count);
        //     Color randomColor = availableColors[randomColorIndex];

        //     // 如果随机颜色与GlobalVariables提供的颜色相同，确保数字不同
        //     if (randomColor == colorMap[GlobalVariables.Instance.message1Result] && randomNumber == GlobalVariables.Instance.message2Result)
        //     {
        //         availableNumbers.RemoveAt(randomNumIndex);
        //         randomNumIndex = Random.Range(0, availableNumbers.Count);
        //         randomNumber = availableNumbers[randomNumIndex];
        //     }

        //     // 如果随机颜色与GlobalVariables提供的颜色不同，则从列表中移除
        //     if (randomColor != colorMap[GlobalVariables.Instance.message1Result])
        //     {
        //         availableColors.RemoveAt(randomColorIndex);
        //     }

        //     availableNumbers.RemoveAt(randomNumIndex); // 移除已经使用的数字

        //     numbers[i].text = randomNumber.ToString();
        //     numbers[i].color = randomColor;
        // }

        // // 从GlobalVariables脚本中获取第四个数字的值和颜色
        // numbers[3].text = GlobalVariables.Instance.message2Result.ToString();
        // numbers[3].color = colorMap[GlobalVariables.Instance.message1Result];

        // 随机分配数字和颜色
        List<int> indices = new List<int> { 0, 1, 2, 3 }; // 创建索引列表
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

            int randomIndex = Random.Range(0, indices.Count); // 从索引列表中随机选择一个索引
            int currentIndex = indices[randomIndex];
            indices.RemoveAt(randomIndex); // 从索引列表中移除已经使用的索引

            numbers[currentIndex].text = randomNumber.ToString();
            numbers[currentIndex].color = randomColor;
        }

        // 从GlobalVariables脚本中获取第四个数字的值和颜色
        int finalIndex = indices[0]; // 获取剩余的索引
        numbers[finalIndex].text = GlobalVariables.Instance.message2Result.ToString();
        numbers[finalIndex].color = colorMap[GlobalVariables.Instance.message1Result];

        // 随机调整数字的顺序
        // ShuffleNumbers(numbers);

        // 在调整顺序后重新设置doorNumberMapping映射
        for (int i = 0; i < numbers.Length; i++)
        {
            doorNumberMapping[i] = numbers[i];
        }


        // 设置立方体的颜色
        for (int i = 0; i < cubes.Length; i++)
        {
            if (IsCorrectDoor(i))
            {
                // 如果立方体对应于正确的门，则将其颜色设置为绿色
                cubes[i].GetComponent<Renderer>().material.color = Color.green;
            }
            else
            {
                // 否则，将立方体颜色设置为红色
                cubes[i].GetComponent<Renderer>().material.color = Color.red;
            }
        }
    }

    private void OnDoorOpened(int doorIndex)
    {
        
        Debug.Log("-------------------------------");
        Debug.Log($"{doorIndex}");
        Debug.Log("-------------------------------");
        if (IsCorrectDoor(doorIndex))
        {
            // 如果门是正确的，执行正确的门打开操作
            Debug.Log("Correct door opened.");
        }
        else
        {
            // 如果门是错误的，执行错误的门打开操作
            Debug.Log("Wrong door opened.");
        }
    }

    private void OnDoorClosed(int doorIndex)
    {
        if (IsCorrectDoor(doorIndex))
        {
            // 如果门是正确的，执行正确的门关闭操作
            Debug.Log("Correct door closed.");
            LeftTime leftTime = FindObjectOfType<LeftTime>();
            if (leftTime)
            {
                leftTime.AddScore(10); // 增加分数
                // leftTime.AddHeart(); // 增加红心
            }
        }
        else
        {
            Debug.Log("Wrong door closed.");
            LeftTime leftTime = FindObjectOfType<LeftTime>();
            if (leftTime)
            {
                leftTime.ReduceHeart(); // 扣除红心
            }
        }
    }

    private bool IsCorrectDoor(int doorIndex)
    {
        // 根据门的索引和GlobalVariables中的数字和颜色来判断是否是正确的门
        if (doorNumberMapping.TryGetValue(doorIndex, out TextMeshProUGUI numberText))
        {
            int number = int.Parse(numberText.text);
            Color color = numberText.color;

            Debug.Log("Opened/Closed Door: Number: " + number + ", Color: " + color + ", Index: " + doorIndex);
            Debug.Log("Expected Correct Door: Number: " + GlobalVariables.Instance.message2Result + ", Color: " + GlobalVariables.Instance.GetColorByName(GlobalVariables.Instance.message1Result));

            if (number == GlobalVariables.Instance.message2Result &&
                color == GlobalVariables.Instance.GetColorByName(GlobalVariables.Instance.message1Result))
            {
                return true;
            }
        }

        return false;
    }

    private void ShuffleNumbers(TextMeshProUGUI[] numbers)
    {
        for (int i = numbers.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            TextMeshProUGUI temp = numbers[i];
            numbers[i] = numbers[randomIndex];
            numbers[randomIndex] = temp;
        }
    }

}
