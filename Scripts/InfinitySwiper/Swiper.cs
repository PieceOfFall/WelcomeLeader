using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Swiper : MonoBehaviour, IEndDragHandler
{
    public RectTransform ContentTransform;
    public float Duration = 0.2f;
    public bool HandleEndSwipe = true;
    public bool HandleEndDrag = false;
    public UnityEvent<DragEventData> onEndDrag;
    public UnityEvent<Direction> onEndSwipe;

    private ScrollRect ScrollRect;
    private float SizeStep;
    private float SwipeStep;
    private bool IsSwiping = false;

    void Awake()
    {
        SizeStep = ContentTransform.rect.width / ContentTransform.childCount;
        ScrollRect = GetComponentInChildren<ScrollRect>();
        float itemViewNum = ScrollRect.viewport.rect.width / SizeStep;
        SwipeStep = 1f / (ContentTransform.childCount - itemViewNum);
    }

    /// <summary>
    /// 向指定方向滑动一次
    /// </summary>
    /// <param name="direction">滑动方向</param>
    public void Swipe(Direction direction)
    {
        if (IsSwiping) return;
        IsSwiping = true;
        GameObject objToRemove = direction == Direction.Left
            ? ContentTransform.GetChild(ContentTransform.childCount - 1).gameObject
            : ContentTransform.GetChild(0).gameObject;

        // 将被remove的对象放到另一边
        objToRemove.transform.SetSiblingIndex(direction == Direction.Left ? 0 : ContentTransform.childCount - 1);

        // 调整Content
        // 向滑动方向增加空间
        float targetX = direction == Direction.Left ? 1 : 0;
        Vector2 originalAnchoredPos = ContentTransform.anchoredPosition;
        AdjustPivotAndPosition(new Vector2(targetX, 0.5f), originalAnchoredPos);
        ContentTransform.sizeDelta = new Vector2(ContentTransform.sizeDelta.x + SizeStep, ContentTransform.sizeDelta.y);
        // 向反方向减少空间
        float reverseTargetX = direction == Direction.Left ? 0 : 1;
        originalAnchoredPos = ContentTransform.anchoredPosition;
        AdjustPivotAndPosition(new Vector2(reverseTargetX, 0.5f), originalAnchoredPos);
        ContentTransform.sizeDelta = new Vector2(ContentTransform.sizeDelta.x - SizeStep, ContentTransform.sizeDelta.y);

        bool hasCorutineComplete = false;
        bool hasEventComplete = false;

        // 进行滑动
        float targetPos = ScrollRect.horizontalNormalizedPosition + (direction == Direction.Right ? SwipeStep : -SwipeStep);
        ScrollRect.DOHorizontalNormalizedPos(targetPos, Duration).OnComplete(() =>
        {
            hasCorutineComplete = true;
            if(hasEventComplete)
            {
                IsSwiping = false;
            }
        });

        // 滑动事件回调
        if (HandleEndSwipe)
        {
            onEndSwipe.Invoke(direction);
            hasEventComplete = true;
            if(hasCorutineComplete)
            {
                IsSwiping = false;
            }
        }
    }

    /// <summary>
    /// 调整Anchor和Pivot
    /// </summary>
    /// <param name="newPivot">新的Pivot</param>
    /// <param name="originalAnchoredPos">原始的Anchor位置</param>
    private void AdjustPivotAndPosition(Vector2 newPivot, Vector2 originalAnchoredPos)
    {
        Vector2 deltaPivot = newPivot - ContentTransform.pivot;
        Vector2 size = ContentTransform.rect.size;
        Vector2 deltaPosition = new(deltaPivot.x * size.x, deltaPivot.y * size.y);
        // 更新 Pivot
        ContentTransform.pivot = newPivot;

        // 调整 anchoredPosition 以补偿移动
        ContentTransform.anchoredPosition = originalAnchoredPos + deltaPosition;
    }

    /// <summary>
    /// 拖拽事件回调
    /// </summary>
    /// <param name="eventData">回调参数</param>
    public void OnEndDrag(PointerEventData eventData)
    {
        Direction dragDirection = eventData.pressPosition.x > eventData.position.x ? Direction.Right : Direction.Left;

        if (HandleEndDrag)
        {
            onEndDrag.Invoke(new()
            {
                DragDirection = dragDirection,
                PointerEventData = eventData
            });
        }

        Swipe(dragDirection);
    }
}
