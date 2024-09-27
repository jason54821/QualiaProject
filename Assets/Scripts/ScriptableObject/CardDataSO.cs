using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Card", menuName = "CardData")]
public class CardDataSO : ScriptableObject
{
    [Header("対応キャラ番号")]
    public int cardCharaNum;

    [Header("通し番号")]
    public int serialNum;

    [Header("カード名")]
    public string cardName_JP;

    [Header("アイコン")]
    public Sprite iconSprite;
    
 
    [Header("効果リスト")]
    [SerializeField] public List<CardEffectDefine> effectList = new List<CardEffectDefine>();

    [Header("強度")]
    public int force;
}
