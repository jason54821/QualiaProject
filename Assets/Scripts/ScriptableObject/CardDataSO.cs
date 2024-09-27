using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Card", menuName = "CardData")]
public class CardDataSO : ScriptableObject
{
    [Header("�Ή��L�����ԍ�")]
    public int cardCharaNum;

    [Header("�ʂ��ԍ�")]
    public int serialNum;

    [Header("�J�[�h��")]
    public string cardName_JP;

    [Header("�A�C�R��")]
    public Sprite iconSprite;
    
 
    [Header("���ʃ��X�g")]
    [SerializeField] public List<CardEffectDefine> effectList = new List<CardEffectDefine>();

    [Header("���x")]
    public int force;
}
