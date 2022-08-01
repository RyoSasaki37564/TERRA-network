public class Card
{
	public Biome Suit;
	public int Number; 
	public CardType Type;
	public bool usedByRole;

	public Card(Biome suit, int number)
    {
		Suit = suit;
		Number = number;
		Type = CardType.Plant;
		if(Number > 4 && Number <= 7)
        {
			Type = CardType.Prey;
		}
		else if(Number > 7 && Number <= 9)
        {
			Type = CardType.Predator;
        }
		else if(Number == 10)
        {
			Type = CardType.PredatorTheTop;
        }
		else if(Number == 11)
        {
			Type = CardType.Disaster;
        }
		else if(Number == 12)
        {
			Type = CardType.Ark;
        }
		else if(Number == 14)
        {
			Type = CardType.Joker;
        }
	}

    public override string ToString()
    {
        return $"{Suit.ToString()} {Number.ToString()}";
    }
}

public enum Biome
{
	Savannah = 1,
	Snowfield = 2,
	Forest = 3,
	Ocean = 4
}
public enum CardType
{
	/// <summary> �A�� </summary>
	Plant,
	/// <summary> ��H�� </summary>
	Prey, 
	/// <summary> �ߐH�� </summary>
	Predator, 
	/// <summary> ���_�ߐH�� </summary>
	PredatorTheTop, 
	/// <summary> �ЊQ </summary>
	Disaster, 
	/// <summary> ���M </summary>
	Ark,
	/// <summary> �W���[�J�[ summary>
    Joker
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