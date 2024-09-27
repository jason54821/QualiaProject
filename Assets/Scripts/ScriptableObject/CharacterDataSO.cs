using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Character", menuName = "CharacterData")]
public class CharacterDataSO : ScriptableObject
{
    [Header("キャラ番号")]
    public int charaNum;

    [Header("ブーストレベル")]
    public FloatVariable boostLevel;

    [Header("クオリア")]
    public FloatVariable qualiaNum;

    public void InitStatus()
    {
        boostLevel.SetValue(0);
        qualiaNum.SetValue(5);
    }
}
