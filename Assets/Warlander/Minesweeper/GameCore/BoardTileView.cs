using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Warlander.Minesweeper.GameCore
{
    /// <summary>
    /// Board tile view is expected to always be 1 Unity unit sized.
    /// </summary>
    public class BoardTileView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private SpriteRenderer _tileRenderer;
        [SerializeField] private SpriteRenderer _mineRenderer;

        [SerializeField] private Color _revealedTileColor = new Color(1f, 1f, 1f);
        [SerializeField] private Color _hiddenTileColor = new Color(0.5f, 0.5f, 0.5f);
        [SerializeField] private Color _mineTileColor = new Color(1f, 0.5f, 0.5f);

        [SerializeField] private Color[] _numberColors;
        [SerializeField] private Color _flagColor = Color.red;

        [SerializeField] private float _transitionAnimationLength = 0.3f;

        public void SetHidden(bool instant = false)
        {
            FinishAnimationInstantly();
            
            _text.transform.DOScale(0, _transitionAnimationLength).SetEase(Ease.InBack).OnComplete(() =>
            {
                _text.gameObject.SetActive(false);
            });
            
            _mineRenderer.gameObject.SetActive(false);

            _tileRenderer.DOColor(_hiddenTileColor, _transitionAnimationLength);
            
            if (instant)
            {
                FinishAnimationInstantly();
            }
        }

        public void SetNumber(int number, bool instant = false)
        {
            FinishAnimationInstantly();

            Color numberColor = _numberColors[Math.Clamp(number - 1, 0, _numberColors.Length - 1)];
            _text.color = numberColor;
            _text.text = number.ToString();
            _text.gameObject.SetActive(number != 0);
            _text.transform.DOScale(1, _transitionAnimationLength).SetEase(Ease.OutBack);
            
            _mineRenderer.gameObject.SetActive(false);

            _tileRenderer.DOColor(_revealedTileColor, _transitionAnimationLength);
            
            if (instant)
            {
                FinishAnimationInstantly();
            }
        }

        public void SetMine(bool instant = false)
        {
            FinishAnimationInstantly();
            
            _text.gameObject.SetActive(false);
            
            _mineRenderer.gameObject.SetActive(true);
            
            _tileRenderer.DOColor(_mineTileColor, _transitionAnimationLength);

            if (instant)
            {
                FinishAnimationInstantly();
            }
        }

        public void SetFlagged(bool instant = false)
        {
            FinishAnimationInstantly();
            
            _mineRenderer.gameObject.SetActive(false);

            _text.transform.localScale = Vector3.zero;
            _text.transform.DOScale(1, _transitionAnimationLength).SetEase(Ease.OutBack);
            _text.color = _flagColor;
            _text.text = "X";
            _text.gameObject.SetActive(true);

            _tileRenderer.color = _hiddenTileColor;
            
            if (instant)
            {
                FinishAnimationInstantly();
            }
        }
        
        private void FinishAnimationInstantly()
        {
            _text.DOComplete(true);
            _text.transform.DOComplete(true);
            _tileRenderer.DOComplete(true);
            _mineRenderer.DOComplete(true);
        }

        private void KillAnimationInstantly()
        {
            _text.DOKill();
            _text.transform.DOKill();
            _tileRenderer.DOKill();
            _mineRenderer.DOKill();
        }

        private void OnDestroy()
        {
            KillAnimationInstantly();
        }
    }
}