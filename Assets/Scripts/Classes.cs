public struct Card
{
	public Biorm Suit;
	public int Number; 
	public CardType CardType;

	public Card(Biorm suit, int number)
    {
		Suit = suit;
		Number = number;
		CardType = CardType.Plant;
		if(Number > 4 && Number <= 7)
        {
			CardType = CardType.Prey;
		}
		else if(Number > 7 && Number <= 9)
        {
			CardType = CardType.Predator;
        }
		else if(Number == 10)
        {
			CardType = CardType.PredatorTheTop;
        }
		else if(Number == 11)
        {
			CardType = CardType.Disaster;
        }
		else if(Number == 12)
        {
			CardType = CardType.Ark;
        }
		else if(Number == 14)
        {

        }
	}

    public override string ToString()
    {
        return $"{Suit.ToString()} {Number.ToString()}";
    }
}

public enum Biorm
{
	Savannah,
	Snowfield,
	Forest,
	Ocean
}
public enum CardType
{
	/// <summary> 植物 </summary>
	Plant,
	/// <summary> 被食者 </summary>
	Prey, 
	/// <summary> 捕食者 </summary>
	Predator, 
	/// <summary> 頂点捕食者 </summary>
	PredatorTheTop, 
	/// <summary> 災害 </summary>
	Disaster, 
	/// <summary> 方舟 </summary>
	Ark,
	/// <summary> ジョーカー summary>
    Joker
}


/// <summary>
/// イベント ID 1 とか 2 は PunTurnManager が使っているのでよける
/// </summary>
public enum GameEvent : byte
{
	Start = 10,
	Draw = 20,
	Distribute = 30,
	Discard = 40,
	End = 50,
}