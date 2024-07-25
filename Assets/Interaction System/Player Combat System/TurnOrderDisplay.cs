using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnOrderDisplay : MonoBehaviour
{
    public Image[] turnOrderImages; // Assign these in the Inspector

    public void SetTurnOrder(List<Sprite> turnOrder)
    {
        for (int i = 0; i < turnOrderImages.Length; i++)
        {
            if (i < turnOrder.Count && turnOrder[i] != null)
            {
                turnOrderImages[i].sprite = turnOrder[i];
                turnOrderImages[i].gameObject.SetActive(true);
            }
            else
            {
                turnOrderImages[i].gameObject.SetActive(false);
            }
        }
    }
}