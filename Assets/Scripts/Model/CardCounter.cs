using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardCounter
{
    static CardCounter instance = new CardCounter();

    public Dictionary<string, Card> Discarded;
    public BlackJackHand Hand;

    int _trueCount;

    public float TrueCount
    {
        get
        {
            if(_trueCount >= 52.0)
            {
                return 0;
            }
            else
            {
                return (float)Math.Round(_trueCount / ((52.0f - Discarded.Count) / 52.0f),1);
            }
        }
    }

    public CardCounter()
    {
        Discarded = new Dictionary<string, Card>();
        Hand = BlackJackHand.getBlackJackHand();
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
            case 6: IncCount(); break;
            case 7:
            case 8:
            case 9: break;
            case 10: DecCount(); break;
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
        else if(trueCount < 3)
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
        Hand.PopulateStrategy();
    }
    void IncCount()
    {
        float TempCount = TrueCount;
        _trueCount++;
        if(TempCount < -2 && TrueCount > -2)
        {
            Hand.IncNegTwo();
        }
        else if(TempCount < -1 && TrueCount > -1)
        {
            Hand.IncNegOne();
        }
        else if (TempCount < 0 && TrueCount > 0)
        {
            Hand.IncZero();
        }
        else if (TempCount < 1.5 && TrueCount > 1.5)
        {
            Hand.IncOneFive();
        }
        else if (TempCount < 2 && TrueCount > 2)
        {
            Hand.IncTwo();
        }
        else if (TempCount < 3 && TrueCount > 3)
        {
            Hand.IncThree();
        }
        else if (TempCount < 5 && TrueCount > 5)
        {
            Hand.IncFive();
        }
        else if (TempCount < 6 && TrueCount > 6)
        {
            Hand.IncSix();
        }
    }
    void DecCount()
    {
        float TempCount = TrueCount;
        _trueCount--;
        if (TempCount > -2 && TrueCount < -2)
        {
            Hand.DecNegTwo();
        }
        else if (TempCount > -1 && TrueCount < -1)
        {
            Hand.DecNegOne();
        }
        else if (TempCount > 0 && TrueCount < 0)
        {
            Hand.DecZero();
        }
        else if (TempCount > 1.5 && TrueCount < 1.5)
        {
            Hand.DecOneFive();
        }
        else if (TempCount > 2 && TrueCount < 2)
        {
            Hand.DecTwo();
        }
        else if (TempCount > 3 && TrueCount < 3)
        {
            Hand.DecThree();
        }
        else if (TempCount > 5 && TrueCount < 5)
        {
            Hand.DecFive();
        }
        else if (TempCount > 6 && TrueCount < 6)
        {
            Hand.DecSix();
        }
    }
}

//7 4 vs Q (double down)
//A 7 vs J (soft hit)
//K K vs 5 (stand)
//8 8 vs K (split)
//Q 3 vs 2 with < -1 count
//Q 3 vs 2 with > -1 count