using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;

public class CardManager : MonoBehaviour
{
    ARTrackedImageManager _trackedImageManager;
    Dictionary<string, Card> _deck;
    Dictionary<string, Card> _discarded;
    

    [SerializeField] public Image[] Icons;

    void Awake()
    {
        _trackedImageManager = FindObjectOfType<ARTrackedImageManager>();
        _deck = new Dictionary<string, Card>();
        _discarded = new Dictionary<string, Card>();
    }

    void Start()
    {
        for (int i = 0; i < 13; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                Card newCard = new Card((cardRank)i, (cardSuit)j, i * 4 + j );
                _deck.Add(newCard.CardName, newCard);
            }
        }
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
        foreach(ARTrackedImage trackedImage in eventArgs.added)
        {
            string key = trackedImage.referenceImage.name;
            Card card = _deck[key];
            if (!_discarded.ContainsKey(key))
            {
                _discarded.Add(key, card);
                //Icons[card.CardId].enabled = false;
                Icons[card.CardId].color = new Color(1f, 1f, 1f, 0.5f);
            }

            /*
            trackedImage.referenceImage.name;
            trackedImage.referenceImage.guid;
            trackedImage.transform;
            trackedImage.trackingState;
            trackedImage.trackableId;
            */
        }
        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            
        }
        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {

        }
    }
    void UpdateImage(ARTrackedImage trackedImage)
    {
        string name = trackedImage.referenceImage.name;
        Vector3 position = trackedImage.transform.position;

        //GameObject prefab = 
    }
}
