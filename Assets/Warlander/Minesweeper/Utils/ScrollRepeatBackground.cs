using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Warlander.Minesweeper.Utils
{
    public class ScrollRepeatBackground : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Image _image;

        [SerializeField] private Vector2Int _moveDirection = new Vector2Int(1, 0);
        [SerializeField] private float _cycleSpeed = 1f;

        private void Start()
        {
            Rect spriteRect = _image.sprite.textureRect;
            float scaledWidth = spriteRect.width / _image.pixelsPerUnitMultiplier;
            float scaledHeight = spriteRect.height / _image.pixelsPerUnitMultiplier;
            _rectTransform.anchoredPosition = Vector2.zero;
            
            Vector2 finalShift = new Vector2(scaledWidth, scaledHeight) * _moveDirection;
            _rectTransform.DOAnchorPos(finalShift, _cycleSpeed).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
        }

        private void OnDestroy()
        {
            _rectTransform.DOKill();
        }
    }
}