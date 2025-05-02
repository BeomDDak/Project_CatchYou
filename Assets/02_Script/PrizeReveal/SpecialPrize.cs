using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class SpecialPrize : MonoBehaviour
{
    
    [SerializeField] private GameObject specialStar;             // 스타 오브젝트
    [SerializeField] private GameObject specialPrizeTrs;          // 당첨 아이템 위치
    private SpecialItem specialItem;
    [SerializeField] private GameObject specialPrizeEffect1;     // 당첨 이펙트
    [SerializeField] private GameObject specialPrizeEffect2;     // 당첨 이펙트
    [SerializeField] private TextMeshProUGUI specialPrizeName;
    [SerializeField] private TextMeshProUGUI specialPrizePrice;

    // 배경
    [SerializeField] private Image background;
    [SerializeField] private ParticleSystem startPaticle;

    void Start()
    {
        specialItem = ProbabilityManager.Instance.lastSpecialItem;
        StartCoroutine(StartSpecialPrize());
    }

    IEnumerator StartSpecialPrize()
    {
        Color color = background.color;

        while (!startPaticle.isStopped)
        {
            yield return null;
        }

        background.gameObject.SetActive(true);          // 백그라운드 이미지 활성화
        SetSpecialPrize();

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
        // 스페셜 아이템 인벤토리에 추가 하려면 여기
    }

    private void SetSpecialPrize()
    {
        // 지우기
        foreach (Transform child in specialPrizeTrs.transform)
        {
            Destroy(child.gameObject);
        }

        // 켜기
        specialStar.SetActive(true);
        specialPrizeTrs.SetActive(true);
        specialPrizeName.gameObject.SetActive(true);
        specialPrizePrice.gameObject.SetActive(true);

        // 생성
        GameObject specialPrizeIns = Instantiate(specialItem.prefab, specialPrizeTrs.transform);
        specialPrizeIns.transform.localScale = specialPrizeTrs.transform.localScale;
        Instantiate(specialPrizeEffect1, specialPrizeTrs.transform);
        Instantiate(specialPrizeEffect2, specialPrizeTrs.transform);

        // 변경
        specialPrizeName.text = specialItem.name.Replace("\\n", "\n");
        specialPrizePrice.text = specialItem.price.ToString($"###,###")+"\\";
        specialPrizeTrs.GetComponent<Button>().onClick.AddListener(() => GameSceneManager.Instance.GoToScene("Reward"));
    }
}
