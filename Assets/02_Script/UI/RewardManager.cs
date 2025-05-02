using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class RewardManager : MonoBehaviour
{
    // 컨텐츠 오브젝트 (스크롤 뷰의 Content)
    public Transform contentContainer;

    // 시작 번호
    public int startNumber = 1;

    [Header("버튼")]
    public Button inventory_Button;
    public Button inventoryExit_Button;

    [Header("인벤토리 관련")]
    public GameObject inventory_Panel;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI mileageText;
    public Transform inventory_ContentContainer;
    public GameObject noItem_Image;
    public List<GameObject> inventoryItemUi = new List<GameObject>();

    void Start()
    {
        NumberImagesText();
        this.GetComponent<HideNavi>().ClosePanel();
        
        // 인벤토리 버튼 연결
        inventory_Button.onClick.AddListener(() => 
        {
            this.GetComponent<HideNavi>().OpenPanel();
            inventory_Panel.SetActive(true);
            UpdateInventoryUI();
        });

        // 인벤토리 나가기 버튼 연결
        inventoryExit_Button.onClick.AddListener(() =>
        {
            this.GetComponent<HideNavi>().ClosePanel();
            inventory_Panel.SetActive(false);
        });

    }

    // 번호 매기기 메서드
    public void NumberImagesText()
    {
        // 컨텐츠의 모든 직계 자식(이미지 오브젝트들)을 가져옴
        for (int i = 0; i < contentContainer.childCount; i++)
        {
            Transform imageObject = contentContainer.GetChild(i);

            // TextMeshPro 컴포넌트 찾기
            TextMeshProUGUI textComponent = imageObject.GetComponentInChildren<TextMeshProUGUI>();
            if (textComponent != null)
            {
                textComponent.text = (i + startNumber).ToString() + "번";
            }
        }
    }

    private void UpdateInventoryUI()
    {
        goldText.text = PlayerInventory.Instance.gold.ToString();
        mileageText.text = PlayerInventory.Instance.mileage.ToString();

        // 먼저 기존 Content 안에 있던 모든 아이템 UI 제거 (중복 방지)
        foreach (Transform child in inventory_ContentContainer)
        { 
            Destroy(child.gameObject);
        }

        // 인벤토리에 아무 아이템도 없으면 X 표시 프리팹 추가
        if (PlayerInventory.Instance.lastObtainedItems.Count <= 0)
        {
            Instantiate(noItem_Image, inventory_ContentContainer);
        }
        else
        {
            // lastObtainedItems 리스트 안에 있는 아이템 추가
            foreach (PrizeItem item in PlayerInventory.Instance.lastObtainedItems)
            {
                foreach (GameObject uiPrefab in inventoryItemUi)
                {
                    // 프리팹에 붙어 있는 PrizeItemHolder 가져오기
                    PrizeItemHolder holder = uiPrefab.GetComponent<PrizeItemHolder>();

                    // 비교: 이름 일치
                    if (holder.prizeItemData.id == item.id)
                    {
                        Instantiate(uiPrefab, inventory_ContentContainer);
                        break;
                    }
                }
            }
        }
    }
}
