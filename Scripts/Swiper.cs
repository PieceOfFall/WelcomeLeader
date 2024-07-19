using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Swiper : MonoBehaviour, IEndDragHandler
{
    public RectTransform ContentTransform;
    public float Size;
    public float Duration = 0.2f;
    public bool HandleEndSwipe = true;
    public UnityEvent<SwiperEventData> onEndSwipe;

    private ScrollRect ScrollRect;
    private float SizeStep;
    private float SwipeStep;

    void Awake()
    {
        SizeStep = Size;
        ScrollRect = GetComponentInChildren<ScrollRect>();
        float itemViewNum = ScrollRect.GetComponent<RectTransform>().rect.width / SizeStep;
        SwipeStep = 1f / (ContentTransform.childCount - itemViewNum);
    }

    /// <summary>
    /// ��ָ�����򻬶�һ��
    /// </summary>
    /// <param name="direction">��������</param>
    public void Swipe(Direction direction)
    {
        GameObject objToRemove = direction == Direction.Left
            ? ContentTransform.GetChild(ContentTransform.childCount - 1).gameObject
            : ContentTransform.GetChild(0).gameObject;

        // ����remove�Ķ���ŵ���һ��
        objToRemove.transform.SetSiblingIndex(direction == Direction.Left ? 0 : ContentTransform.childCount - 1);

        // ����Content
        // �򻬶��������ӿռ�
        float targetX = direction == Direction.Left ? 1 : 0;
        Vector2 originalAnchoredPos = ContentTransform.anchoredPosition;
        AdjustPivotAndPosition(new Vector2(targetX, 0.5f), originalAnchoredPos);
        ContentTransform.sizeDelta = new Vector2(ContentTransform.sizeDelta.x + SizeStep, ContentTransform.sizeDelta.y);
        // �򷴷�����ٿռ�
        float reverseTargetX = direction == Direction.Left ? 0 : 1;
        originalAnchoredPos = ContentTransform.anchoredPosition;
        AdjustPivotAndPosition(new Vector2(reverseTargetX, 0.5f), originalAnchoredPos);
        ContentTransform.sizeDelta = new Vector2(ContentTransform.sizeDelta.x - SizeStep, ContentTransform.sizeDelta.y);

        // ���л���
        float targetPos = ScrollRect.horizontalNormalizedPosition + (direction == Direction.Right ? SwipeStep : -SwipeStep);
        ScrollRect.DOHorizontalNormalizedPos(targetPos, Duration);
    }

    /// <summary>
    /// ����Anchor��Pivot
    /// </summary>
    /// <param name="newPivot">�µ�Pivot</param>
    /// <param name="originalAnchoredPos">ԭʼ��Anchorλ��</param>
    private void AdjustPivotAndPosition(Vector2 newPivot, Vector2 originalAnchoredPos)
    {
        Vector2 deltaPivot = newPivot - ContentTransform.pivot;
        Vector2 size = ContentTransform.rect.size;
        Vector2 deltaPosition = new(deltaPivot.x * size.x, deltaPivot.y * size.y);
        // ���� Pivot
        ContentTransform.pivot = newPivot;

        // ���� anchoredPosition �Բ����ƶ�
        ContentTransform.anchoredPosition = originalAnchoredPos + deltaPosition;
    }

    /// <summary>
    /// ��ק�¼��ص�
    /// </summary>
    /// <param name="eventData">�ص�����</param>
    public void OnEndDrag(PointerEventData eventData)
    {
        Direction swipeDirection = eventData.pressPosition.x > eventData.position.x ? Direction.Right : Direction.Left;
        Swipe(swipeDirection);
        if (HandleEndSwipe)
        {
            onEndSwipe.Invoke(new()
            {
                SwipeDirection = swipeDirection,
                PointerEventData = eventData
            });
        }
    }
}
