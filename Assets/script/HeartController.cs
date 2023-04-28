using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HeartController : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public List<Image> hearts;
    public float countdownDuration = 5f;

    private float countdownTimer;

    void Start()
    {
        countdownTimer = countdownDuration;
    }

    void Update()
    {
        countdownTimer -= Time.deltaTime;
        timerText.text = $"Time Remaining: {countdownTimer.ToString("F2")}s";

        if (countdownTimer <= 0)
        {
            RemoveHeart();
            countdownTimer = countdownDuration;
        }
    }

    void RemoveHeart()
    {
        if (hearts.Count > 0)
        {
            int lastIndex = hearts.Count - 1;
            Image lastHeart = hearts[lastIndex];
            lastHeart.gameObject.SetActive(false);
            hearts.RemoveAt(lastIndex);
        }
    }
}
