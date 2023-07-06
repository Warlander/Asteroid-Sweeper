using DG.Tweening;
using TMPro;
using UnityEngine;
using Warlander.Minesweeper.Utils;

namespace Warlander.Minesweeper.MainMenu
{
    public class MainMenuAppearAnimation : MonoBehaviour
    {
        [SerializeField] private bool _skipAnimation;
        
        [SerializeField] private RectTransform _titleTransform;
        [SerializeField] private RectTransform _subtitleTransform;
        [SerializeField] private RectTransform _titleRoot;
        [SerializeField] private RectTransform _asteroidsRoot;
        [SerializeField] private RectTransform _leftPanel;
        [SerializeField] private RectTransform _rightPanel;

        private void Start()
        {
            if (_skipAnimation)
            {
                return;
            }
            
            SetInitialValues();
            PerformAnimation();
        }

        private void SetInitialValues()
        {
            _titleTransform.localScale = Vector3.zero;
            _subtitleTransform.localScale = Vector3.zero;
            
            _asteroidsRoot.anchorMin = new Vector2(-1, 0);
            _asteroidsRoot.anchorMax = new Vector2(0, 1);

            _leftPanel.anchoredPosition = _leftPanel.anchoredPosition.AddX(-_leftPanel.sizeDelta.x);
            _rightPanel.anchoredPosition = _rightPanel.anchoredPosition.AddX(_rightPanel.sizeDelta.x);
        }

        private void PerformAnimation()
        {
            _titleTransform.DOScale(1, 0.3f).SetDelay(0.1f);
            _subtitleTransform.DOScale(1, 0.6f).SetDelay(0.8f);

            _titleRoot.DOShakePosition(0.6f, 50f).SetDelay(0.8f).SetEase(Ease.InOutCubic);
            
            _asteroidsRoot.DOAnchorMin(new Vector2(1, 0), 1f).SetDelay(0.6f);
            _asteroidsRoot.DOAnchorMax(new Vector2(2, 1), 1f).SetDelay(0.6f)
                .OnComplete(() => _asteroidsRoot.gameObject.SetActive(false));

            _leftPanel.DOAnchorPos(Vector2.zero, 0.5f).SetDelay(1f).SetEase(Ease.OutBounce);
            _rightPanel.DOAnchorPos(Vector2.zero, 0.5f).SetDelay(1f).SetEase(Ease.OutBounce);
        }
    }
}