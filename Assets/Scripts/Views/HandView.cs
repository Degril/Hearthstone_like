using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Serialization;

public class HandView : MonoBehaviour
{
    [SerializeField] private Deck deck;
    [SerializeField] private TableView tableView;
    [SerializeField] private Transform handContent;
    [SerializeField] private float zoomScaleToSelectCard;
    
    private readonly List<CardView> _cards = new List<CardView>();
    private int _lastSiblingIndexZoomedCard = -1;

    private const int MaxCards = 10;
    private const int MaxRotation = 20;
    private const int MaxCardsToFullSIze = 4;

    [ContextMenu("ClearCards")]
    public void CLearCards()
    {
        while (handContent.childCount > 0)
            DestroyImmediate(handContent.GetChild(0).gameObject);
        
        _cards.Clear();
    }
    
    public void AddCardFromDeck()
    {
        if(_cards.Count + 1 > MaxCards)
            return;
        
        var newCard = deck.GetNextCard();
        newCard.transform.parent = handContent;
        _cards.Add(newCard);
        ListenCard(newCard);
        UpdateCardPositions();
    }

    private void ListenCard(CardView cardView)
    {
        cardView.OnZoomed += OnZoomedCard;
        cardView.OnUnZoomed += OnUnZoomedCard;
        cardView.OnHold += OnHoldCard;
        cardView.OnUnHold += OnUnHoldCard;
    }
    
    private void UnListenCard(CardView cardView)
    {
        cardView.OnZoomed -= OnZoomedCard;
        cardView.OnUnZoomed -= OnUnZoomedCard;
        cardView.OnHold -= OnHoldCard;
        cardView.OnUnHold -= OnUnHoldCard;
    }

    private void OnHoldCard(CardView cardView)
    {
        cardView.transform.rotation = Quaternion.identity;
        cardView.transform.localScale = Vector3.one;
        
        cardView.ObjectMover.MoveToMouse = true;
    }

    private void OnUnHoldCard(CardView cardView)
    {
        if (tableView.TryMoveToTable(cardView))
        {
            UnListenCard(cardView);
            _cards.Remove(cardView);
        }
        UpdateCardPositions();
        cardView.ObjectMover.MoveToMouse = false;
    }

    private void OnZoomedCard(CardView cardView)
    {
        if(_cards.Count <= MaxCardsToFullSIze)
            return;
        
        cardView.ObjectScaler.ChangeScale(Vector3.one * zoomScaleToSelectCard);
        _lastSiblingIndexZoomedCard = transform.GetSiblingIndex();
        transform.SetAsLastSibling();
            
        var currentId = _cards.IndexOf(cardView);
        if (currentId != _cards.Count - 1)
            _cards.Insert(currentId + 1, null);
        if (currentId != 0)
            _cards.Insert(currentId, null);
        UpdateCardPositions();
    }

    private void OnUnZoomedCard(CardView cardView)
    {
        if(_lastSiblingIndexZoomedCard <0 || _lastSiblingIndexZoomedCard >= _cards.Count)
            return;
        _cards.RemoveAll(item => item == null);
        UpdateCardPositions();
        cardView.ObjectScaler.ChangeScale(Vector3.one);
        
        transform.SetSiblingIndex(_lastSiblingIndexZoomedCard);
        _lastSiblingIndexZoomedCard = -1;
    }

    private void GetCardPositionAndDirection(int cardNumber, out Vector2 position, out float directionZ)
    {
        var cardWidth = ((RectTransform) _cards[cardNumber].transform).rect.width - CardView.BackgroundBorder;
        var handWidth = ((RectTransform) transform).rect.width - cardWidth;
        var cardsLineSize = (_cards.Count - 1) * cardWidth;
        directionZ = 0;
        if (_cards.Count > MaxCardsToFullSIze)
        {
            directionZ = Mathf.Lerp(MaxRotation, -MaxRotation, _cards.Count == 1 ? 0.5f : cardNumber / (float) (_cards.Count - 1));
            cardsLineSize = handWidth;
        }

        var leftCardPosition = - cardsLineSize * 0.5f;
        var cardPosition = _cards.Count == 1 ? 0 :  cardNumber / (float)  (_cards.Count - 1) * cardsLineSize + leftCardPosition;

        position = new Vector2(cardPosition, 0);
    }

    private void UpdateCardPositions()
    {
        var cardNumberId = 0;
        foreach (var card in _cards)
        {
            if (card == null)
            {
                cardNumberId++;
                continue;
            }
            
            GetCardPositionAndDirection(cardNumberId++, out var position, out var direction);
            var yBias = Vector3.down * Mathf.Abs(direction) * 2;
            var nextPosition= (Vector3)position + yBias;
            var nextEulerAngles = Vector3.forward * direction;
            card.ObjectMover.Move(nextPosition, Quaternion.Euler(nextEulerAngles));
        }
    }
}