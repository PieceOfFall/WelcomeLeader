using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockCtrl : MonoBehaviour
{
    [HideInInspector]
    public bool IsLock
    {
        get => LockImage.sprite == LockSprite;
    }

    public Sprite LockSprite;
    public Sprite UnlockSprite;
    public IdleCtrl idleCtrl;
    public Swiper swiper;

    private Image LockImage;

    void Start()
    {
        LockImage = GetComponent<Image>();

        StartCoroutine(WaitToggle());
    }

    IEnumerator WaitToggle()
    {
        yield return new WaitForSeconds(5);
        if (IsLock)
        {
            ToggleLock();
        }
        yield return null;
    }

    public void ToggleLock()
    {
        LockImage.sprite = IsLock ? UnlockSprite : LockSprite;
        idleCtrl.EnableAutoSwipe = !IsLock;
        swiper.EnableSwipe = !IsLock;
        if (swiper.EnableSwipe) swiper.Swipe(Direction.Right);
    }
}
