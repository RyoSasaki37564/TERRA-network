using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCard : MonoBehaviour
{
    [SerializeField] 
    RoleBase _role = default;
    Card card;
    // Start is called before the first frame update
    void Start()
    {
        _role = new ForestOcean();
        card = new Card(Biome.Snowfield, 5);
        Debug.Log(card.Type);
    }

    public void TestForest(Card card)
    {
        var p = _role.HandsCheck(card);
        Debug.Log(p);
    }
}
