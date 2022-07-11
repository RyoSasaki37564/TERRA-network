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
