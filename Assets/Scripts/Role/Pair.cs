using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pair : PokarHandBase
{
    /// <summary>対象がカード単体の場合</summary>
    /// <param name="card"></param>
    /// <returns></returns>
    public override int HandsCheck(Card card)
    {
        var gm = GameObject.FindObjectOfType<CardGameManager>();
        if (card.Suit == gm?.PlayerBiome)
        {
            return card.Number * 2;
        }
        return 0;
    }

    /// <summary>対象が役も含めた合計値の場合</summary>
    /// <param name="scoreSum"></param>
    /// <returns></returns>
    public int HandsChack(int scoreSum)
    {
        return scoreSum * 2;
    }


}
