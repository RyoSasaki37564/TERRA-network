using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pair : PokarHandBase
{
    /// <summary>�Ώۂ��J�[�h�P�̂̏ꍇ</summary>
    /// <param name="cards"></param>
    /// <returns></returns>
    public override int HandsCheck(Card cards)
    {
        return cards.Number * 2;
    }
    /// <summary>�Ώۂ������܂߂����v�l�̏ꍇ</summary>
    /// <param name="scoreSum"></param>
    /// <returns></returns>
    public int HandsChack(int scoreSum)
    {
        return scoreSum * 2;
    }
}
