using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PerspectiveSwitcher : MonoBehaviour
{
    public bool orthoOn;

    void Start()
    {
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            List<Folding> foldings = this.gameObject.GetComponent<Game>().ListFolding;

            orthoOn = !orthoOn;

            if (orthoOn)
            {
                foreach (Folding folding in foldings)
                {
                    folding.animationSens = 1;
                    folding.startAnimation = Time.realtimeSinceStartup;
                }
            }
            else
            {
                foreach (Folding folding in foldings)
                {
                    folding.animationSens = -1;
                    folding.startAnimation = Time.realtimeSinceStartup;
                }
            }
        }
    }
}