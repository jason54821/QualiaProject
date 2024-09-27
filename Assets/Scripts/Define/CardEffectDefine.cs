using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �J�[�h�̌��ʂ̎�ނȂǂ��`
/// </summary>
[System.Serializable]
public class CardEffectDefine
{
    //�p�����[�^
    [Header("���ʂ̎��")]
    public CardEffectDefine.CardEffect cardEffect;
    [Header("���ʒl")]
    public int value;

    #region ���ʂ̎�ޒ�`��
    //�J�[�h���ʒ�`
    public enum CardEffect
    {
        Damage,     // �_���[�W
        WeaponDmg,  // ����_���[�W
        Heal,       // ��

        _MAX,
    }

    // ���ʖ�(JP)
    readonly public static Dictionary<CardEffect, string> Dic_EffectName_JP = new Dictionary<CardEffect, string>()
    {
        {CardEffect.Damage,
            "�_���[�W{0}"},
        {CardEffect.WeaponDmg,
            "����_���[�W{0}"},
        {CardEffect.Heal,
            "��{0}"},
    };

    // ���ʖ�(EN)
    readonly public static Dictionary<CardEffect, string> Dic_EffectName_EN = new Dictionary<CardEffect, string>()
    {
        {CardEffect.Damage,
            "Damage {0}" },
        {CardEffect.WeaponDmg,
            "Weapon Dmg {0}" },
        {CardEffect.Heal,
            "Heal {0}" },
    };

    // ���ʐ���(JP)
    readonly public static Dictionary<CardEffect, string> Dic_EffectExplain_JP = new Dictionary<CardEffect, string>()
    {
        {CardEffect.Damage,
            "����� {0} �̃_���[�W��^����" },
        {CardEffect.WeaponDmg,
            "����� {0} �̃_���[�W��^����" },
        {CardEffect.Heal,
            "�����̗̑͂� {0} �񕜂���" },
    };
    // ���ʐ���(EN)
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
