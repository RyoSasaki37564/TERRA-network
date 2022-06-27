using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardController : MonoBehaviour
{
    Card _card = new Card(Suit.Spade, 1);
    Image _image;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SetImage(Card card)
    {
        _card = card;
        SetImage();
    }

    void SetImage()
    {
        _image = GetComponent<Image>();
        var sprites = Resources.LoadAll<Sprite>("Sprites/Cards");
        print($"Set sprite {_card.ToString()} to image");
        if (sprites.Length == 0) Debug.LogError("Failed to load image");
        var sprite = Array.Find<Sprite>(sprites, s => s.name == _card.Suit.ToString() + " " + _card.Number.ToString("00"));
        if (!sprite)
        {
            Debug.LogError("not found");
        }
        _image.sprite = sprite;
    }
}
