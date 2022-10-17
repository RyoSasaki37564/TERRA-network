using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokerJudgeManager : MonoBehaviour
{
    PokarHandBase[] _pokarJudges = new PokarHandBase[] { new Pair(), new ForestOcean(), new FoodChain(), new Reduction() };

    public int Judge(Card card)
    {
        int addScore = 0;
        foreach (var judge in _pokarJudges)
        {
            addScore += judge.HandsCheck(card);
        }
        return addScore;
    }
}
