using UnityEngine;
using TMPro;

public class PrizeOutTextAnim : MonoBehaviour
{
    public TextMeshProUGUI prizeOutText;
    public float animSpeed = 2f;

    void Update()
    {
        float alpha = Mathf.PingPong(Time.time * animSpeed, 1f);

        Color color = prizeOutText.color;
        color.a = alpha;
        prizeOutText.color = color;
    }
}
