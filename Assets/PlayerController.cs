using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Photon 用の名前空間を参照する
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

public class PlayerController : MonoBehaviour, IOnEventCallback
{
    #region IOnEventCallback の実装

    void IOnEventCallback.OnEvent(ExitGames.Client.Photon.EventData photonEvent)
    {
        //if (photonEvent.Code > 200) return;
        Debug.Log("Test: " + photonEvent.Code.ToString());
    }

    #endregion
}
