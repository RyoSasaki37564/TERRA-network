using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeDemo : MonoBehaviour
{
    [SerializeField] GameObject _tradePannel;
    public bool _isEnd;


    private void Start()
    {
        _tradePannel.SetActive(false);
    }

    //�{�^������Ăяo���O��
    public void TradeON()
    {
        _tradePannel.SetActive(true);
    }

    public void TradeOrder(int num, Biome biome, int orderPlayerID)
    {

    }
}
