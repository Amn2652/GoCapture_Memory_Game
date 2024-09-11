using System.Collections;
using UnityEngine;

namespace Assets.Script
{
    public class CardIdentity : MonoBehaviour
    {
        [SerializeField] private int _cardNumber;
        [SerializeField] private float _animationTime;

        [SerializeField] private AnimationCurve _rotationCurve;

        [Space(5f)] 
        [SerializeField] private Sprite[] _cardFrontSprite;

        private void Awake()
        {
            transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = _cardFrontSprite[_cardNumber - 1];
        }
        public int CardNumber => _cardNumber;
        public float AnimationTime => _animationTime;

        public bool IsOpen { get; private set; }

        public void OpenCard()
        {
            IsOpen= true;
            LeanTween.rotateY(gameObject, 0, _animationTime).setEase(_rotationCurve);
        }
        public void CloseCard()
        {
            LeanTween.rotateY(gameObject, 180, _animationTime).setEase(_rotationCurve).setOnComplete(() => { IsOpen = false; });
        }
    }
}