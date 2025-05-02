using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PrizeResult : MonoBehaviour
{
    [SerializeField]
    [Header("스타")]
    public GameObject star;
    public List<GameObject> stars = new List<GameObject>();

    [SerializeField]
    [Header("당첨아이템")]
    public GameObject prizeItem;
    public GameObject prizeItemEffect;

    [SerializeField]
    [Header("당첨아이템이름")]
    public TextMeshProUGUI prizeName;

    [SerializeField]
    [Header("배경")]
    public Image background;
    public ParticleSystem startPaticle;

    private PrizeItem prize;    // 당첨된 아이템 담아놓을 변수

    void Start()
    {
        prize = ProbabilityManager.Instance.lastPrizeItem;

        StartCoroutine(StartPrizeReveal());
    }

    IEnumerator StartPrizeReveal()
    {
        Color color = background.color;

        while (!startPaticle.isStopped)
        {
            yield return null;
        }

        background.gameObject.SetActive(true);          // 백그라운드 이미지 활성화
        SetPrize();                                     // 당첨 아이템 표시

        // 백그라운드 애니메이션
        if (startPaticle.isStopped)
        {
            while (color.a > 0f)
            {
                color.a -= Time.deltaTime * 0.2f;
                background.color = color;
                yield return null;
            }
            background.gameObject.SetActive(false);
        }

        PlayerInventory.Instance.lastObtainedItems.Add(prize);
    }

    private void SetPrize()
    {
        foreach (Transform child in prizeItem.transform)
        {
            Destroy(child.gameObject);
        }

        // 이름 변경
        prizeName.text = prize.name;

        // 아이템 생성
        GameObject prizeIns = Instantiate(prize.prefab, prizeItem.transform.position, Quaternion.identity);
        prizeIns.transform.SetParent(prizeItem.transform);
        prizeIns.transform.localScale = prize.prefab.transform.localScale;

        // 이펙트 생성
        Instantiate(prizeItemEffect, prizeItem.transform);
        SetStar(prize.star);

        star.SetActive(true);
        prizeName.gameObject.SetActive(true);
        prizeItem.gameObject.SetActive(true);
        prizeItem.GetComponent<Button>().onClick.AddListener(() => GameSceneManager.Instance.GoToScene("Play"));
    }

    private void SetStar(int count)
    {
        for (int i = 0; i < stars.Count; i++)
        {
            stars[i].SetActive(i < count);
        }
    }
}
