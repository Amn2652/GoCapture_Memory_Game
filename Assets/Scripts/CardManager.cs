using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace Assets.Script
{
    public class CardManager : MonoBehaviour
    {
        private int _firstOpenedCardNumber = 0, _secondOpenedCardNumber = -1;
        private bool _isFirstCardOpened, _isSecondCardOpened;

        private int _maxCardCanBeOpened = 2;
        private GameObject _firstCard, _secondCard;

        [SerializeField] private int _cardPair = 5;
        private int _cardMatched = 0;

        private void Start()
        {
            GameManager.instance.OnGameStart += Enable;
            GameManager.instance.OnGameEnd += Disable;
        }

        private void Enable()
        {
            EnhancedTouchSupport.Enable();
            Touch.onFingerUp += OpenCard;
            ShuffleCards();  // Shuffle cards when the game starts
        }

        private void Disable()
        {
            Touch.onFingerUp -= OpenCard;
            EnhancedTouchSupport.Disable();
        }

        private void OnDisable()
        {
            Touch.onFingerUp -= OpenCard;
            EnhancedTouchSupport.Disable();
        }

        private void Update()
        {
            if (Touch.activeTouches.Count == 0 && Input.GetMouseButtonDown(0))
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Collider2D collider = Physics2D.OverlapPoint(mousePosition);
                if (collider != null && collider.CompareTag("card"))
                {
                    CardIdentity identity = collider.GetComponent<CardIdentity>();
                    if (identity.IsOpen == false && _maxCardCanBeOpened > 0)
                    {
                        identity.OpenCard();
                        GetAndCheckOpenedCard(identity.gameObject);
                    }
                }
            }
        }

        private void OpenCard(Finger finger)
        {
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(finger.currentTouch.screenPosition);
            Collider2D collider = Physics2D.OverlapPoint(touchPosition);
            if (collider != null && collider.CompareTag("card"))
            {
                CardIdentity identity = collider.GetComponent<CardIdentity>();
                if (identity.IsOpen == false && _maxCardCanBeOpened > 0)
                {
                    identity.OpenCard();
                    GetAndCheckOpenedCard(identity.gameObject);
                }
            }
        }

        private void GetAndCheckOpenedCard(GameObject obj)
        {
            if (_isFirstCardOpened)
            {
                _secondOpenedCardNumber = obj.GetComponent<CardIdentity>().CardNumber;
                _secondCard = obj;
                _isSecondCardOpened = true;
                _maxCardCanBeOpened = 0;
                StartCoroutine(StartCheckingCard());
            }
            else
            {
                _firstOpenedCardNumber = obj.GetComponent<CardIdentity>().CardNumber;
                _firstCard = obj;
                _isFirstCardOpened = true;
                _maxCardCanBeOpened = 1;
            }
        }

        private IEnumerator StartCheckingCard()
        {
            yield return new WaitForSeconds(0.5f);
            if (_isFirstCardOpened && _isSecondCardOpened)
            {
                if (_firstOpenedCardNumber != _secondOpenedCardNumber)
                {
                    _firstCard.GetComponent<CardIdentity>().CloseCard();
                    _secondCard.GetComponent<CardIdentity>().CloseCard();
                    ResetCardStats();
                }
                else
                {
                    Destroy(_firstCard.GetComponent<Collider>());
                    Destroy(_secondCard.GetComponent<Collider>());
                    _cardMatched++;
                    LeanTween.scale(_firstCard, Vector2.zero, 0.3f).setDestroyOnComplete(true);
                    LeanTween.scale(_secondCard, Vector2.zero, 0.3f).setDestroyOnComplete(true).setOnComplete(() =>
                    {
                        ResetCardStats();
                        CheckIsAllCardMatch();
                    });
                }
            }
        }

        private void ResetCardStats()
        {
            _firstCard = null;
            _secondCard = null;
            _firstOpenedCardNumber = 0;
            _secondOpenedCardNumber = -1;
            _isFirstCardOpened = false;
            _isSecondCardOpened = false;
            _maxCardCanBeOpened = 2;
        }

        private void CheckIsAllCardMatch()
        {
            if (_cardMatched == _cardPair || GameObject.FindGameObjectsWithTag("card").Length == 0)
            {
                GameManager.instance.IsWon = true;
                GameManager.instance.OnGameEnd?.Invoke();
            }
        }

        public void GameWon()
        {
            _cardMatched = _cardPair;
            CheckIsAllCardMatch();
        }

        private void ShuffleCards()
        {
            // Call Shuffle function from ShuffleCards script
            FindObjectOfType<ShuffleCards>().ShuffleCardsList();
        }
    }
}
