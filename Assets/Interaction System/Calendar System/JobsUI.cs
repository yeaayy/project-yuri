using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class JobsUI : MonoBehaviour
{
    public TMP_Text chagallCafePayText;
    public TMP_Text beblueVPayText;
    public TMP_Text screenshotPayText;

    void Start()
    {
        chagallCafePayText.text = "¥2,500+";
        beblueVPayText.text = "¥3,500+";
        screenshotPayText.text = "¥5,000+";
    }
}