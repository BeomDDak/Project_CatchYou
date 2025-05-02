using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSceneManager : MonoBehaviour
{
    // 싱글톤 구현
    public static GameSceneManager Instance;

    // 버튼 레퍼런스
    public Button rewardButton;
    public Button playButton;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // 버튼에 리스너 추가
        if (rewardButton != null) rewardButton.onClick.AddListener(() => LoadScene("Reward"));
        if (playButton != null) playButton.onClick.AddListener(() => LoadScene("Play"));

    }

    // 버튼에서 호출할 메서드
    public void GoToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadScene(string sceneName)
    {
        // 같은 씬으로 이동하는 것 방지
        if(SceneManager.GetActiveScene().name == sceneName)
        {
            return;
        }

        SceneManager.LoadScene(sceneName);
    }
}
