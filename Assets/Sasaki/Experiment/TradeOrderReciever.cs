using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TradeOrderReciever : MonoBehaviour
{
    [SerializeField] GameObject _orderContentsPanel;
    [SerializeField] Text _orderSuitText;
    [SerializeField] Text _orderNumText;
    string _suit;
    string _num;

    public void PanelOpen()
    {
        if(_orderContentsPanel.activeSelf)
        {
            _orderContentsPanel.SetActive(false);
            if (_orderSuitText.text == _suit)
            {
                _orderContentsPanel.SetActive(true);
                _orderSuitText.text = _suit;
                _orderNumText.text = _num;
            }
        }
        else
        {
            _orderContentsPanel.SetActive(true);
            _orderSuitText.text = _suit;
            _orderNumText.text = _num;
        }
    }

    public void OrderMake(string s, string n)
    {
        _suit = s;
        _num = n;
    }
}
