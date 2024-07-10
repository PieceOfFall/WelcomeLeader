using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SwiperCtrl : MonoBehaviour
{
    [Header("轮播图")]
    public Swiper CustomSwiper;

    [Header("背景图片")]
    public int SelectedIndex;
    public Image BgImage;
    public Image NoiseImage;
    public float Duration;
    public float HeightGrowth = 70f;

    [Header("日期字符串")]
    public TextMeshProUGUI DateText;
    public float DateHeightGrowth = 160f;
    public float DateDuration = 1.5f;

    private Vector2 startDateTextPos;
    private Vector4 startDateTextColor;
    private Coroutine dateTextCorutine;

    [Header("内容字符串")]
    public TextMeshProUGUI ContentTextMeshPro;
    public float ContentTextDuration = 1.5f;
    private Coroutine contentTextCoroutine;

    private RectTransform lastObjRect;


    private void Start()
    {
        lastObjRect = CustomSwiper.ContentTransform.GetChild(SelectedIndex).gameObject.GetComponent<RectTransform>();

        SwiperItem startSwiperItem = lastObjRect.gameObject.GetComponent<SwiperItem>();
        startDateTextPos = DateText.transform.localPosition;
        startDateTextColor = DateText.color;
        DateText.text = $"{startSwiperItem.Year}年{startSwiperItem.Month}月{startSwiperItem.Day}日";

        dateTextCorutine = StartCoroutine(ChangeDateAlphaAndPos(DateText, 1, DateText.transform.localPosition.y + DateHeightGrowth, startSwiperItem.Text));
        StartCoroutine(ChangeHeight(lastObjRect, lastObjRect.sizeDelta.y + HeightGrowth));
    }

    public void HandleSwipe(SwiperEventData _)
    {
        GameObject selectedObj = CustomSwiper.ContentTransform.GetChild(SelectedIndex).gameObject;
        SwiperItem selectedItem = selectedObj.GetComponent<SwiperItem>();

        RectTransform currentRect = selectedObj.GetComponent<RectTransform>();
        StartCoroutine(ChangeHeight(currentRect, currentRect.sizeDelta.y + HeightGrowth));
        StartCoroutine(ChangeHeight(lastObjRect, lastObjRect.sizeDelta.y - HeightGrowth));

        if (dateTextCorutine != null)
            StopCoroutine(dateTextCorutine);
        if (contentTextCoroutine != null)
            StopCoroutine(contentTextCoroutine);
        ContentTextMeshPro.text = "";
        DateText.transform.localPosition = startDateTextPos;
        DateText.color = startDateTextColor;
        DateText.text = $"{selectedItem.Year}年{selectedItem.Month}月{selectedItem.Day}日";
        dateTextCorutine = StartCoroutine(ChangeDateAlphaAndPos(DateText, 1, DateText.transform.localPosition.y + DateHeightGrowth, selectedItem.Text));

        BgImage.sprite = selectedItem.OuterBg;
        NoiseImage.sprite = selectedItem.NoiseBg;


        lastObjRect = currentRect;
    }


    private IEnumerator ChangeHeight(RectTransform rect, float targetValue)
    {
        float startValue = rect.sizeDelta.y;
        float elapsedTime = 0f;

        while (elapsedTime < Duration)
        {
            elapsedTime += Time.deltaTime;
            rect.sizeDelta = new(rect.sizeDelta.x, Mathf.SmoothStep(startValue, targetValue, elapsedTime / Duration));
            yield return null;
        }
        rect.sizeDelta = new(rect.sizeDelta.x, targetValue);
    }

    private IEnumerator ChangeDateAlphaAndPos(TextMeshProUGUI DateText, float targetAlpha, float targetPos, string contentText)
    {
        float startAlpha = DateText.color.a;
        float startPos = DateText.transform.localPosition.y;
        float elapsedTime = 0f;

        while (elapsedTime < DateDuration)
        {
            elapsedTime += Time.deltaTime;
            DateText.transform.localPosition = new(DateText.transform.localPosition.x, Mathf.SmoothStep(startPos, targetPos, elapsedTime / DateDuration));
            DateText.color = new(DateText.color.r, DateText.color.g, DateText.color.b, Mathf.SmoothStep(startAlpha, targetAlpha, elapsedTime / DateDuration));
            yield return null;
        }

        DateText.transform.localPosition = new(DateText.transform.localPosition.x, targetPos);
        DateText.color = new(DateText.color.r, DateText.color.g, DateText.color.b, targetAlpha);

        contentTextCoroutine = StartCoroutine(ChangeContentText(ContentTextMeshPro, contentText));
    }

    private IEnumerator ChangeContentText(TextMeshProUGUI contentText, string content)
    {
        float elapsedTime = 0f;
        int currentIndex = 0;

        bool hasShowFirst = false;
        while (elapsedTime < ContentTextDuration)
        {
            elapsedTime += Time.deltaTime;
            float calcIndex = Mathf.SmoothStep(0, content.Length, elapsedTime / ContentTextDuration);

            for (int i = currentIndex + 1; i < calcIndex; i++)
            {
                if (!hasShowFirst)
                {
                    contentText.text += content[0];
                    hasShowFirst = true;
                }
                contentText.text += content[i];
            }

            currentIndex = (int)calcIndex;
            yield return null;
        }
    }

}
