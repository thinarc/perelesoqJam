using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace _Project.Scripts.Game.Dialogue
{
    public class DialogueController : MonoBehaviour
    {
        private CanvasGroup _group;

        private void Start()
        {
            _group = GetComponentInChildren<CanvasGroup>();
        }
        
        public async void OnEndType()
        {
            await UniTask.Delay(2000);
            _group.DOFade(0, 0.4f).SetEase(Ease.InOutSine);
        }
    }
}