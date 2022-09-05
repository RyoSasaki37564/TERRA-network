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

/// <summary>
/// カードゲームのシステムを提供するコンポーネント
/// 途中で抜けられた時の処理を考慮していない
/// </summary>
[RequireComponent(typeof(PunTurnManager))]
public class CardGameManager : MonoBehaviourPunCallbacks, IPunTurnManagerCallbacks, IOnEventCallback
{
    [SerializeField]
    Text _turnText = null;

    /// <summary>山札。マスタークライアントが管理する。</summary>
    List<Card> _stock = new List<Card>();
    /// <summary>手札。各クライアントが管理する。</summary>
    List<Card> _hand = new List<Card>();
    /// <summary>捨てた札。各クライアントが管理する。</summary>
    List<Card> _discard = new List<Card>();
    /// <summary>シーン上にあるカードオブジェクト。</summary>
    List<Image> _allCardObjects = new List<Image>();

    /// <summary>自分が何番目のプレイヤーか（0スタート。途中抜けを考慮していない）</summary>
    int _playerIndex = -1;
    Biome _playerBiome;
    public Biome PlayerBiome => _playerBiome;
    /// <summary>現在何番目のプレイヤーが操作をしているか（0スタート。途中抜けを考慮していない）</summary>
    int _activePlayerIndex = -1;

    bool _isInit = false;

    /// <summary>
    /// 山札を準備し、シャッフルする
    /// </summary>
    public void InitializeCards()
    {
        Debug.Log("Initialize Game...");

        var allsuits = (Biome[])Enum.GetValues(typeof(Biome));

        foreach (var suit in allsuits)//全通りのカードを山札にAdd
        {
            for (int i = 1; i <= 13; i++)
            {
                _stock.Add(new Card(suit, i));
            }
        }

        _stock = _stock.OrderBy(c => Guid.NewGuid()).ToList();//昇順ソート
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
        // 山からカードを一枚選ぶ
        var card = _stock[UnityEngine.Random.Range(0, _stock.Count)];
        // 山から削除する
        _stock.Remove(card);
        // 対象のクライアントに引いたカードを通知する
        RaiseEventOptions eventOptions = new RaiseEventOptions();
        eventOptions.TargetActors = new int[] { actorNumber };
        SendOptions sendOptions = new SendOptions();
        sendOptions.Encrypt = true;
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

    #region IPunTurnManagerCallbacks の実装

    /// <summary>
    /// ターン開始時に呼び出される
    /// </summary>
    /// <param name="turn">ターン番号 (1, 2, 3, ...)</param>
    void IPunTurnManagerCallbacks.OnTurnBegins(int turn)
    {
        if (!_isInit)
        {
            _turnText = GameObject.Find("TurnText").GetComponent<Text>();
            _turnText.text = 0.ToString();
            _playerIndex = Array.IndexOf(PhotonNetwork.PlayerList, PhotonNetwork.LocalPlayer);
            _playerBiome = (Biome)_playerIndex + 1;
            Text biomeText = GameObject.Find("BiomeText").GetComponent<Text>();
            biomeText.text = _playerBiome.ToString();
            //Debug.LogError($"Shuffle Cards.番号：{_playerIndex}バイオーム：{_playerBiome}");
            _isInit = true;
            _activePlayerIndex = (0) % PhotonNetwork.CurrentRoom.PlayerCount;
        }

        Debug.LogFormat("ターン開始 現在のターン数：{0}", turn);

        _turnText.text = turn.ToString();
        if (turn == 1 && PhotonNetwork.IsMasterClient)
        {
            InitializeCards();

            // 最初の手札を配る
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                for (int j = 0; j < 3 + i; j++)
                {
                    Distribute(PhotonNetwork.PlayerList[i].ActorNumber);
                }
            }

            // 最初のプレイヤーに札を配る
            Distribute(PhotonNetwork.PlayerList[_activePlayerIndex].ActorNumber);
        }

        Debug.Log($"現在アクティブなプレイヤー{_activePlayerIndex},クライアントID{_playerIndex}");
        //順番のプレイヤーは操作できるように、順番以外のプレイヤーは操作できないようにする（パネルでクリックを塞いでしまえばよい）

        _allCardObjects.ForEach(c => c.raycastTarget = false);
        if (_activePlayerIndex == _playerIndex)
        {
            var cards = _allCardObjects.Where(c => c.gameObject.GetComponent<CardController>().Owner == _playerBiome);
            foreach (var card in cards)
            {
                card.raycastTarget = true;
            }
            Debug.LogError($"{_playerBiome}のターン");
        }
    }

    /// <summary>
    /// プレイヤーが行動終了したら呼び出される
    /// </summary>
    /// <param name="player">行動終了したプレイヤーの情報</param>
    /// <param name="turn"></param>
    /// <param name="move">プレイヤーが送ったメッセージ</param>
    void IPunTurnManagerCallbacks.OnPlayerFinished(Photon.Realtime.Player player, int turn, object move)
    {
        Debug.LogFormat($"OnPlayerFinished from Player: {player.ActorNumber}, move: {move}, for turn: {turn}");
        _activePlayerIndex = (_activePlayerIndex + 1) % PhotonNetwork.CurrentRoom.PlayerCount;
        // 順番のプレイヤーに札を配る
        if (PhotonNetwork.IsMasterClient)
        {
            Distribute(PhotonNetwork.PlayerList[_activePlayerIndex].ActorNumber);
        }

        // TODO: 順番のプレイヤーは操作できるように、順番以外のプレイヤーは操作できないようにする（パネルでクリックを塞いでしまえばよい）
        //if (_activePlayerIndex != _playerIndex)
        //{
        //    _allCardObjects.ForEach(c => c.raycastTarget = false);
        //}
        //else
        //{
        //    _allCardObjects.ForEach(c => c.raycastTarget = true);
        //}
    }

    /// <summary>
    /// プレイヤーが PunTurnManager.SendMove を呼び出したが、行動を終了していない時
    /// </summary>
    /// <param name="player">SendMove を呼び出したプレイヤーの情報</param>
    /// <param name="turn"></param>
    /// <param name="move">プレイヤーが送ったメッセージ</param>
    void IPunTurnManagerCallbacks.OnPlayerMove(Photon.Realtime.Player player, int turn, object move)
    {
        Debug.LogFormat($"OnPlayerMove received from Player: {player.ActorNumber}, move: {move.ToString()} for turn {turn}");
        //this.OnPlayerMove(move);
    }

    /// <summary>
    /// 参加しているプレイヤー全員が行動を終了した時に呼び出される
    /// </summary>
    /// <param name="turn">ターン番号</param>
    void IPunTurnManagerCallbacks.OnTurnCompleted(int turn)
    {
        Debug.LogFormat("OnTurnCompleted {0}", turn);
        // 新たなターンを開始する
        PunTurnManager turnManager = GameObject.FindObjectOfType<PunTurnManager>();
        turnManager.BeginTurn();
    }

    /// <summary>
    /// ターンが時間切れになった時に呼び出される
    /// </summary>
    /// <param name="turn"></param>
    void IPunTurnManagerCallbacks.OnTurnTimeEnds(int turn)
    {
        Debug.LogFormat("OnTurnTimeEnds {0}", turn);
        // 新たなターンを開始する
        PunTurnManager turnManager = GameObject.FindObjectOfType<PunTurnManager>();
        turnManager.BeginTurn();
    }

    #endregion

    #region IOnEventCallback の実装

    void IOnEventCallback.OnEvent(ExitGames.Client.Photon.EventData photonEvent)
    {
        if (photonEvent.Code > 200) return;

        if (photonEvent.Code == (byte)GameEvent.Draw)   // マスタークライアントのみ受け取る
        {
            if (PhotonNetwork.IsMasterClient)
            {
                Distribute(photonEvent.Sender);
            }
        }
        else if (photonEvent.Code == (byte)GameEvent.Distribute)//分配
        {
            string suit = ((object[])photonEvent.CustomData)[0].ToString();
            string number = ((object[])photonEvent.CustomData)[1].ToString();
            Debug.Log($"Event Received. Code: Distribute, Suit: {suit}, Number: {number}");
            Biome s = (Biome)Enum.Parse(typeof(Biome), suit);
            Card card = new Card(s, int.Parse(number));
            // カードを手札に加える
            _hand.Add(card);
            var go = PhotonNetwork.Instantiate("Card", Vector3.zero, Quaternion.identity);
            _allCardObjects.Add(go.GetComponent<Image>());
            var cardController = go.GetComponent<CardController>();
            cardController.SetImage(card);
            cardController.SetCardToHand(PhotonNetwork.LocalPlayer.ActorNumber);
        }
    }

    #endregion
}
