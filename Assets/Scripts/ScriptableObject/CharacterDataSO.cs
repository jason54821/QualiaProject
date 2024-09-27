using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Character", menuName = "CharacterData")]
public class CharacterDataSO : ScriptableObject
{
    [Header("�L�����ԍ�")]
    public int charaNum;

    [Header("�u�[�X�g���x��")]
    public FloatVariable boostLevel;

    [Header("�N�I���A")]
    public FloatVariable qualiaNum;

    public void InitStatus()
    {
        boostLevel.SetValue(0);
        qualiaNum.SetValue(5);
    }
}
