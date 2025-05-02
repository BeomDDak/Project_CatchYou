using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    [SerializeField]
    [Header("게임 플레이 패널")]
    public GameObject playPanel;
    public bool openPlayPanal = false;
    private int gold;
    public TextMeshProUGUI goldText;
    public Button startPlayButton;
    public Button pressButton;

    [SerializeField]
    [Header("집게발 이동 관련 변수")]
    public GameObject claw;
    private RectTransform clawRectTransform;
    public float clawPosX;
    public float clawPosY;
    private int clawSpeed;
    private int clawDir;

    [SerializeField]
    [Header("푸쉬 버튼 관련")]
    public bool pressing = false;
    private int clawDownMax = 700;
    public GameObject clawedPrize;

    [SerializeField]
    [Header("찬스 관련")]
    private int chance;
    public GameObject chanceEffectPrefab;
    public GameObject chanceTextPrefab;

    [SerializeField]
    [Header("상품 체크 화면")]
    public GameObject prizeOut;
    public Image prizeEnterImage;
    public AnimationCurve scaleCurve;
    public GameObject prizeCheckText;
    public Button prize;
    public GameObject prizeCheckEffect;
    public GameObject prizeHide;
    public Image prizeRevealChangeImage;

    void Start()
    {
        // 변수설정
        clawRectTransform = claw.GetComponent<RectTransform>();
        clawPosX = clawRectTransform.anchoredPosition.x;
        clawPosY = clawRectTransform.anchoredPosition.y;
        clawSpeed = 1;
        clawDir = 1;

        GameSceneManager.Instance.transform.Find("Canvas/Navigator_Panel").gameObject.SetActive(true);

        gold = PlayerInventory.Instance.gold;
        goldText.text = PlayerInventory.Instance.gold.ToString();

        startPlayButton.onClick.AddListener(() =>
        {
            StartGame();
        });
    }

    void Update()
    {
        ClawMove();
    }

    // 집게발 이동 함수
    public void ClawMove()
    {
        clawPosX = clawRectTransform.anchoredPosition.x;

        if (openPlayPanal & !pressing)
        {
            if (clawPosX > 395)
            {
                clawDir = -1;
            }

            if (clawPosX < -395)
            {
                clawDir = 1;
            }

            clawRectTransform.anchoredPosition += new Vector2(clawSpeed * clawDir, 0);
        }
    }

    public void StartGame()
    {
        if(gold < 100)
        {
            return;
        }
        else
        {
            PlayerInventory.Instance.gold -= 100;
            goldText.text = PlayerInventory.Instance.gold.ToString();
            pressButton.gameObject.SetActive(true);
            startPlayButton.gameObject.SetActive(false);
            ChanceProbability();
        }
    }

    // 집게발 프레스
    public void PressClaw()
    {
        pressing = true;
        StartCoroutine(PressAnim());
    }

    // 집게발 애니메이션
    IEnumerator PressAnim()
    {
        // 아래로 이동하는 애니메이션
        float duration = 1.5f; // 이동에 걸리는 시간
        float timer = 0;
        float startY = clawRectTransform.anchoredPosition.y;
        float targetY = startY - clawDownMax; // 값 만큼 아래로 이동

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float progress = timer / duration;
            float newY = Mathf.Lerp(startY, targetY, progress);
            clawRectTransform.anchoredPosition = new Vector2(clawRectTransform.anchoredPosition.x, newY);
            yield return null; // 다음 프레임까지 대기
        }

        // 완전히 내려간 상태에서 대기
        yield return new WaitForSeconds(1f);

        StartCoroutine(CreatePrize(targetY,duration));  // 뽑기 상품 이미지 생성
        ProbabilityManager.Instance.GetRandomPrize();   // 아이템 확률 계산

        // 다시 위로 이동하는 애니메이션
        timer = 0;
        startY = clawRectTransform.anchoredPosition.y;
        targetY = startY + clawDownMax; // 값 만큼 위로 이동

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float progress = timer / duration;
            float newY = Mathf.Lerp(startY, targetY, progress);
            clawRectTransform.anchoredPosition = new Vector2(clawRectTransform.anchoredPosition.x, newY);
            yield return null;
        }

        // 완료 후 행동
        pressing = false;
        CheckPrizeOut();
    }

    // 뽑기 상품 생성 및 제거
    public IEnumerator CreatePrize(float clawedPrizePosY, float duration)
    {
        GameObject claw = GameObject.FindGameObjectWithTag("Claw");
        GameObject clawedPrizeIns = Instantiate(clawedPrize, claw.transform.position, Quaternion.identity);
        clawedPrizeIns.transform.SetParent(claw.transform);
        clawedPrizeIns.transform.localPosition = new Vector3(0, clawedPrizePosY+150, 0);
        clawedPrizeIns.transform.localScale = Vector3.one;

        yield return new WaitForSeconds(duration);
        Destroy(clawedPrizeIns);
    }

    // 찬스생성
    public void ChanceProbability()
    {
        int chanceEffectX;
        GameObject machinePanel = GameObject.Find("Machine_Panel");

        chance = Random.Range(0, 11);
        chanceEffectX = Random.Range(-293, 294);

        if( chance >= 0)
        {
            // 이펙트 생성
            GameObject chanceEffectIns = Instantiate(chanceEffectPrefab, machinePanel.transform.position, Quaternion.identity);
            chanceEffectIns.transform.SetParent(machinePanel.transform);
            chanceEffectIns.transform.localPosition = new Vector3(chanceEffectX, -400, 0);

            // 텍스트 생성
            GameObject chanceTextIns = Instantiate(chanceTextPrefab, machinePanel.transform.position, Quaternion.identity);
            chanceTextIns.transform.SetParent(machinePanel.transform);
            chanceTextIns.transform.localPosition = new Vector3(chanceEffectX + 50, -100, 0);
            chanceTextIns.transform.localScale = Vector3.one;
        }
    }

    // 상품 입구 화면
    public void CheckPrizeOut()
    {
        prizeOut.SetActive(true);
        StartCoroutine(PrizeOutFillAmount(0.7f));
        StartCoroutine(PrizeOutScale(3f, 1.5f));
    }

    // 입구 열리는 애니메이션
    private IEnumerator PrizeOutFillAmount(float duration)
    {
        float time = 0f;
        float startFillAmount = prizeEnterImage.fillAmount;

        yield return new WaitForSeconds(2.5f);
        while (time < duration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / duration);
            prizeEnterImage.fillAmount = Mathf.Lerp(startFillAmount, 0f, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }

        prizeEnterImage.fillAmount = 0f;
    }

    // 화면 확대 애니메이션
    private IEnumerator PrizeOutScale(float delay, float duration)
    {
        yield return new WaitForSeconds(delay);

        float time = 0f;
        Vector3 startScale = new Vector3(192f, 192f, 1f);
        Vector3 endScale = new Vector3(280f, 280f, 1f);

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = scaleCurve.Evaluate(time/2f);
            prizeOut.transform.localScale = Vector3.Lerp(startScale, endScale, t);
            yield return null;
        }

        prizeOut.transform.localScale = endScale;
        CanTouchPrize();
    }

    public void CanTouchPrize()
    {
        prizeEnterImage.gameObject.SetActive(false);    // 앞에 화면 없애기 ( 버튼클릭 위해서 )
        prize.interactable = true;          // 상품 클릭 활성화
        prizeCheckText.SetActive(true);     // 상품 클릭 메세지 활성화
        prizeCheckEffect.SetActive(true);   // 상품 이펙트 활성화
        prizeHide.SetActive(false);         // 상품 가리개 지우기
    }

    public void TouchPrize()
    {
        StartCoroutine(PrizeRevealStart());
    }

    IEnumerator PrizeRevealStart()
    {
        prizeRevealChangeImage.gameObject.SetActive(true);
        while(prizeRevealChangeImage.color.a < 1)
        {
            Color color = prizeRevealChangeImage.color;
            color.a += Time.deltaTime;
            prizeRevealChangeImage.color = color;
            yield return null;
        }

        SceneManager.LoadScene("PrizeReveal");
    }
}
