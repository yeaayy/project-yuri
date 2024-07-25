using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DailyLogEntry
{
    public bool isJob;
    public string jobDescription;
    public string details;
    public string timePeriod;
    public float pay;
    public string location;
}