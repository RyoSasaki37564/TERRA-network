using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public Suit _biome;

    public void StandUp(Suit s)
    {
        _biome = s;
    }
}
