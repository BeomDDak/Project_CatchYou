using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideUpMenu : MonoBehaviour
{
    public Animator menuAnimator;
    private bool isMenuVisible = false;

    public void ToggleMenu()
    {
        isMenuVisible = !isMenuVisible;
        menuAnimator.SetBool("IsVisible", isMenuVisible);
    }
}
