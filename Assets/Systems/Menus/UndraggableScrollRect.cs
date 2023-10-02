using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UI;
#endif

public class UndraggableScrollRect : ScrollRect {

    public override void OnBeginDrag(PointerEventData eventData) { }
    public override void OnDrag(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData) { }

    #if UNITY_EDITOR
    [CustomEditor(typeof(UndraggableScrollRect))]
    private class UndraggableScrollRectEditor : ScrollRectEditor { }
    #endif
}
