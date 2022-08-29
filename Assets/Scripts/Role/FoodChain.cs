using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodChain : PokarHandBase
{
    bool[] _inCard = new bool[4];
    public FoodChain()
    {
        if(!_init)
        {
            _point = 110;
            _memberCountLine = 4;
        }
    }
    public override int HandsCheck(Card card)
    {
        if (_cards.Count == 0)
        {
            _cards.Add(card);
            _memberCount++;
        }
        else
        {
            if (!_cards.Contains(card))
            {
                bool inCard = _inCard[(int)card.Type];
                if (!inCard)
                {
                    _cards.Add(card);
                    _memberCount++;
                    _inCard[(int)card.Type] = true;
                    if(_memberCount == _memberCountLine)
                    {
                        _memberCount = 0;
                        for (int i = 0; i < _inCard.Length; i++)
                        {
                            _inCard[i] = false;
                        }
                        return _point;
                    }
                }
            }
        }
        return 0;
    }
}
