using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Move : MonoBehaviour
{
    [Header("圖片")]
    [SerializeField] GameObject[] imags;

    [Header("速度")]
    [SerializeField] float speed;

    [Header("開始按鈕")]
    [SerializeField] Button playButton;


    float aniSpeed = 0.01f;//動畫速度
    float stopTimer = 5f;//轉動倒數時間
    float dt = 0;
    int imagIndex;
    float moveSpeed;
    float x = 1;
    bool isStopTime;//是否暫停計時
    bool isSlowDown;//是否開始減速


    private bool isOnClickPlaying; // 點了抽獎按鈕正在抽獎
    private int rewardIndex = 0;  // 本次中獎ID
    public float height = 100;//一個圖片高度



    private void Awake()
    {
        Debug.Log("開始執行");
        isStopTime = false;
        isSlowDown = false;
    }


    void Update()
    {
    }

    private void OnEnable()
    {
        playButton.onClick.AddListener(PlayGame);
    }



    void Timer()
    {
        if (isStopTime == true)
        {
            stopTimer -= 1;
            Debug.Log("轉輪計時器倒數 ---- ： " + stopTimer);
        }
        if (stopTimer == 0)
        {
            isSlowDown = true;
            CancelInvoke("Timer");//關閉計時器
            //InvokeRepeating("SlowDownSpeed", 0, 1);
            //StartCoroutine(SlowDown());
        }
    }

    /// <summary> 滾動動畫效果</summary>
    IEnumerator AllMove()
    {
        int index = 100;
        for (int i = 0; i < imags.Length; i++)
        {
            imagIndex = i;

            if (imags[i].transform.localPosition.y == -300) imags[i].transform.localPosition = new Vector3(0, 300, 0);
            else imags[i].transform.Translate(new Vector3(0, -100, 0));
            yield return null;
        }
        //減速
        if (isSlowDown == true)
        {
            
            if (aniSpeed < 0.5)
            {
                aniSpeed += 0.001f * x;
                x = x * 1.1f;
                Debug.Log("測試速度 ---- " + aniSpeed);
            }
        }
        yield return new WaitForSeconds(aniSpeed);
        if (aniSpeed < 0.5) StartCoroutine(AllMove());
    }

    /// <summary> 開始滾動</summary>
    void PlayGame()
    {
        StartCoroutine(AllMove());
        //InvokeRepeating("Timer", 3, 1);
    }

    public void StartSlow()
    {
        isSlowDown = true;
    }

    /// <summary> 停止滾動</summary>
    void StopMove()
    {
        Debug.Log("開始停止滾動");
        if (aniSpeed > 0.9) return;
        Debug.Log("開始停止滾動1111111");
        //StartCoroutine(SlowDown());
    }

    /// <summary> 減速</summary>
    IEnumerator SlowDown() 
    {
        Debug.Log("開始緩速");
        while (aniSpeed > 0)
        {
            yield return new WaitForSeconds(0.1f);
            aniSpeed -= 0.1f;


            if (aniSpeed <= 0)
            {
                Debug.Log("緩速成功，並且歸零");
                //StartCoroutine(ReturnZero());
            }
        }
        yield return new WaitForSeconds(1f);
    }

    /// <summary> 位置歸零</summary>
    IEnumerator ReturnZero()
    {
        Debug.Log("開始歸零");
        float dis = imags[0].transform.localPosition.y;
        int id = 0;

        for (int i = 0; i < imags.Length; i++)
        {
            if (Mathf.Abs(imags[i].transform.localPosition.y) < Mathf.Abs(dis))
            {
                dis = imags[i].transform.localPosition.y;
                id = i;
            }
        }

        while (Mathf.Abs(imags[id].transform.localPosition.y) > 0.5f)
        {
            yield return new WaitForSeconds(0.01f);
            for (int k = 0; k < imags.Length; k++)
            {
                imags[k].transform.localPosition -= new Vector3(0, dis/100, 0);
               // Debug.Log("位置 ： " + imags[k].transform.localPosition);
            }
        }
    }

    //void StopLo

}
