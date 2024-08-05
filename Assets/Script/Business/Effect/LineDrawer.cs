using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[Obsolete]
public class LineDrawer : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public float lineWidth = 20f;
    public Color lineColor = Color.white;

    public GameObject linePrefab;
    public RectTransform lineRoot;

    private GameObject _currentLineObj;
    private RectTransform _metroLineRoot;

    public void OnPointerDown(PointerEventData eventData)
    {
        _currentLineObj = Instantiate(linePrefab, lineRoot);
        _currentLineObj.GetComponent<Image>().color = lineColor;
        _metroLineRoot = _currentLineObj.GetComponent<RectTransform>();
        _metroLineRoot.sizeDelta = Vector2.zero;

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            lineRoot,
            eventData.position,
            eventData.pressEventCamera,
            out localPoint);

        _metroLineRoot.anchoredPosition = localPoint;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _currentLineObj = null;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(_metroLineRoot != null)
        {
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                lineRoot,
                eventData.position,
                eventData.pressEventCamera,
                out localPoint);

            Vector2 startPos = _metroLineRoot.anchoredPosition;
            Vector2 endPos = localPoint;

            // 更新线条的大小和位置
            Vector2 sizeDelta = new Vector2(Vector2.Distance(startPos, endPos), lineWidth);
            _metroLineRoot.sizeDelta = sizeDelta;
            _metroLineRoot.pivot = new Vector2(0, 0.5f);
            _metroLineRoot.anchoredPosition = startPos;

            // 计算旋转角度
            float angle = Mathf.Atan2(endPos.y - startPos.y, endPos.x - startPos.x) * Mathf.Rad2Deg;
            _metroLineRoot.localRotation = Quaternion.Euler(0, 0, angle);
        }
    }
}