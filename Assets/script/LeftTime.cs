using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI; // 引入UI命名空间

public class LeftTime : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
     public TextMeshProUGUI message1; // 添加message1的引用
    public TextMeshProUGUI message2; // 添加message2的引用
    public Image[] hearts; // 创建一个Image类型的数组，用于存储红心对象
    private int remainingHearts; // 创建一个整数变量，用于存储剩余的红心数量
    private float timer = 10f;

    public Vector3 initialPosition; // 用于存储TechDemoXRRig的初始位置
    public string initialScene; // 添加一个变量来存储初始场景名称

    public TextMeshProUGUI scoreText;
    private int score = 0;



    private void Start()
    {
        remainingHearts = hearts.Length;
        initialScene = SceneManager.GetActiveScene().name; // 在Start方法中设置初始场景名称
        SetNewMessages(); 
    }

    void Update()
    {
        if (GameManager.Instance.isGameStarted)
        {
            float previousTime = Mathf.FloorToInt(timer); // 保存上一帧的秒数
            timer -= Time.deltaTime;
            int seconds = Mathf.Clamp(Mathf.FloorToInt(timer), 0, 30);
            textMeshPro.text = $"{seconds}s";

            if (previousTime > seconds) // 当整数秒减少时
            {
                if (seconds == 0 && remainingHearts > 0) // 如果秒数为0并且还有剩余的红心
                {
                    remainingHearts--;
                    hearts[remainingHearts].enabled = false;
                    timer = 10f;
                    SetNewMessages(); // 在倒计时重置时设置message1和message2
                }

                if (remainingHearts == 0) // 如果没有剩余的红心
                {
                    Debug.Log("All hearts are gone, resetting the scene.");
                    // 重置整个场景，例如加载当前场景
                    SceneManager.LoadScene(GameManager.Instance.initialScene); // 加载初始场景
                    GameManager.Instance.isGameStarted = false;
                    // 在场景重置后，重新激活开始按钮和其他对象
                    ShowStartButtonAndObjects();
                    this.enabled = false; // 禁用LeftTime脚本
                }
            }
        }
    }
    


    private void SetNewMessages()
    {
        string[] colors = { "red", "green", "yellow", "blue" };
        Color[] colorsValues = { Color.red, Color.green, Color.yellow, Color.blue }; // 定义颜色值数组
        int randomColorIndex = Random.Range(0, colors.Length);
        message1.text = colors[randomColorIndex]; // 随机设置message1的文本
        message1.color = colorsValues[randomColorIndex]; // 设置message1的颜色与文本相对应
        // GlobalVariables.Instance.message1Result = colors[randomColorIndex]; // 存储message1的结果到全局变量

        int randomNum = Random.Range(10, 100); // 生成一个10到99之间的随机数
        message2.text = $"{randomNum} % 4 = ?";
        // GlobalVariables.Instance.message2Result = randomNum % 4; // 计算结果并存储到全局变量

        // 调用GlobalVariables的AssignValues方法并传入结果
        GlobalVariables.Instance.AssignValues(colors[randomColorIndex], randomNum % 4);
    }

    public void ShowStartButtonAndObjects()
    {
        // 通过GameManager的实例访问objectsToHideOnStart列表
        foreach (GameObject obj in GameManager.Instance.objectsToHideOnStart)
        {
            obj.SetActive(true);
        }
    }

    public void UpdateScore(bool isCorrectDoor)
    {
        if (isCorrectDoor)
        {
            score += 10;
            if (remainingHearts < hearts.Length)
            {
                remainingHearts++;
                hearts[remainingHearts - 1].enabled = true;
                timer = 10f;
                SetNewMessages(); // 在倒计时重置时设置message1和message2
            }
        }
        else
        {
            remainingHearts--;
            hearts[remainingHearts].enabled = false;
            timer = 10f;
            SetNewMessages(); // 在倒计时重置时设置message1和message2
        }

        scoreText.text = $"{score}";
        // 调用GlobalVariables的updateTotalScore方法并传入结果
        GlobalVariables.Instance.UpdateTotalScore(score);
    }

}
