public struct Card
{
	public Suit Suit;
	public int Number;

	public Card(Suit suit, int number)
    {
		Suit = suit;
		Number = number;
    }

    public override string ToString()
    {
        return $"{Suit.ToString()} {Number.ToString()}";
    }
}

public enum Suit
{
	Clover,
	Diamond,
	Heart,
	Spade,
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