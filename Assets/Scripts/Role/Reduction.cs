using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reduction : PokarHandBase
{
    int _predatorCount = 0;
    int _plantCount = 0;
    int _predatorCountLine = 0;
    int _plantCountLine = 0;
    public Reduction()
    {
        if (!_init)
        {
            _point = 35;
            _memberCountLine = 3;
            _plantCountLine = 2;
            _predatorCountLine = 1;
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
                    if(_plantCount<_plantCountLine) _plantCount++;
                    break;
                case CardType.Predator:
                    _cards.Add(card);
                    _memberCount++;
                    if (_predatorCount < _predatorCountLine) _predatorCount++;
                    break;
                case CardType.PredatorTheTop:
                    _cards.Add(card);
                    _memberCount++;
                    if (_predatorCount < _predatorCountLine) _predatorCount++;
                    break;
                default: break;
            }
        }
        else
        {
            if (_cards.Contains(card))
            {
                switch (card.Type)
                {
                    case CardType.Plant:
                        _cards.Add(card);
                        _memberCount++;
                        break;
                    case CardType.Predator:
                        _cards.Add(card);
                        _memberCount++;
                        break;
                    case CardType.PredatorTheTop:
                        _cards.Add(card);
                        _memberCount++;
                        break;
                    default: break;
                }
            }

        }
        return _point;
    }
}
