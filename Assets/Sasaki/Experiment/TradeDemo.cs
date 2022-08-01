using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TradeDemo : MonoBehaviour
{
    [SerializeField] GameObject _tradePannel;

    int _targetBiome;
    int _targetNum;

    [SerializeField] Dropdown[] _dDs;
    [SerializeField] Text _onOffButtonText;

    private void Start()
    {
        _tradePannel.SetActive(false);
    }

    //ボタンから呼び出す前提
    public void TradeONOFF()
    {
        if(!_tradePannel.activeSelf)
        {
            foreach(var d in _dDs)
            {
                d.value = 0;
            }
            _tradePannel.SetActive(true);
            _onOffButtonText.text = "閉じる";
        }
        else
        {
            _tradePannel.SetActive(false);
            _onOffButtonText.text = "取引";
        }
    }

    public void TradeOrder(int num, Biome biome, int orderPlayerID)
    {

    }

    public void OrderTest()
    {
        TradeWaiterDemo twd = FindObjectOfType<TradeWaiterDemo>().GetComponent<TradeWaiterDemo>();
        if (_targetNum == 13)
        {
            twd.CommentAdd(" Joker ");
            Debug.Log(" Joker ");
        }
        else
        {
            string b, n;
            if (_targetBiome != 0)
            {
                b = $"{ (Biome)_targetBiome - 1}";
            }
            else
            {
                b = "All";
            }
            if (_targetNum != 0)
            {
                n = $"{_targetNum}";
            }
            else
            {
                n = "All";
            }
            twd.CommentAdd($"{b} / {n} ");
            Debug.Log($"{b} / {n} ");
        }
    }

    public void OnBiomeChanged()
    {
        _targetBiome = _dDs[0].value;
    }

    public void OnNumChanged()
    {
        _targetNum = _dDs[1].value;
    }
}
