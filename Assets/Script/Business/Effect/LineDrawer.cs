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

    private GameObject _currentLine;
    private RectTransform _lineRectTransform;

    public void OnPointerDown(PointerEventData eventData)
    {
        _currentLine = Instantiate(linePrefab, lineRoot);
        _currentLine.GetComponent<Image>().color = lineColor;
        _lineRectTransform = _currentLine.GetComponent<RectTransform>();
        _lineRectTransform.sizeDelta = Vector2.zero;

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            lineRoot,
            eventData.position,
            eventData.pressEventCamera,
            out localPoint);

        _lineRectTransform.anchoredPosition = localPoint;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _currentLine = null;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(_lineRectTransform != null)
        {
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                lineRoot,
                eventData.position,
                eventData.pressEventCamera,
                out localPoint);

            Vector2 startPos = _lineRectTransform.anchoredPosition;
            Vector2 endPos = localPoint;

            // 更新线条的大小和位置
            Vector2 sizeDelta = new Vector2(Vector2.Distance(startPos, endPos), lineWidth);
            _lineRectTransform.sizeDelta = sizeDelta;
            _lineRectTransform.pivot = new Vector2(0, 0.5f);
            _lineRectTransform.anchoredPosition = startPos;

            // 计算旋转角度
            float angle = Mathf.Atan2(endPos.y - startPos.y, endPos.x - startPos.x) * Mathf.Rad2Deg;
            _lineRectTransform.localRotation = Quaternion.Euler(0, 0, angle);
        }
    }
}