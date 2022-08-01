using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TradeWaiterDemo : MonoBehaviour
{
    [SerializeField] Scrollbar _scBar;

    [SerializeField] GameObject _content;

    [SerializeField] GameObject _tempCom;

    List<GameObject> _commentList = new List<GameObject>();

    public void CommentAdd(string comment)
    {
        var x = Instantiate(_tempCom, _content.transform);
        x.SetActive(true);
        x.transform.GetChild(0).GetComponent<Text>().text = comment;
        _commentList.Add(x);
    }
}
