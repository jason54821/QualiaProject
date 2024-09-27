using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// カードの効果の種類などを定義
/// </summary>
[System.Serializable]
public class CardEffectDefine
{
    //パラメータ
    [Header("効果の種類")]
    public CardEffectDefine.CardEffect cardEffect;
    [Header("効果値")]
    public int value;

    #region 効果の種類定義部
    //カード効果定義
    public enum CardEffect
    {
        Damage,     // ダメージ
        WeaponDmg,  // 武器ダメージ
        Heal,       // 回復

        _MAX,
    }

    // 効果名(JP)
    readonly public static Dictionary<CardEffect, string> Dic_EffectName_JP = new Dictionary<CardEffect, string>()
    {
        {CardEffect.Damage,
            "ダメージ{0}"},
        {CardEffect.WeaponDmg,
            "武器ダメージ{0}"},
        {CardEffect.Heal,
            "回復{0}"},
    };

    // 効果名(EN)
    readonly public static Dictionary<CardEffect, string> Dic_EffectName_EN = new Dictionary<CardEffect, string>()
    {
        {CardEffect.Damage,
            "Damage {0}" },
        {CardEffect.WeaponDmg,
            "Weapon Dmg {0}" },
        {CardEffect.Heal,
            "Heal {0}" },
    };

    // 効果説明(JP)
    readonly public static Dictionary<CardEffect, string> Dic_EffectExplain_JP = new Dictionary<CardEffect, string>()
    {
        {CardEffect.Damage,
            "相手に {0} のダメージを与える" },
        {CardEffect.WeaponDmg,
            "相手に {0} のダメージを与える" },
        {CardEffect.Heal,
            "自分の体力を {0} 回復する" },
    };
    // 効果説明(EN)
    readonly public static Dictionary<CardEffect, string> Dic_EffectExplain_EN = new Dictionary<CardEffect, string>()
    {
        {CardEffect.Damage,
            "Deals {0} damage to the opponent" },
        {CardEffect.WeaponDmg,
            "Deals {0} damage to the opponent" },
        {CardEffect.Heal,
            "heals {0} of own HP" },
    };

    #endregion

}
