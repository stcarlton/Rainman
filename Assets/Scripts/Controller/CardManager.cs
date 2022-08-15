using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;
using System;

public class CardManager : MonoBehaviour
{
    ARTrackedImageManager _trackedImageManager;
    Dictionary<string, Card> _deck;
    Dictionary<string, Card> _onTable;
    CardCounter _cardCounter;
    Card _dealerUp;
    Card _handAnchor;
    Camera _mainCamera;

    [SerializeField] public Image[] Icons;
    [SerializeField] public GameObject OnTableDisplay;
    [SerializeField] public GameObject HandDisplay;
    [SerializeField] public GameObject DealerDisplay;
    [SerializeField] public GameObject OnTableInfo;
    [SerializeField] public TextMeshProUGUI MinDisplay;
    [SerializeField] public TextMeshProUGUI MaxDisplay;
    [SerializeField] public TextMeshProUGUI HandValueDisplay;
    [SerializeField] public TextMeshProUGUI StrategyDisplay;
    [SerializeField] public TextMeshProUGUI TrueCountDisplay;
    [SerializeField] public TextMeshProUGUI BettingStrategyDisplay;
    [SerializeField] public Image ImagePrefab;
    [SerializeField] public GameObject TextPrefab;

    void Awake()
    {
        _trackedImageManager = FindObjectOfType<ARTrackedImageManager>();
        _deck = new Dictionary<string, Card>();
        _onTable = new Dictionary<string, Card>();
        _cardCounter = CardCounter.getCardCounter();
        _mainCamera = Camera.main;
    }
    void Start()
    {
        BuildDeck();
    }

    void OnEnable()
    {
        _trackedImageManager.trackedImagesChanged += ImageChanged;
    }
    void OnDisable()
    {
        _trackedImageManager.trackedImagesChanged -= ImageChanged;
    }
    void ImageChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach(ARTrackedImage i in eventArgs.added)
        {
            ManageTable(i);
            
        }
        foreach (ARTrackedImage i in eventArgs.updated)
        {
            ManageTable(i);
        }
        foreach (ARTrackedImage i in eventArgs.removed)
        {
            Remove(i);
        }
    }
    void ManageTable(ARTrackedImage i)
    {
        string key = i.referenceImage.name;
        Card card = _deck[key];

        //discard if not discarded
        if (!_cardCounter.Discarded.ContainsKey(key))
        {
            _cardCounter.ProcessCard(card);
            _cardCounter.Discarded.Add(key, card);
            Icons[card.CardId].color = new Color(1f, 1f, 1f, 0.5f);
        }
        //remove from table if not on screen and on table already
        if (i.trackingState != TrackingState.Tracking && _onTable.ContainsKey(key))
        {
            _onTable.Remove(key);
        }
        //add to table if on screen and not on table yet
        else if (i.trackingState == TrackingState.Tracking && !_onTable.ContainsKey(key))
        {
            _onTable.Add(key, card);
        }

        //manage space
        card.depth = Vector3.Distance(_mainCamera.transform.position, i.transform.position);
        card.pos = i.transform.position;

        SetMinDepth();
        SetMaxDepth();
        ResetOnTable();
        ResetHand();
        SetBasicStrategy();
        UpdateDisplay();
    }
    void ResetOnTable()
    {
        foreach (Transform child in OnTableDisplay.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in OnTableInfo.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (KeyValuePair<string, Card> c in _onTable)
        {
            AddImage(OnTableDisplay, c.Value.CardId);
            AddText(OnTableInfo, Math.Round(c.Value.depth, 2).ToString());
        }
    }
    void ResetHand()
    {
        _cardCounter.Hand.cards.Clear();
        foreach (KeyValuePair<string, Card> c in _onTable)
        {
            if (Vector3.Distance(_handAnchor.pos, c.Value.pos) < 0.15 && !c.Value.Equals(_dealerUp))
            {
                _cardCounter.Hand.cards.Add(c.Value.CardName, c.Value);
            }
        }
        foreach (Transform child in HandDisplay.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (KeyValuePair<string, Card> c in _cardCounter.Hand.cards)
        {
            AddImage(HandDisplay, c.Value.CardId);
        }
        foreach (Transform child in DealerDisplay.transform)
        {
            Destroy(child.gameObject);
        }
        if (!_dealerUp.Equals(null))
        {
            AddImage(DealerDisplay, _dealerUp.CardId);
        }
    }
    void AddImage(GameObject go, int i)
    {
        Image imageInstance = Instantiate(ImagePrefab);
        imageInstance.transform.SetParent(go.transform, true);
        imageInstance.sprite = Icons[i].sprite;
    }
    void AddText(GameObject go, string s)
    {
        GameObject textInstance = Instantiate(TextPrefab);
        textInstance.transform.SetParent(go.transform, true);
        TextMeshProUGUI cardText = textInstance.GetComponent<TextMeshProUGUI>();
        cardText.text = s;

    }
    void SetMaxDepth()
    {
        float tempMax = float.MinValue;
        _dealerUp = null;
        if (_onTable.Count > 0)
        {
            foreach (KeyValuePair<string, Card> c in _onTable)
            {
                if (c.Value.depth > tempMax)
                {
                    tempMax = c.Value.depth;
                    _dealerUp = c.Value;
                }
            }
        }
        MaxDisplay.text = Math.Round(tempMax, 2).ToString();
    }
    void SetMinDepth()
    {
        float tempMin = float.MaxValue;
        _handAnchor = null;
        if (_onTable.Count > 0)
        {
            foreach (KeyValuePair<string, Card> c in _onTable)
            {
                if (c.Value.depth < tempMin)
                {
                    tempMin = c.Value.depth;
                    _handAnchor = c.Value;
                }
            }
        }
        MinDisplay.text = Math.Round(tempMin, 2).ToString();
    }
    void SetBasicStrategy()
    {
        if (_dealerUp != null)
        {
            tactic t = _cardCounter.Hand.GetStrategy(_dealerUp.CardValue);
            StrategyDisplay.text = t.ToString();
            switch (t)
            {
                case tactic.None: StrategyDisplay.text = ""; break;
                case tactic.Bust:
                    StrategyDisplay.color = Color.red;
                    break;
                case tactic.Stand:
                    StrategyDisplay.color = Color.green;
                    break;
                case tactic.Hit:
                    StrategyDisplay.color = Color.yellow;
                    break;
                case tactic.Double:
                    StrategyDisplay.color = Color.blue;
                    break;
                case tactic.Split:
                    StrategyDisplay.color = Color.magenta;
                    break;
                default:
                    break;
            }
        }
    }
    void UpdateDisplay()
    {
        HandValueDisplay.text = _cardCounter.Hand.HandValue.ToString();
        TrueCountDisplay.text = _cardCounter.TrueCount.ToString();

        bettingStrategy betStrategy = _cardCounter.GetStrategy();
        BettingStrategyDisplay.text = betStrategy.ToString();
        switch (betStrategy)
        {
            case bettingStrategy.Minimum:
                BettingStrategyDisplay.color = Color.red;
                TrueCountDisplay.color = Color.red;
                break;
            case bettingStrategy.Big:
                BettingStrategyDisplay.color = Color.green;
                TrueCountDisplay.color = Color.green;
                break;
            case bettingStrategy.Maximum:
                BettingStrategyDisplay.color = Color.blue;
                TrueCountDisplay.color = Color.blue;
                break;
            default: break;
        }
    }
    void BuildDeck()
    {
        for (int i = 0; i < 13; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                Card newCard = new Card((cardRank)i, (cardSuit)j, i * 4 + j);
                _deck.Add(newCard.CardName, newCard);
            }
        }
    }
    void Remove(ARTrackedImage i)
    {
        string key = i.referenceImage.name;

        //if card is on table, remove it
        if (_onTable.ContainsKey(key))
        {
            _onTable.Remove(key);
        }
    }
    public void ResetCounter()
    {
        _cardCounter.Reset();
        foreach(Image i in Icons){
            i.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }
        TrueCountDisplay.text = _cardCounter.TrueCount.ToString();
        UpdateDisplay();
    }
}
