using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public Biome _biome;

    List<DamyRole> _list = new List<DamyRole>();

    public void StandUp(int b)
    {
        if(b >= 0 && b <= 3)
        {
            _biome = (Biome)b;
        }
        else
        {
            Debug.LogError("プレイヤーのIDが想定外のナンバーになっている");
        }
    }
}

public class DamyRole
{
    Card?[] menbers;
    int point;

    int checkCount;

    public void Check(Card c)
    {
        for(var i = 0; i < menbers.Length; i++)
        {
            switch(c.Type)
            {
                case CardType.Plant:
                    if(i == 0 && menbers[i] == null)
                    {
                        menbers[i] = c;
                        checkCount++;
                    }
                    break;
                        
            }
        }
        if (checkCount == menbers.Length)
        {
            //pointをプレイヤーの得点に
        }
    }
}