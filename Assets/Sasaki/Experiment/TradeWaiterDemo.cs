using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TradeWaiterDemo : MonoBehaviourPunCallbacks
{
    [SerializeField] Scrollbar _scBar;

    [SerializeField] GameObject _content;

    [SerializeField] GameObject _tempCom;

    [SerializeField] List<GameObject> _commentList = new List<GameObject>();

    private void Start()
    {
        if(_tempCom.activeSelf)
        {
            _tempCom.SetActive(false);
        }
    }

    public void CommentAdd(string comment)
    {
        var x = Instantiate(_tempCom, _content.transform);
        x.SetActive(true);
        x.transform.GetChild(0).GetComponent<Text>().text = comment;
        _commentList.Add(x);
        RectTransform rec = _content.GetComponent<RectTransform>();
        rec.anchoredPosition = new Vector2(rec.anchoredPosition.x, _tempCom.GetComponent<RectTransform>().sizeDelta.y * _commentList.Count);
    }

}
