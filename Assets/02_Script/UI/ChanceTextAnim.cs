using UnityEngine;
using TMPro;
using System.Collections;

public class ChanceTextAnim : MonoBehaviour
{
    public float floatSpeed = 50f;      // 텍스트가 올라가는 속도
    public float fadeSpeed = 1.0f;      // 텍스트 페이드 아웃 속도

    private TextMeshProUGUI chanceText;  // UI용 TextMeshPro

    void Start()
    {
        chanceText = this.GetComponent<TextMeshProUGUI>();
        StartCoroutine(AnimationText());
    }

    IEnumerator AnimationText()
    {
        if (chanceText != null)
        {
            Color textColor = chanceText.color;

            while (textColor.a > 0)
            {
                // 위로 이동
                transform.localPosition += new Vector3(0, floatSpeed * Time.deltaTime, 0);

                // 알파값 감소
                textColor.a -= fadeSpeed * Time.deltaTime;
                chanceText.color = textColor;

                yield return null;
            }
        }
    }
}
