using UnityEngine;

public enum IdleOptions
{
    Video,
    Swiper
}


public class IdleCtrl : MonoBehaviour
{


    public float timeoutSeconds = 900; // 用户无操作的超时时间（秒）
    public IdleOptions idleOptions = IdleOptions.Video;

    [Header("视频待机配置")]
    public GameObject IdleVideoObj;
    private bool isIdle = false;

    [Header("轮播图待机配置")]
    public Swiper Swiper;
    public float SwipeInterval = 5f;
    public Direction IdleSwipeDirection = Direction.Right;
    private float timer = 0f;

    private float lastInteractionTime; // 记录最后操作时间
    

    void Start()
    {
        ResetTimer();
    }

    void Update()
    {
        // 检查是否有键盘输入或鼠标移动
        if (Input.touchCount > 0 || Input.GetKeyDown(KeyCode.Space))
        {
            ResetTimer();
            if (isIdle)
                BreakIdle();
        }

        // 检查是否超时
        if (Time.time - lastInteractionTime > timeoutSeconds && !isIdle)
        {
            StartIdle();
        }

        // 轮播图Idle
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
    /// 重置计时器
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

