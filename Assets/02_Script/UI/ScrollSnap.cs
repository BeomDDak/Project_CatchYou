using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollSnap : MonoBehaviour, IEndDragHandler
{
    public ScrollRect scrollRect;
    private int itemCount;

    void Start()
    {
        // 아이템 수 자동 계산
        itemCount = scrollRect.content.childCount;
    }

    // 드래그가 끝났을 때 자동으로 호출됨
    public void OnEndDrag(PointerEventData eventData)
    {
        SnapToClosestItem();
    }

    void SnapToClosestItem()
    {
        // 현재 정규화된 위치 (0~1)
        float scrollPos = scrollRect.horizontalNormalizedPosition;

        // 가장 가까운 인덱스 계산 (itemCount-1 개의 영역으로 나눔)
        float itemStep = 1f / (itemCount - 1);
        int targetIndex = Mathf.RoundToInt(scrollPos / itemStep);

        // 타겟 위치로 스냅
        float targetPosition = targetIndex * itemStep;

        // 스냅 애니메이션
        StartCoroutine(SmoothScrollTo(targetPosition));
    }

    System.Collections.IEnumerator SmoothScrollTo(float targetPosition)
    {
        float startPosition = scrollRect.horizontalNormalizedPosition;
        float time = 0;
        float duration = 0.2f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            scrollRect.horizontalNormalizedPosition = Mathf.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        scrollRect.horizontalNormalizedPosition = targetPosition;
    }
}
