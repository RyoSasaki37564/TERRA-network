using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestOcean : RoleBase
{
    int _suitCount = 0;
    int _suitCountLine = 0;
    public ForestOcean()
    {
        if(!_init)
        {
            _point = 55;
            _memberCountLine = 5;
            _init = true;
        }
    }
    public override int HandsCheck(Card card)
    {
        for (int i = 0; i < _cards.Count;i++)
        {
            switch (card.Type)
            {
                case CardType.Plant:
                    if (_cards[i] == null)
                    {
                        _cards[i] = card;
                    }
                    break;
                default:break;
            }

            
            if(_memberCount == _memberCountLine)
            {
                var arrayBase = _cards.ToArray();
                _cardsCopy.CopyTo(arrayBase, _cardsCopy.Count);
                var unUse = _cardsCopy.Where(x => x.usedByRole == false).ToList();
                //TODO:‚±‚±‚É‰Á“_ˆ—‚ð‘‚­
                unUse.ForEach(x => x.usedByRole = true);
                _memberCount = 0;
                return _point;
            }
        }
        return 0;
    }
}
