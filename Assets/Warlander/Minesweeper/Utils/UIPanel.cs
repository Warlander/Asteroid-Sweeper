using DG.Tweening;
using UnityEngine;

namespace Warlander.Minesweeper.Utils
{
    public class UIPanel : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;

        public void Show()
        {
            gameObject.SetActive(true);
            _canvasGroup.alpha = 0;
            _canvasGroup.DOFade(1, 0.5f);
        }
    }
}