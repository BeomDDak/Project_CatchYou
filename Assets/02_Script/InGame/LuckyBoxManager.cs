using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public static class PrizeIds
{
    public const int NONE = 0;
    public const int CHANEL = 1;
    public const int GPU5090 = 2;
    public const int MONITOR_G9 = 3;
}

[System.Serializable]
public class PrizeInfo
{
    public int id;
    public string name;
    public string price;
    public string requiredKeyText;
    public Sprite image;
}

public class LuckyBoxManager : MonoBehaviour
{
    private int currentIndex;

    [Header("버튼들")]
    [SerializeField] private Button[] luckyButtons;

    [Header("럭키박스 패널")]
    [SerializeField] private GameObject luckyPanel;
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemPrice;
    [SerializeField] private TextMeshProUGUI canOpenNeedKeyText;

    [Header("상품 정보")]
    [SerializeField] private PrizeInfo[] prizeInfos;

    [Header("기타 UI")]
    [SerializeField] private Button openBtn;
    [SerializeField] private Button inventoryBtn;
    [SerializeField] private Image openImage;
    [SerializeField] private GameObject[] luckyBoxMainImage;

    private int currentSelectedId;

    private int[] itemId = new int[12];

    void Start()
    {
        // 줄바꿈 코드 자동 변환 (한번만 하면 됨)
        foreach (PrizeInfo info in prizeInfos)
        {
            info.name = info.name.Replace("\\n", "\n");
        }

        LoadItemIds();  // PlayerPrefs 불러옴

        // 메인화면 이미지 켜고 끄기
        for (int i = 0; i < luckyBoxMainImage.Length; i++)
        {
            luckyBoxMainImage[i].SetActive(false);
        }

        for (int i = 0; i < itemId.Length; i++)
        {
            if (itemId[i] != PrizeIds.NONE)
            {
                luckyBoxMainImage[i].SetActive(true);
            }
        }

        // 버튼 연결
        for (int i = 0; i < luckyButtons.Length; i++)
        {
            int index = i;
            luckyButtons[i].onClick.AddListener(() => ClickLuckyBox(index));
        }

        openBtn.onClick.AddListener(OpenPrize);
    }

    void LoadItemIds()
    {
        for (int i = 0; i < itemId.Length; i++)
        {
            itemId[i] = PlayerPrefs.GetInt("LuckyBoxSlot_" + i, -1);
            if (itemId[i] == -1)
            {
                if (i == 0) itemId[i] = PrizeIds.CHANEL;
                else if (i == 1) itemId[i] = PrizeIds.GPU5090;
                else if (i == 2) itemId[i] = PrizeIds.MONITOR_G9;
                else itemId[i] = PrizeIds.NONE;

                PlayerPrefs.SetInt("LuckyBoxSlot_" + i, itemId[i]);
            }
        }
        PlayerPrefs.Save();
    }

    public void ClickLuckyBox(int index)
    {
        luckyPanel.SetActive(true);
        currentIndex = index;

        inventoryBtn.interactable = false;

        int selectedId = (index >= 0 && index < itemId.Length) ? itemId[index] : PrizeIds.NONE;

        PrizeInfo info = GetPrizeInfoById(selectedId);

        if (info != null)
        {
            itemName.text = info.name;
            itemPrice.text = info.price;
            canOpenNeedKeyText.text = info.requiredKeyText;
            itemImage.sprite = info.image;
        }
        else
        {
            itemName.text = "정보 없음";
            itemPrice.text = "-";
            canOpenNeedKeyText.text = "-";
            itemImage.sprite = null;
        }

        currentSelectedId = selectedId;

        bool hasKey = false;
        foreach (PrizeItem item in PlayerInventory.Instance.lastObtainedItems)
        {
            if (item.id == currentSelectedId)
            {
                hasKey = true;
                break;
            }
        }

        openBtn.interactable = hasKey;
        ProbabilityManager.Instance.SetSpecialPrize(selectedId);
    }

    private void OpenPrize()
    {
        StartCoroutine(OpenImageAnim());
    }

    IEnumerator OpenImageAnim()
    {
        openImage.gameObject.SetActive(true);
        GameSceneManager.Instance.transform.Find("Canvas/Navigator_Panel").gameObject.SetActive(false);

        Color color = openImage.color;

        while (color.a < 1f)
        {
            color.a += Time.deltaTime * 0.2f;
            openImage.color = color;
            yield return null;
        }

        SelectSpecialPrize();
        RemoveKey();

        GameSceneManager.Instance.LoadScene("SpcialPrize");
    }

    private void SelectSpecialPrize()
    {
        ProbabilityManager.Instance.SetSpecialPrize(currentSelectedId);
        
    }

    private void RemoveKey()
    {
        PrizeItem removeItem = null;

        foreach (PrizeItem item in PlayerInventory.Instance.lastObtainedItems)
        {
            if (item.id == currentSelectedId)
            {
                removeItem = item;
                break;
            }
        }

        if (removeItem != null)
            PlayerInventory.Instance.lastObtainedItems.Remove(removeItem);

        // 아이템 제거 + PlayerPrefs 저장
        itemId[currentIndex] = PrizeIds.NONE;
        PlayerPrefs.SetInt("LuckyBoxSlot_" + currentIndex, PrizeIds.NONE);
        PlayerPrefs.Save();

        ClickLuckyBox(currentIndex);  // UI 갱신
    }

    private PrizeInfo GetPrizeInfoById(int id)
    {
        foreach (PrizeInfo info in prizeInfos)
        {
            if (info.id == id)
                return info;
        }
        return null;
    }

    public void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();  // 플레이어 프랩스 저장 초기화
    }
}
