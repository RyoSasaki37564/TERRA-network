using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCard : MonoBehaviour
{
    [SerializeField] UnityEngine.UI.Button[] button = default;
    PokarHandBase _role1 = default;
    PokarHandBase _role2 = default;
    int sum = 0;
    Card[] _cards =
    {
        new Card(Biome.Savannah,1),
        new Card(Biome.Savannah,2),
        new Card(Biome.Savannah,5),
        new Card(Biome.Savannah,8),
        new Card(Biome.Savannah,10),
        new Card(Biome.Ocean,1),
        new Card(Biome.Ocean,2),
        new Card(Biome.Ocean,5),
        new Card(Biome.Ocean,8),
        new Card(Biome.Ocean,10),
        new Card(Biome.Snowfield,1),
        new Card(Biome.Snowfield,2),
        new Card(Biome.Snowfield,5),
        new Card(Biome.Snowfield,8),
        new Card(Biome.Snowfield,10),
        new Card(Biome.Forest,1),
        new Card(Biome.Forest,2),
        new Card(Biome.Forest,5),
        new Card(Biome.Forest,8),
        new Card(Biome.Forest,10),
    };
    // Start is called before the first frame update
    void Start()
    {
        _role1 = new ForestOcean();
        for (int i = 0; i < _cards.Length; i++)
        {
            var card = _cards[i];
            button[i].onClick.AddListener(() =>
            {
                TestForest(card);
            });
        }

    }

    public void TestForest(Card card)
    {
        sum += _role1.HandsCheck(card);
        Debug.Log(sum);
        //Debug.Log(card.Type);
    }
}
