using UnityEngine;

public class Card
{
    public readonly cardRank Rank;
    public readonly cardSuit Suit;
    public readonly int CardValue;
    public readonly string CardName;
    public readonly int CardId;
    public float depth;
    public Vector3 pos;

    public Card(cardRank rank, cardSuit suit, int cardId)
    {
        Suit = suit;
        Rank = rank;
        CardName = Rank + "_of_" + Suit;
        CardId = cardId;
        depth = 0f;
        pos = new Vector3(0, 0);

        switch (rank)
        {
            case cardRank.Jack:
            case cardRank.Queen:
            case cardRank.King: CardValue = 10; break;
            default: CardValue = (int)rank + 1; break;
        }
    }
}