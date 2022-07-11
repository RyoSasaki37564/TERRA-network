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