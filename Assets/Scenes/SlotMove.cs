using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotMove : MonoBehaviour
{
    [Header("抽獎按鈕")]
    [SerializeField] Button DrowBtn;

    [Header("獎勵圖片")]
    [SerializeField] Image[] ArardImgArr;

    [Header("轉盤速度")]
    [SerializeField] float AniMoveSpeed = 0f;

    // 進度
    private float[] progress = new[] { 0f, 1f, 2f, 3f, 4f, 5f, 6f};

    // 轉動動畫位置
    private Vector3[] AniPosV3 = new[]
          {Vector3.up * 400, Vector3.up * 300, Vector3.up * 200, Vector3.up * 100, Vector3.zero,Vector3.down * 100, Vector3.down * 200};

    // 自動暫停
    private bool isAutoStop;
    // 抽獎結束停止ui
    private bool isStopUpdatePos;
    // 隨機停止
    int rang;

    void Start()
    {
        DrowBtn.onClick.AddListener(DrawFun);
        isAutoStop = false;
        isStopUpdatePos = false;
    }

    void Update()
    {
        if (isStopUpdatePos) return;


        float t = Time.deltaTime * AniMoveSpeed;
        for (int i = 0; i < ArardImgArr.Length; i++)
        {
            progress[i] += t;
            ArardImgArr[i].transform.localPosition = MovePosition(i);
        }
    }


    /// <summary>獲取下一個位置</summary>
    Vector3 MovePosition(int position)    
    {
        int index = Mathf.FloorToInt(progress[position]);
        rang = Random.Range(0, 8);

        if (index > AniPosV3.Length - 2)
        {
            progress[position] -= index;
            index = 0;
            if (position == rang && isAutoStop)
            {
                isStopUpdatePos = true;
                Debug.Log("展示獎勵介面...");
                Debug.Log("位置為 ： " + position);
            }
            return AniPosV3[index];
        }
        else
        {
            return Vector3.Lerp(AniPosV3[index], AniPosV3[index + 1], progress[position] - index);
        }
    }

    /// <summary>點擊抽獎</summary>
    void DrawFun()
    {
        isAutoStop = false;
        isStopUpdatePos = false;
        StartCoroutine(SetMoveSpeed(2));
    }

    /// <summary>抽獎動畫速度控制</summary>
    IEnumerator SetMoveSpeed(int time)
    {
        /*
        AniMoveSpeed = 30;
        yield return new WaitForSeconds(time);
        AniMoveSpeed = 2;
        yield return new WaitForSeconds(time);
        isAutoStop = true;
        */

        float dt = 0f;
        float dura = time * 3f;

        while (dt < 1)
        {
            //dt管理時間
            dt += UnityEngine.Time.deltaTime / dura;
            AniMoveSpeed = Mathf.Lerp(30, 2f, dt);
            yield return null;
        }
        isAutoStop = true;
    }
}

