public struct Card
{
	public Biome Suit;
	public int Number; 
	public CardType Type;

	/// <summary>
	/// �J�[�h�̃o�C�I�[���Ɛ���������������R���X�g���N�^
	/// </summary>
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

        }
	}

    public override string ToString()
    {
        return $"{Suit.ToString()} {Number.ToString()}";
    }
}

public enum Biome
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