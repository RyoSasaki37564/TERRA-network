using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TradeDemo : MonoBehaviourPunCallbacks, IOnEventCallback
{
    [SerializeField] GameObject _tradePannel;

    int _targetCardBiome;
    int _targetCardNum;
    int _targetPlayerNum;

    [SerializeField] Dropdown[] _dDs;
    [SerializeField] Text _onOffButtonText;
    TradeWaiterDemo _twd;

    private void Start()
    {
        _tradePannel.SetActive(false);
        _twd = FindObjectOfType<TradeWaiterDemo>().GetComponent<TradeWaiterDemo>();
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cardNum"></param>
    /// <param name="cardBiome"></param>
    /// <param name="orderPlayerID"></param>
    public void TradeOrder()
    {
        // 欲しいカードを一枚選ぶ
        var card = new Card((Biome)_targetCardBiome, _targetCardNum);
        // 対象のクライアントに欲しいカードを通知する
        RaiseEventOptions eventOptions = new RaiseEventOptions();
        eventOptions.TargetActors = new int[] { _targetPlayerNum };
        SendOptions sendOptions = new SendOptions();
        //sendOptions.Encrypt = true;
        object[] content = new object[] { card.Suit.ToString(), card.Number.ToString() };
        Debug.Log($"Raise Event ID:{GameEvent.TradeOrder.ToString()}, Suit: {card.Suit.ToString()}, Number: {card.Number}, TargetActor: {_targetPlayerNum}");
        PhotonNetwork.RaiseEvent((byte)GameEvent.TradeOrder, content, eventOptions, sendOptions);
    }

    void IOnEventCallback.OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code > 200) return;

        if (photonEvent.Code == (byte)GameEvent.TradeOrder)
        {
            string suit = ((object[])photonEvent.CustomData)[0].ToString();
            string number = ((object[])photonEvent.CustomData)[1].ToString();
            Debug.Log($"Event Received. Code: Distribute, Suit: {suit}, Number: {number}");
            Biome s = (Biome)Enum.Parse(typeof(Biome), suit);
            Card card = new Card(s, int.Parse(number));

            if (card.Number == 13)
            {
                _twd.CommentAdd(" Joker ");
                Debug.Log(" Joker ");
            }
            else
            {
                string b, n;
                if (s != 0)
                {
                    b = $"{ s - 1}";
                }
                else
                {
                    b = "All";
                }
                if (card.Number != 0)
                {
                    n = $"{card.Number}";
                }
                else
                {
                    n = "All";
                }
                _twd.CommentAdd($"{b} / {n} ");
                Debug.Log($"{b} / {n} ");
            }

        }
    }

    public void OrderTest()
    {
        if (_targetCardNum == 13)
        {
            _twd.CommentAdd(" Joker ");
            Debug.Log(" Joker ");
        }
        else
        {
            string b, n;
            if (_targetCardBiome != 0)
            {
                b = $"{ (Biome)_targetCardBiome - 1}";
            }
            else
            {
                b = "All";
            }
            if (_targetCardNum != 0)
            {
                n = $"{_targetCardNum}";
            }
            else
            {
                n = "All";
            }
            _twd.CommentAdd($"{b} / {n} ");
            Debug.Log($"{b} / {n} ");
        }
    }

    public void OnBiomeChanged()
    {
        _targetCardBiome = _dDs[0].value;
    }

    public void OnNumChanged()
    {
        _targetCardNum = _dDs[1].value;
    }

    public void OnPlayerNumChanged()
    {
        _targetPlayerNum = _dDs[2].value;
    }
}
