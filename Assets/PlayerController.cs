using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Photon �p�̖��O��Ԃ��Q�Ƃ���
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

public class PlayerController : MonoBehaviour, IOnEventCallback
{
    #region IOnEventCallback �̎���

    void IOnEventCallback.OnEvent(ExitGames.Client.Photon.EventData photonEvent)
    {
        //if (photonEvent.Code > 200) return;
        Debug.Log("Test: " + photonEvent.Code.ToString());
    }

    #endregion
}
