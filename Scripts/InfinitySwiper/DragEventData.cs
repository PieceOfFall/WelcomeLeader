﻿using System;
using UnityEngine.EventSystems;

/// <summary>
/// 滑动事件参数
/// </summary>
public class DragEventData
{
    /// <summary>
    /// ScrollView点击事件
    /// </summary>
    public PointerEventData PointerEventData {  get; set; }

    public Direction DragDirection { get; set; }
}

