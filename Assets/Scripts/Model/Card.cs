public struct Card
{
    public readonly cardRank Rank;
    public readonly cardSuit Suit;
    public readonly int CardValue;
    public readonly string CardName;
    public readonly int CardId;

    public Card(cardRank rank, cardSuit suit, int cardId)
    {
        Suit = suit;
        Rank = rank;
        CardName = Rank + "_of_" + Suit;
        CardId = cardId;

        switch (rank)
        {
            case cardRank.Jack:
            case cardRank.Queen:
            case cardRank.King: CardValue = 10; break;
            default: CardValue = (int)rank + 1; break;
        }
    }
}