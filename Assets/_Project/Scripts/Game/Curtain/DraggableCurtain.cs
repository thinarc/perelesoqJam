using DG.Tweening;
using UnityEngine;

namespace _Project.Scripts.Curtain
{
    public class DraggableCurtain : MonoBehaviour
    {
        private CanvasGroup _group;

        private void Start()
        {
            _group = GetComponent<CanvasGroup>();
            _group.DOFade(0, 0);
        }

        public void Show(Ease ease, float time)
        {
            _group.DOFade(1, time).SetEase(ease);
        }

        public void Hide(Ease ease, float time)
        {
            _group.DOFade(0, time).SetEase(ease);
        }
    }
}
