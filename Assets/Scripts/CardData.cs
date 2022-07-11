using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardData
{
    public Biome _biome;
    public CardType _type;
    [Range(1, 10)] public int _cardNum;
    public bool _isPlay;
}
public enum Biome
{
    Savannah = 0,
    Snowfield = 1,
    Forest = 2,
    Ocean = 3
}

public enum CardType
{
    Normal,
    Disaster,
    Ark
}
