using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardZone : MonoBehaviour
{

    //Šeí•Ï”EQÆ
    public enum ZoneType
    {
        Hand,
        PlayZone0,
        PlayZone1,
        PlayZone2,
        PlayZone3,
        PlayZone4,
        PlayZone5,
        StartZone,
        Trash
    }

    public int zoneNum;

    public ZoneType zoneType;

}
