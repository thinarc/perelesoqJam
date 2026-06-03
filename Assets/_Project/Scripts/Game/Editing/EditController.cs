using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Game.Editing.Photo;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Game.Editing
{
    public class EditController : MonoBehaviour
    {
        public static EditController Instance { get; private set; }
        
        public ToolType currentTool;

        [SerializeField, Space(5)] private int minTargetShine;
        [SerializeField] private int maxTargetShine;

        [SerializeField, Space(5)] private Transform dust;
        [SerializeField] private Transform dirt;
        [SerializeField] private Transform shine;

        [SerializeField, Space(5)] private RectTransform linesContainer;
        private Image[] _fillLines;

        [SerializeField, Space(5)] private CanvasGroup buttonGroup;
        [SerializeField] private Button acceptButton;
        [SerializeField] private Button rejectButton;
        
        private ParticleSpawner _spawner;
        
        private Queue<EditingPhoto> _queue;

        public int targetS;
        public bool prepareRotate;

        public bool waitChoose;

        private void Awake() => Instance = this;

        private async void Start()
        {
            _spawner = GetComponent<ParticleSpawner>();
            
            _fillLines = linesContainer.GetComponentsInChildren<Image>();
            _fillLines.ForEach(l => l.fillAmount = 0f);
            
            buttonGroup.alpha = 0f;
            buttonGroup.blocksRaycasts = false;
            buttonGroup.interactable = false;
            
            _queue = new Queue<EditingPhoto>();
            GetComponentsInChildren<EditingPhoto>().ToList().ForEach(photo => _queue.Enqueue(photo));

            while (true)
            {
                await EditCycle();
                if (_queue.Count == 0) break;
            }
        }

        public async void CrossOutLine(int i)
        {
            var l = _fillLines[i];
            while (true)
            {
                l.fillAmount = Mathf.MoveTowards(l.fillAmount, 1f, Time.deltaTime * 2f);
                await UniTask.Yield();
                if (Mathf.Approximately(l.fillAmount, 1f)) break;
            }
        }

        public async void ShowButtons()
        {
            await UniTask.Delay(800);
            
            buttonGroup.DOFade(1, 0.4f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                buttonGroup.interactable = true;
                buttonGroup.blocksRaycasts = true;
            });
        }

        private async UniTask EditCycle()
        {
            prepareRotate = false;
            
            var photo = _queue.Dequeue();
            _spawner.FillPhoto(photo);
            
            targetS = Random.Range(minTargetShine, maxTargetShine);

            while (true)
            {
                var d = dust.childCount;
                var r = dirt.childCount;
                var s = shine.childCount;
                
                var completeClean = d == 0 && r == 0 && s == targetS;
                
                await UniTask.Yield();
                if (completeClean) break;
            }
            
            photo.PrepareToRotate();
            prepareRotate = true;

            waitChoose = true;
            
            acceptButton.onClick.AddListener(() =>
            {
                waitChoose = false;
            });
            
            rejectButton.onClick.AddListener(() =>
            {
                waitChoose = false;
            });
            
            await UniTask.WaitUntil(() => waitChoose);
        }
        
        
        public enum ToolType
        {
            Brush,
            Nag,
            Spray,
            None
        }
    }
}