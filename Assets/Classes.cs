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
/// �C�x���g ID 1 �Ƃ� 2 �� PunTurnManager ���g���Ă���̂ł悯��
/// </summary>
public enum GameEvent : byte
{
	Start = 10,
	Draw = 20,
	Distribute = 30,
	Discard = 40,
	End = 50,
}