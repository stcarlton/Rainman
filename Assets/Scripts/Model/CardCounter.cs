using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardCounter
{
    static CardCounter instance = new CardCounter();

    public Dictionary<string, Card> Discarded;
    int _trueCount;

    public float TrueCount
    {
        get
        {
            return _trueCount / (Discarded.Count / 52.0f);
        }
    }

    public CardCounter()
    {
        Discarded = new Dictionary<string, Card>();
        _trueCount = 0;
    }

    public static CardCounter getCardCounter()
    {
        return instance;
    }

    public void ProcessCard(Card c)
    {
        switch (c.CardValue)
        {
            case 2:
            case 3:
            case 4:
            case 5:
            case 6: _trueCount++; break;
            case 7:
            case 8:
            case 9: break;
            case 10: _trueCount--; break;
            default: break;
        }
    }
    public bettingStrategy GetStrategy()
    {
        float trueCount = TrueCount;

        if(trueCount < 1)
        {
            return bettingStrategy.Minimum;
        }
        else if(trueCount < 2)
        {
            return bettingStrategy.Big;
        }
        else
        {
            return bettingStrategy.Maximum;
        }
    }
    public void Reset()
    {
        Discarded.Clear();
        _trueCount = 0;
    }
}
