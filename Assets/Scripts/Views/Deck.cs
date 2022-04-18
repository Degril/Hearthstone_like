using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using Random = UnityEngine.Random;

public class Deck : MonoBehaviour
{
    private List<CardData> cardTypes = new List<CardData>();
    [SerializeField] private CardView cardViewPrefab;

    public async void Awake()
    {
        for (int i = 0; i < 6; i++)
        {
            var spriteCard = await ImageDownloader.GetRandomImage();
            cardTypes.Add(new CardData()
            {
                avatar = spriteCard, 
                attack = Random.Range(0, 10), 
                hp = Random.Range(0, 10),
                mana = Random.Range(0, 10)
            });
        }
    }

    public CardView GetNextCard()
    {
        var card = Instantiate(cardViewPrefab);
        card.transform.position = transform.position;
        card.Init(cardTypes[Random.Range(0, cardTypes.Count)]);
        return card;
    }
}