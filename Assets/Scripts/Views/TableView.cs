using System.Collections.Generic;
using UnityEngine;

namespace Views
{
    public class TableView : MonoBehaviour
    {
        private readonly List<CardView> _cards = new List<CardView>();

        private const int MaxCards = 7;
        public bool TryMoveToTable(CardView cardView)
        {
            if(_cards.Count + 1 > MaxCards)
                return false;
            Vector2 localMousePosition = transform.InverseTransformPoint(Input.mousePosition);
            if (!((RectTransform) transform).rect.Contains(localMousePosition))
            {
                return false;
            }
            
            AddCard(cardView);
            return true;
        }
        
        private Vector2 GetCardPosition(int cardNumber)
        {
            var cardTransform = ((RectTransform) _cards[cardNumber].transform);
            var cardWidth = cardTransform.rect.width - CardView.BackgroundBorder * cardTransform.localScale.x;
            var handWidth = ((RectTransform) transform).rect.width - cardWidth;
            var cardsLineSize = (_cards.Count - 1) * cardWidth;
            if (cardsLineSize > handWidth)
            {
                cardsLineSize = handWidth;
            }

            var leftCardPosition = - cardsLineSize * 0.5f;
            var cardPosition = _cards.Count == 1 ? 0 :  cardNumber / (float)  (_cards.Count - 1) * cardsLineSize + leftCardPosition;

            return new Vector2(cardPosition, 0);
        }


        private void AddCard(CardView cardView)
        {
            _cards.Add(cardView);
            cardView.transform.parent = transform;

            UpdateCardPositions();
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
                
                card.ObjectMover.Move(GetCardPosition(cardNumberId++), Quaternion.identity);
            }
        }
    }
}