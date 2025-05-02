using System.Collections.Generic;
using UnityEngine;

// 기본 아이템 클래스
[System.Serializable]
public class PrizeItem
{
    public string name;             // 아이템 이름
    public GameObject prefab;       // 아이템 프리팹
    public float weight;            // 가중치(확률)
    public int star;                // 아이템 희귀도
    public int id;                 // 아이템 아이디
}

[System.Serializable]
public class SpecialItem
{
    public string name;
    public GameObject prefab;
    public int price;
    public int id;
}

public class ProbabilityManager : MonoBehaviour
{
    public static ProbabilityManager Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    [Header("뽑기 상품")]
    [SerializeField]
    private List<PrizeItem> prizeItems = new List<PrizeItem>();

    [Header("특별 상품")]
    [SerializeField]
    private List<SpecialItem> specialItem = new List<SpecialItem>();

    private float totalWeight = 0f;

    [Header("최근 당첨 품목")]
    public PrizeItem lastPrizeItem;
    public SpecialItem lastSpecialItem;

    private void Start()
    {
        CalculateTotalWeight();
    }
    // 모든 가중치 합계
    private void CalculateTotalWeight()
    {
        totalWeight = 0f;
        foreach(var item in  prizeItems)
        {
            totalWeight += item.weight;
        }
    }

    // 뽑기 물건 확률
    public PrizeItem GetRandomPrize()
    {
        // 0에서 총 가중치 사이의 랜덤 값 생성
        float randomVal = Random.Range(0f, totalWeight);
        float weightSum = 0f;

        // 가중치 누적 합계가 랜덤 값 초과하는 첫 번째 아이템 선택
        foreach (var item in prizeItems)
        {
            weightSum += item.weight;
            if (randomVal <= weightSum)
            {
                lastPrizeItem = item;
                return item;
            }
        }

        lastPrizeItem = prizeItems[prizeItems.Count - 1];
        return lastPrizeItem;
    }

    public SpecialItem SetSpecialPrize(int index)
    {
        foreach(var item in specialItem)
        {
            if(item.id == index)
            {
                lastSpecialItem = item;
                return item;
            }
        }
        return lastSpecialItem;
    }
}
