using UnityEngine;

public enum IdleOptions
{
    Video,
    Swiper
}


public class IdleCtrl : MonoBehaviour
{


    public float timeoutSeconds = 900; // �û��޲����ĳ�ʱʱ�䣨�룩
    public IdleOptions idleOptions = IdleOptions.Video;

    [Header("��Ƶ��������")]
    public GameObject IdleVideoObj;
    private bool isIdle = false;

    [Header("�ֲ�ͼ��������")]
    public Swiper Swiper;
    public float SwipeInterval = 5f;
    public Direction IdleSwipeDirection = Direction.Right;
    private float timer = 0f;

    private float lastInteractionTime; // ��¼������ʱ��
    

    void Start()
    {
        ResetTimer();
    }

    void Update()
    {
        // ����Ƿ��м������������ƶ�
        if (Input.touchCount > 0 || Input.GetKeyDown(KeyCode.Space))
        {
            ResetTimer();
            if (isIdle)
                BreakIdle();
        }

        // ����Ƿ�ʱ
        if (Time.time - lastInteractionTime > timeoutSeconds && !isIdle)
        {
            StartIdle();
        }

        // �ֲ�ͼIdle
        if (idleOptions == IdleOptions.Swiper && isIdle)
        {
            timer += Time.deltaTime;

            if(timer > SwipeInterval)
            {
                timer = 0f;
                Swiper.Swipe(IdleSwipeDirection);
            }
        }
    }

    private void OnEnable()
    {
        ResetTimer();
    }

    /// <summary>
    /// ���ü�ʱ��
    /// </summary>
    void ResetTimer()
    {
        lastInteractionTime = Time.time;
    }

    private void StartIdle()
    {
        isIdle = true;
        if (idleOptions == IdleOptions.Video)
        {
            IdleVideoObj.SetActive(true);
        }
    }

    private void BreakIdle()
    {
        isIdle = false;
        if (idleOptions == IdleOptions.Video)
        {
            IdleVideoObj.SetActive(false);
        }
    }
}

