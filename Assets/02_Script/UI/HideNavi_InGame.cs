using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideNavi_InGame : MonoBehaviour
{
    private GameObject gameManager;

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
    }

    public void OpenPanel()
    {
        GameSceneManager.Instance.transform.Find("Canvas/Navigator_Panel").gameObject.SetActive(false);
        gameManager.GetComponent<GameManager>().openPlayPanal = true;
    }
    public void ClosePanel()
    {
        GameSceneManager.Instance.transform.Find("Canvas/Navigator_Panel").gameObject.SetActive(true);
        gameManager.GetComponent<GameManager>().openPlayPanal = false;
    }
}
