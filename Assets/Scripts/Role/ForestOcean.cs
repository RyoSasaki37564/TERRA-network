using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestOcean : PokarHandBase
{
    int _suitCount = 0;
    int _suitCountLine = 0;
    public ForestOcean()
    {
        if (!_init)
        {
            _point = 55;
            _memberCountLine = 5;
            _init = true;
        }
    }
    public override int HandsCheck(Card card)
    {
        if (_cards.Count == 0)
        {
            switch (card.Type)
            {
                case CardType.Plant:
                    _cards.Add(card);
                    _memberCount++;
                    break;
                default: break;
            }
        }
        else
        {
            //for (int i = 0; i < _cards.Count; i++)
            //{
                switch (card.Type)
                {
                    case CardType.Plant:
                        if (!_cards.Contains(card))
                        {
                            _cards.Add(card); ;
                            _memberCount++;
                        }
                        break;
                    default: break;
                }

                if (_memberCount == _memberCountLine)
                {
                    _memberCount = 0;
                    return _point;
                }
            //}
        }
        return 0;
    }
}
