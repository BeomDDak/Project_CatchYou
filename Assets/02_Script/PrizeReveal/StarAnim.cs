using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarAnim : MonoBehaviour
{
    private Animator starAnimators;
    private float timer;
   

    void Start()
    {
        starAnimators = this.GetComponent<Animator>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 5f)
        {
            timer = 0.0f;
        }
        starAnimators.SetFloat("timer", timer);
    }
}
