using UnityEngine;
using UnityEngine.UI;

public class HideNavi : MonoBehaviour
{ 
    public void OpenPanel()
    {
        GameSceneManager.Instance.transform.Find("Canvas/Navigator_Panel").gameObject.SetActive(false);
    }

    public void ClosePanel()
    {
        GameSceneManager.Instance.transform.Find("Canvas/Navigator_Panel").gameObject.SetActive(true);
    }
}
