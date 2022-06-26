using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Photon —p‚Ì–¼‘O‹óŠÔ‚ðŽQÆ‚·‚é
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

public class PlayerController : MonoBehaviour, IOnEventCallback
{
    #region IOnEventCallback ‚ÌŽÀ‘•

    void IOnEventCallback.OnEvent(ExitGames.Client.Photon.EventData photonEvent)
    {
        //if (photonEvent.Code > 200) return;
        Debug.Log("Test: " + photonEvent.Code.ToString());
    }

    #endregion
}
