using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;

public class GameManager : MonoBehaviourPunCallbacks, IPunTurnManagerCallbacks, IOnEventCallback
{
    /// <summary>山札。マスタークライアントが管理する。</summary>
    List<Card> _stock = new List<Card>();
    /// <summary>手札。各クライアントが管理する。</summary>
    List<Card> _hand = new List<Card>();
    /// <summary>捨てた札。各クライアントが管理する。</summary>
    List<Card> _discard = new List<Card>();

    void Init()
    {
        var allBiome = (Biome[])Enum.GetValues(typeof(Biome));
        foreach (var b in allBiome)
        {
            for (int i = 0;i<=13;i++)
            {
                _stock.Add(new Card(b, i));
            }
        }
        _stock = _stock.OrderBy(c => Guid.NewGuid()).ToList();
    }

    /// <summary>
    /// 山札から一枚カードを引く
    /// </summary>
    /// <param name="actorNumber"></param>
    public void Draw(int actorNumber)
    {
        RaiseEventOptions eventOptions = new RaiseEventOptions();
        eventOptions.Receivers = ReceiverGroup.MasterClient;
        SendOptions sendOptions = new SendOptions();
        sendOptions.Encrypt = true;
        PhotonNetwork.RaiseEvent((byte)GameEvent.Draw, null, eventOptions, sendOptions);
    }

    /// <summary>
    /// マスタークライアントでのみ呼び出される。対象のクライアントに札を配布する。
    /// </summary>
    /// <param name="actorNumber"></param>
    void Distribute(int actorNumber)
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        // 山からカードを一枚選ぶ
        var card = _stock[UnityEngine.Random.Range(0, _stock.Count)];

        // 山から削除する
        _stock.Remove(card);

        // 対象のクライアントに引いたカードを通知する
        RaiseEventOptions eventOptions = new RaiseEventOptions();
        eventOptions.TargetActors = new int[] { actorNumber };

        //暗号化
        SendOptions sendOptions = new SendOptions();
        sendOptions.Encrypt = true;

        //イベント登録
        object[] content = new object[] { card.Suit.ToString(), card.Number.ToString() };
        Debug.Log($"Raise Event ID:{GameEvent.Distribute.ToString()}, Suit: {card.Suit.ToString()}, Number: {card.Number}, TargetActor: {actorNumber}");
        PhotonNetwork.RaiseEvent((byte)GameEvent.Distribute, content, eventOptions, sendOptions);
    }

    /// <summary>
    /// カードを捨てる
    /// </summary>
    /// <param name="card"></param>
    public void Discard(Card card)
    {
        _hand.Remove(card);
        _discard.Add(card);
    }

    public void OnEvent(EventData photonEvent)
    {
        throw new NotImplementedException();
    }

    //PUNTurnManagerのコールバック処理
    public void OnPlayerFinished(Player player, int turn, object move)
    {
        throw new NotImplementedException();
    }

    public void OnPlayerMove(Player player, int turn, object move)
    {
        throw new NotImplementedException();
    }

    public void OnTurnBegins(int turn)
    {
        throw new NotImplementedException();
    }

    public void OnTurnCompleted(int turn)
    {
        throw new NotImplementedException();
    }

    public void OnTurnTimeEnds(int turn)
    {
        throw new NotImplementedException();
    }
}
