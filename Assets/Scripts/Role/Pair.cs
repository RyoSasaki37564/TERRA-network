using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pair : PokarHandBase
{
    /// <summary>対象がカード単体の場合</summary>
    /// <param name="cards"></param>
    /// <returns></returns>
    public override int HandsCheck(Card cards)
    {
        return cards.Number * 2;
    }
    /// <summary>対象が役も含めた合計値の場合</summary>
    /// <param name="scoreSum"></param>
    /// <returns></returns>
    public int HandsChack(int scoreSum)
    {
        return scoreSum * 2;
    }
}
