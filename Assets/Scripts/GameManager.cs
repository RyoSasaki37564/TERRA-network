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
    /// <summary>�R�D�B�}�X�^�[�N���C�A���g���Ǘ�����B</summary>
    List<Card> _stock = new List<Card>();
    /// <summary>��D�B�e�N���C�A���g���Ǘ�����B</summary>
    List<Card> _hand = new List<Card>();
    /// <summary>�̂Ă��D�B�e�N���C�A���g���Ǘ�����B</summary>
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
    /// �R�D����ꖇ�J�[�h������
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
    /// �}�X�^�[�N���C�A���g�ł̂݌Ăяo�����B�Ώۂ̃N���C�A���g�ɎD��z�z����B
    /// </summary>
    /// <param name="actorNumber"></param>
    void Distribute(int actorNumber)
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        // �R����J�[�h���ꖇ�I��
        var card = _stock[UnityEngine.Random.Range(0, _stock.Count)];

        // �R����폜����
        _stock.Remove(card);

        // �Ώۂ̃N���C�A���g�Ɉ������J�[�h��ʒm����
        RaiseEventOptions eventOptions = new RaiseEventOptions();
        eventOptions.TargetActors = new int[] { actorNumber };

        //�Í���
        SendOptions sendOptions = new SendOptions();
        sendOptions.Encrypt = true;

        //�C�x���g�o�^
        object[] content = new object[] { card.Suit.ToString(), card.Number.ToString() };
        Debug.Log($"Raise Event ID:{GameEvent.Distribute.ToString()}, Suit: {card.Suit.ToString()}, Number: {card.Number}, TargetActor: {actorNumber}");
        PhotonNetwork.RaiseEvent((byte)GameEvent.Distribute, content, eventOptions, sendOptions);
    }

    /// <summary>
    /// �J�[�h���̂Ă�
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

    //PUNTurnManager�̃R�[���o�b�N����
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
