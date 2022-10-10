using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    public TMP_Text timerText;

    public float time;
    private int minutes, seconds;

    void Update()
    {
        time -= Time.deltaTime;
        minutes = (int)(time / 60f);
        seconds = (int)(time - minutes * 60f);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
