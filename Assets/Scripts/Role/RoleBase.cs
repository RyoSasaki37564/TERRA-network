using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PokarHandBase
{
    protected List<Card> _cards = new List<Card>();
    protected List<Card> _cardsCopy;
    protected bool _used = false;
    protected bool _init = false;
    protected int _point;
    protected int _memberCount = 0;
    protected int _memberCountLine = 0;

    void PlayCard(Card card)
    {

    }
    public abstract int HandsCheck(Card cards);
}
