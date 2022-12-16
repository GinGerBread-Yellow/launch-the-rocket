using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class launchRocket : MonoBehaviour
{
    public Slider progressBar;
    public SpriteRenderer image;
    public Sprite[] spriteArray;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (progressBar == null)
        {
            return;
        }
        else
        {
            if (progressBar.value <= 0.5)
            {
                if (progressBar.value <= 0.33)
                {
                    image.sprite = spriteArray[0];
                }
                else
                {
                    image.sprite = spriteArray[1];
                }
            }
            else
            {
                if (progressBar.value <= 0.85)
                {
                    image.sprite = spriteArray[2];
                }
                else
                {
                    image.sprite = spriteArray[3];
                }
            }
        }
    }
}