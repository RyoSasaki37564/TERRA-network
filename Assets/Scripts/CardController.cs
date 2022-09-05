using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// Photon 用の名前空間を参照する
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;

/// <summary>
/// カードを制御するコンポーネント
/// カードのプレハブに追加して使う
/// </summary>
[RequireComponent(typeof(PhotonView))]
public class CardController : MonoBehaviour
{
    /// <summary>カードの内容</summary>
    Card _card = new Card(Biome.Ocean, 1);
    /// <summary>カードの画像（裏返している時は裏向きの画像）</summary>
    Image _image;
    PhotonView _view;

    bool _discarded = false;

    Biome _owner;
    public Biome Owner => _owner;

    void Awake()
    {
        _view = GetComponent<PhotonView>();
    }

    /// <summary>
    /// カードを捨てる
    /// クリックイベントから呼び出される前提で作っている
    /// </summary>
    public void Discard()
    {
        var go = GameObject.FindGameObjectWithTag("GameController");
        var cardGameManager = go.GetComponent<CardGameManager>();
        var turnManager = go.GetComponent<PunTurnManager>();
        if (_discarded)
        {
            return;
        }
        _discarded = true;
        // TODO: 自分のではないカードをクリックして捨てられないようにする
        // TODO: 捨てたカードはクリックしても何も起こらないようにする

        var gameManager = go.GetComponent<CardGameManager>();
        gameManager.Discard(_card);
        SetCardToDiscard(PhotonNetwork.LocalPlayer.ActorNumber);
        // 捨てたことを通知する
        turnManager.SendMove(null, true);
    }

    /// <summary>
    /// カードを手札にセットする
    /// </summary>
    /// <param name="playerIdx">プレイヤーのID（ActorNumber）</param>
    public void SetCardToHand(int playerIdx)
    {
        object[] parameters = { playerIdx, "Hand" };
        //自分だけ見える
        SetImage();
        _view.RPC(nameof(SetCardToDeck), RpcTarget.All, parameters);
    }

    /// <summary>
    /// カードを捨て札にセットする
    /// </summary>
    /// <param name="playerIdx">プレイヤーのID（ActorNumber）</param>
    public void SetCardToDiscard(int playerIdx)
    {
        //①メソッド名、②呼び出しのターゲット、③①の引数
        object[] parameters = { playerIdx, "Discard" };
        _view.RPC(nameof(SetCardToDeck), RpcTarget.All, parameters);
        //捨てるときに他の人にも見せる
        object[] parameters2 = { _card.Suit.ToString(), _card.Number };
        _view.RPC(nameof(SetImage), RpcTarget.All, parameters2);
    }

    /// <summary>
    /// カードをデッキにセットする
    /// </summary>
    /// <param name="playerIdx">プレイヤーのID（ActorNumber）</param>
    /// <param name="handOrDiscard">手札にするか、捨て札にするかを文字列 (Hand/Discard) で指定する</param>
    [PunRPC]
    void SetCardToDeck(int playerIdx, string handOrDiscard)
    {

        //持ち主を確定
        _owner = (Biome)playerIdx;

        var deck = GameObject.Find(handOrDiscard + " " + playerIdx);

        if (deck)
        {
            Debug.Log($"{deck.name} found.");
        }
        else
        {
            Debug.LogError($"{deck.name} not found.");
        }

        transform.SetParent(deck.transform);
        transform.localScale = Vector3.one;
        transform.localRotation = Quaternion.identity;
    }

    /// <summary>
    /// カードに画像をセットする
    /// </summary>
    /// <param name="suit"></param>
    /// <param name="number"></param>
    [PunRPC]
    public void SetImage(string suit, int number)
    {
        Biome s = (Biome)Enum.Parse(typeof(Biome), suit);
        _card = new Card(s, number);
        SetImage();
    }

    /// <summary>
    /// カードに画像をセットする
    /// </summary>
    /// <param name="card"></param>
    public void SetImage(Card card)
    {
        _card = card;
        SetImage();
    }

    /// <summary>
    /// カードに画像をセットする
    /// </summary>
    void SetImage()
    {
        _image = GetComponent<Image>();
        var sprites = Resources.LoadAll<Sprite>("Sprites/Cards");
        print($"Set sprite {_card.ToString()} to image");
        if (sprites.Length == 0) Debug.LogError("Failed to load image");

        string suitName = "";
        switch (_card.Suit)
        {
            case Biome.Snowfield:
                suitName = "Clover";
                break;
            case Biome.Ocean:
                suitName = "Diamond";
                break;
            case Biome.Savannah:
                suitName = "Heart";
                break;
            case Biome.Forest:
                suitName = "Spade";
                break;
        }


        var sprite = Array.Find<Sprite>(sprites, s => s.name == suitName + " " + _card.Number.ToString("00"));


        if (!sprite)
        {
            Debug.LogError($"not found.cardSuit{_card}");
        }
        _image.sprite = sprite;
    }
}
