using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public Biome _biome;

    public void StandUp(Biome b)
    {
        _biome = b;
    }
}
