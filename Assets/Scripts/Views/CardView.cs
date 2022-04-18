using System;
using Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Utils.CoroutineAnimation;

namespace Views
{
    [RequireComponent(typeof(ObjectMover))]
    [RequireComponent(typeof(ObjectScaler))]
    public class CardView : MonoBehaviour
    {
        [SerializeField] private Image avatarImage;
        [SerializeField] private TextMeshProUGUI manaText;
        [SerializeField] private TextMeshProUGUI attackText;
        [SerializeField] private TextMeshProUGUI hpText;
        [SerializeField] private InteractionListener interactionListener;
        public ObjectMover ObjectMover { get; private set; }
        public ObjectScaler ObjectScaler { get; private set; }

        public event Action<CardView> OnZoomed;
        public event Action<CardView> OnUnZoomed;
        public event Action<CardView> OnHold;
        public event Action<CardView> OnUnHold;
        public const int BackgroundBorder = 20;

        private void Awake()
        {
            ObjectMover = GetComponent<ObjectMover>();
            ObjectScaler = GetComponent<ObjectScaler>();
        }

        public void Init(CardData cardData)
        {
            avatarImage.sprite = cardData.avatar;
            manaText.text = cardData.mana.ToString();
            attackText.text = cardData.attack.ToString();
            hpText.text = cardData.hp.ToString();
        }

        private void OnEnable()
        {
            interactionListener.onPointerEnter += Zoom;
            interactionListener.onPointerExit += UnZoom;
            interactionListener.onPointerDown += Hold;
            interactionListener.onPointerUp += UnHold;
        }

        private void OnDisable()
        {
            interactionListener.onPointerEnter -= Zoom;
            interactionListener.onPointerExit -= UnZoom;
            interactionListener.onPointerDown -= Hold;
            interactionListener.onPointerUp -= UnHold;
        }

        private void Hold() => OnHold?.Invoke(this);
        private void UnHold() => OnUnHold?.Invoke(this);

        private void Zoom() =>  OnZoomed?.Invoke(this);

        private void UnZoom() => OnUnZoomed?.Invoke(this);
    }
}
