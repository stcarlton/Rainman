using System.Collections.Generic;

public class BlackJackHand
{
    static BlackJackHand instance = new BlackJackHand();

    bool _soft;
    bool _twins;
    int _handValue;
    Dictionary<int, tactic> _strategy;

    public int HandValue
    {
        get
        {
            if (cards.Count > 0)
            {
                _soft = false;
                _twins = false;

                int retValue = 0;
                cardRank tempRank = cardRank.None;

                foreach (KeyValuePair<string, Card> c in cards)
                {
                    if (c.Value.Rank == cardRank.Ace)
                    {
                        _soft = true;
                    }
                    if (tempRank != cardRank.None)
                    {
                        _twins = tempRank == c.Value.Rank;
                    }
                    tempRank = c.Value.Rank;
                    retValue += c.Value.CardValue;
                }
                if (_soft && retValue < 12)
                {
                    retValue += 10;
                }
                _twins = _twins && cards.Count == 2;
                return retValue;
            }
            else
            {
                return 0;
            }
        }
    }

    public Dictionary<string, Card> cards;

    private BlackJackHand()
    {
        _handValue = 0;
        _soft = false;
        _twins = false;
        _strategy = new Dictionary<int, tactic>();
        cards = new Dictionary<string, Card>();
        PopulateStrategy();
    }

    public static BlackJackHand getBlackJackHand()
    {
        return instance;
    }

    public tactic GetStrategy(int dealerValue)
    {
        _handValue = HandValue;
        if(cards.Count < 2 || _handValue < 2)
        {
            return tactic.None;
        }
        else if (_handValue > 21)
        {
            return tactic.Bust;
        }
        else
        {
            return _strategy[keyGenerator(dealerValue, _twins, _soft, _handValue)];
        }
    }
    public void PopulateStrategy()
    {
        _strategy.Clear();
        //i is dealer value
        //j is player value
        //initial setup; 35
        for(int i = 1; i < 11; i++)
        {
            //initial populate of normal hands 5-16; 12
            for (int j = 5; j < 17; j++)
            {
                _strategy.Add(keyGenerator(i,false,false,j), tactic.Hit);
            }
            //initial populate of normal hands 17+; 5
            for (int j = 17; j < 22; j++)
            {
                _strategy.Add(keyGenerator(i,false,false,j), tactic.Stand);
            }

            //initial populate of soft hands 13-18; 6
            for (int j = 13; j < 19; j++)
            {
                _strategy.Add(keyGenerator(i,false,true,j), tactic.Hit);
            }
            //soft hands 19-21; 3
            for(int j = 19; j < 22; j++)
            {
                _strategy.Add(keyGenerator(i,false,true,j), tactic.Stand);
            }

            //initial populate of split hands; 9
            for (int j = 4; j < 19; j += 2)
            {
                _strategy.Add(keyGenerator(i,true,false,j), tactic.Split);
            }
            //10 10; 1
            _strategy.Add(keyGenerator(i,true,false,20), tactic.Stand);
            //A A; 1
            _strategy.Add(keyGenerator(i, true, true, 2), tactic.Split);
        }

        //Exceptions
        //normal hands
        for(int i = 3; i < 7; i++)
        {
            _strategy[keyGenerator(i,false,false,9)] = tactic.Double;
        }
        for (int i = 2; i < 10; i++)
        {
            _strategy[keyGenerator(i,false,false,10)] = tactic.Double;
        }
        for(int i = 2; i < 11; i++)
        {
            _strategy[keyGenerator(i, false, false, 11)] = tactic.Double;
        }
        for(int i = 4; i < 7; i++)
        {
            _strategy[keyGenerator(i, false, false, 13)] = tactic.Stand;
        }

        //soft hands
        for(int i = 5; i < 7; i++)
        {
            for(int j = 13; j < 15; j++)
            {
                _strategy[keyGenerator(i, false, true, j)] = tactic.Double;
            }
        }
        for(int i = 4; i < 7; i++)
        {
            for(int j = 15; j < 17; j++)
            {
                _strategy[keyGenerator(i, false, true, j)] = tactic.Double;
            }
        }
        for(int i = 2; i < 9; i++)
        {
            _strategy[keyGenerator(i, false, true, 18)] = tactic.Stand;
        }
        for(int i = 3; i < 7; i++)
        {
            for(int j = 17; j < 19; j++)
            {
                _strategy[keyGenerator(i, false, true, j)] = tactic.Double;
            }
        }

        //twin hands
        for(int j = 4; j < 16; j += 2)
        {
            _strategy[keyGenerator(1, true, false, j)] = tactic.Hit;
        }
        for(int i = 8; i < 11; i++)
        {
            for(int j = 4; j < 8; j += 2)
            {
                _strategy[keyGenerator(i, true, false, j)] = tactic.Hit;
            }
        }
        for(int i = 2; i < 5; i++)
        {
            _strategy[keyGenerator(i, true, false, 8)] = tactic.Hit;
        }
        for (int i = 7; i < 11; i++)
        {
            for(int j = 8; j < 14; j += 2)
            {
                _strategy[keyGenerator(i, true, false, j)] = tactic.Hit;
            }
        }
        for(int i = 2; i < 10; i++)
        {
            _strategy[keyGenerator(i, true, false, 10)] = tactic.Double;
        }
        for(int i = 8; i < 11; i++)
        {
            _strategy[keyGenerator(i, true, false, 14)] = tactic.Hit;
        }
        _strategy[keyGenerator(7, true, false, 18)] = tactic.Stand;
        _strategy[keyGenerator(10, true, false, 18)] = tactic.Stand;
        _strategy[keyGenerator(1, true, false, 18)] = tactic.Stand;
    }
    public void IncNegTwo()
    {
        _strategy[keyGenerator(3, false, false, 13)] = tactic.Hit;
        _strategy[keyGenerator(5, false, false, 12)] = tactic.Hit;
    }
    public void IncNegOne()
    {
        _strategy[keyGenerator(2, false, false, 13)] = tactic.Hit;
        _strategy[keyGenerator(6, false, false, 12)] = tactic.Hit;
    }
    public void IncZero()
    {
        _strategy[keyGenerator(4, false, false, 12)] = tactic.Hit;
    }
    public void IncOneFive()
    {
        _strategy[keyGenerator(3, false, false, 12)] = tactic.Stand;
        _strategy[keyGenerator(2, false, false, 9)] = tactic.Double;
    }
    public void IncTwo()
    {
        _strategy[keyGenerator(1, false, false, 11)] = tactic.Double;
    }
    public void IncThree()
    {
        _strategy[keyGenerator(2, false, false, 12)] = tactic.Stand;
    }
    public void IncFive()
    {
        _strategy[keyGenerator(1, false, false, 10)] = tactic.Double;
        _strategy[keyGenerator(7, false, false, 9)] = tactic.Double;
        _strategy[keyGenerator(6, true, false, 20)] = tactic.Split;
        _strategy[keyGenerator(6, true, false, 10)] = tactic.Split;
    }
    public void IncSix()
    {
        _strategy[keyGenerator(5, true, false, 20)] = tactic.Split;
        _strategy[keyGenerator(5, true, false, 10)] = tactic.Split;
    }
    public void DecNegTwo()
    {
        _strategy[keyGenerator(3, false, false, 13)] = tactic.Stand;
        _strategy[keyGenerator(5, false, false, 12)] = tactic.Stand;
    }
    public void DecNegOne()
    {
        _strategy[keyGenerator(2, false, false, 13)] = tactic.Stand;
        _strategy[keyGenerator(6, false, false, 12)] = tactic.Stand;
    }
    public void DecZero()
    {
        _strategy[keyGenerator(4, false, false, 12)] = tactic.Stand;
    }
    public void DecOneFive()
    {
        _strategy[keyGenerator(3, false, false, 12)] = tactic.Hit;
        _strategy[keyGenerator(2, false, false, 9)] = tactic.Hit;
    }
    public void DecTwo()
    {
        _strategy[keyGenerator(1, false, false, 11)] = tactic.Hit;
    }
    public void DecThree()
    {
        _strategy[keyGenerator(2, false, false, 12)] = tactic.Hit;
    }
    public void DecFive()
    {
        _strategy[keyGenerator(1, false, false, 10)] = tactic.Hit;
        _strategy[keyGenerator(7, false, false, 9)] = tactic.Hit;
        _strategy[keyGenerator(6, true, false, 20)] = tactic.Stand;
        _strategy[keyGenerator(6, true, false, 10)] = tactic.Double;
    }
    public void DecSix()
    {
        _strategy[keyGenerator(5, true, false, 20)] = tactic.Stand;
        _strategy[keyGenerator(5, true, false, 10)] = tactic.Double;
    }

    int keyGenerator(int dealerValue, bool twins, bool soft, int handValue)
    {
        return dealerValue * 10000 + (twins ? 1000 : 0) + (soft ? 100 : 0) + handValue;
    }
}
