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

    [SerializeField, Tooltip("クライアントのスコアを表示するテキスト")]
    Text _scoreText = null;
    //プレイヤーのスコア
    int _score = 0;
    [SerializeField,Tooltip("役判定のコンポーネント")]
    PokerJudgeManager _judgeManager = null;
    [SerializeField, Tooltip("勝利判定のテキスト")]
    Text _resultText = null;

    /// <summary>自分が何番目のプレイヤーか（0スタート。途中抜けを考慮していない）</summary>
    int _playerIndex = -1;
    /// <summary>クライアントのバイオーム</summary>
    Biome _playerBiome;
    public Biome PlayerBiome => _playerBiome;
    /// <summary>現在何番目のプレイヤーが操作をしているか（0スタート。途中抜けを考慮していない）</summary>
    int _activePlayerIndex = -1;

    bool _isInit = false;
    public bool IsClientTurn { get { return _activePlayerIndex == _playerIndex; } }

    /// <summary>
    /// 山札を準備し、シャッフルする
    /// </summary>
    public void InitializeCards()
    {
        Debug.Log("ゲーム初期化");

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
        Debug.Log($"カードを配った:{GameEvent.Distribute.ToString()}, 絵柄: {card.Suit.ToString()}, 番号: {card.Number}, ターゲットID: {actorNumber}");
        PhotonNetwork.RaiseEvent((byte)GameEvent.Distribute, content, eventOptions, sendOptions);
    }

    /// <summary>
    /// カードを捨てる
    /// </summary>
    /// <param name="card"></param>
    public void Discard(Card card)
    {
        //役判定してスコアを加算。
        Debug.Log($"足される前{_score}");
        _score += _judgeManager.Judge(card);
        Debug.Log($"足された後{_score}");
        _scoreText.text = _score.ToString();
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
            _scoreText.text = _score.ToString();
            //役判定クラスのインスタンス取得
            _judgeManager = InstaceComponent<PokerJudgeManager>("JudegeManager");

            //ターンテキストのインスタンスを取得
            _turnText = GameObject.Find("TurnText").GetComponent<Text>();
            if (!_turnText)
            {
                _turnText = InstaceComponent<Text>("TurnText");
            }
            _turnText.text = 0.ToString();

            _resultText = GameObject.Find("ResultText").GetComponent<Text>();
            if (!_resultText)
            {
                _resultText = InstaceComponent<Text>("ResultText");
            }
            _resultText.gameObject.SetActive(false);

            //PlayerIndexの初期化
            _playerIndex = Array.IndexOf(PhotonNetwork.PlayerList, PhotonNetwork.LocalPlayer);

            //バイオームテキストの初期化
            _playerBiome = (Biome)_playerIndex + 1;
            Text biomeText = GameObject.Find("BiomeText").GetComponent<Text>();
            if (!biomeText)
            {
                biomeText = InstaceComponent<Text>("BiomeText");
            }
            biomeText.text = _playerBiome.ToString();
            _activePlayerIndex = (0) % PhotonNetwork.CurrentRoom.PlayerCount;

            _isInit = true;          
        }

        Debug.LogFormat("ターン開始 現在のターン数：{0}", turn);

        //ターンテキストの更新
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
        //順番のプレイヤーは操作できるように、順番以外のプレイヤーは操作できないようにする（パネルでクリックを塞いでしまえばよい）

        _allCardObjects.ForEach(c => c.raycastTarget = false);
        if (_activePlayerIndex == _playerIndex)
        {
            var cards = _allCardObjects.Where(c => c.gameObject.GetComponent<CardController>().Owner == _playerBiome);
            foreach (var card in cards)
            {
                card.raycastTarget = true;
            }
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
        Debug.LogFormat($"行動終了したプレイヤー: {player.ActorNumber}, move: {move}, ターン数: {turn}");
        _activePlayerIndex = (_activePlayerIndex + 1) % PhotonNetwork.CurrentRoom.PlayerCount;
        // 順番のプレイヤーに札を配る
        if (PhotonNetwork.IsMasterClient)
        {
            Distribute(PhotonNetwork.PlayerList[_activePlayerIndex].ActorNumber);
        }
    }

    /// <summary>
    /// プレイヤーが PunTurnManager.SendMove を呼び出したが、行動を終了していない時
    /// </summary>
    /// <param name="player">SendMove を呼び出したプレイヤーの情報</param>
    /// <param name="turn"></param>
    /// <param name="move">プレイヤーが送ったメッセージ</param>
    void IPunTurnManagerCallbacks.OnPlayerMove(Photon.Realtime.Player player, int turn, object move)
    {
        Debug.LogFormat($"行動を送ったプレイヤー: {player.ActorNumber}, move: {move.ToString()} ターン数 {turn}");
        //this.OnPlayerMove(move);
    }

    /// <summary>
    /// 参加しているプレイヤー全員が行動を終了した時に呼び出される
    /// </summary>
    /// <param name="turn">ターン番号</param>
    void IPunTurnManagerCallbacks.OnTurnCompleted(int turn)
    {
        Debug.LogFormat("{0}ターン目が終了", turn);
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
        Debug.LogFormat("{0}ターン目が時間切れで終了。", turn);
        // 新たなターンを開始する
        PunTurnManager turnManager = GameObject.FindObjectOfType<PunTurnManager>();
        if (_stock.Count<=0)
        {
            //TODO各クライアントの点数を比較してTextに表示する
        }
        else
        {
            turnManager.BeginTurn();
        }      
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
            Debug.Log($"カードを配った, 絵柄: {suit}, 番号: {number}");
            Biome s = (Biome)Enum.Parse(typeof(Biome), suit);
            Card card = new Card(s, int.Parse(number));
            // カードを手札に加える
            _hand.Add(card);
            //Photonで全てのクライアントでカードを生成
            var go = PhotonNetwork.Instantiate("Card", Vector3.zero, Quaternion.identity);
            _allCardObjects.Add(go.GetComponent<Image>());
            var cardController = go.GetComponent<CardController>();
            cardController.SetImage(card);
            cardController.SetCardToHand(PhotonNetwork.LocalPlayer.ActorNumber);
        }
    }

    componentType InstaceComponent<componentType>(string objectName) where componentType :Component
    {
        var go  = new GameObject(objectName);
        return go.AddComponent<componentType>();
    }

    #endregion
}
