using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Common.HUD;
using _Project.Scripts.Game.Editing.Photo;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Game.Editing
{
    public class EditController : MonoBehaviour
    {
        public static EditController Instance { get; private set; }
        
        public ToolType currentTool;

        [SerializeField, Space(5)] private int minTargetShine;
        [SerializeField] private int maxTargetShine;

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
        
        private EditingPhoto _currentPhoto;

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
            GetComponentsInChildren<EditingPhoto>().ToList().ForEach(photo =>
            {
                photo.GetComponent<Image>().DOFade(0, 0);
                photo.GetComponent<Image>().raycastTarget = false;
                _queue.Enqueue(photo);
            });

            while (true)
            {
                await EditCycle();
                if (_queue.Count == 0)
                {
                    print("queue end");
                    break;
                }
            }
        }

        public bool waitCrossLine;
        public async void CrossOutLine()
        {
            if (_currentPhoto == null) return;
            var l = _fillLines[_currentPhoto.numberPhoto];
            while (true)
            {
                l.fillAmount = Mathf.MoveTowards(l.fillAmount, 1f, Time.deltaTime * 4f * Random.Range(2f, 13));
                await UniTask.Delay(Random.Range(4, 7));
                if (Mathf.Approximately(l.fillAmount, 1f)) break;
            }

            waitCrossLine = false;
        }

        public async void ShowButtons()
        {
            await UniTask.Delay(800);

            buttonGroup.DOFade(1, 0.4f).SetEase(Ease.OutBack);
            await UniTask.Delay(TimeSpan.FromSeconds(0.2f));
            buttonGroup.interactable = true;
            buttonGroup.blocksRaycasts = true;
        }

        public async void CloseButtons()
        {
            buttonGroup.interactable = false;
            buttonGroup.blocksRaycasts = false;
            await buttonGroup.DOFade(0, 0.2f).SetEase(Ease.InBack).AsyncWaitForCompletion();
        }

        private async UniTask EditCycle()
        {
            prepareRotate = false;
            waitChoose = false;
            var list = linesContainer.parent;
            list.GetComponent<ToolsButton>().enabled = false;
            list.GetComponent<UIEffects>().yaInactiveList = true;
            list.GetComponent<UIEffects>().yaActiveList = false;

            waitCrossLine = false;
            rejectPhoto = false;
            
            var photo = _queue.Dequeue();
            
            _spawner.FillPhoto(photo);
            _currentPhoto = photo;
            
            SprayTool.Instance.UpdateContainer(photo.GetShineRect());
            
            photo.GetComponent<Image>().DOFade(1, 0.3f).SetEase(Ease.InBack);
            photo.GetComponent<Image>().raycastTarget = true;
            photo.StartEditing();
            
            targetS = Random.Range(minTargetShine, maxTargetShine);

            while (true)
            {
                var dust = photo.GetDustRect();
                var dirt = photo.GetDirtRect();
                var shine = photo.GetShineRect();
                
                if (dust == null || dirt == null || shine == null) break;
                
                var d = dust.childCount;
                var r = dirt.childCount;
                var s = shine.childCount;
                
                var completeClean = d == 0 && r == 0 && s == targetS;
                
                await UniTask.Yield();
                if (completeClean) break;
            }
            
            photo.PrepareToRotate();
            prepareRotate = true;

            acceptButton.onClick.RemoveAllListeners();
            rejectButton.onClick.RemoveAllListeners();
            waitChoose = true;
            
            acceptButton.onClick.AddListener(async () =>
            {
                waitChoose = false;
                rejectPhoto = false;

                photo.transform.DORotate(new Vector3(0f, 0f, 15f), 0.15f);
                
                CloseButtons();

                await photo.GetComponent<RectTransform>().DOAnchorPosX(photo.GetComponent<RectTransform>().anchoredPosition.x + 100, 1f).SetEase(Ease.InBack).AsyncWaitForCompletion();
                
                await photo.GetComponent<Image>().DOFade(0, 0.3f).SetEase(Ease.InBack).AsyncWaitForCompletion();

                photo.gameObject.SetActive(false);
            });
            
            rejectButton.onClick.AddListener(async () =>
            {
                var rectTransform = photo.GetComponent<RectTransform>();
                var image = photo.GetComponent<Image>();

                var direction = Random.value > 0.5f ? -1f : 1f;
                
                CloseButtons();

                photo.transform.DORotate(
                    new Vector3(0f, 0f, -15f * direction),
                    0.15f
                );

                rectTransform
                    .DOAnchorPosX(
                        rectTransform.anchoredPosition.x + 200f * direction,
                        1f
                    )
                    .SetEase(Ease.InBack);

                await buttonGroup
                    .DOFade(0, 0.2f)
                    .SetEase(Ease.InBack)
                    .AsyncWaitForCompletion();

                await image
                    .DOFade(0, 0.3f)
                    .SetEase(Ease.InBack)
                    .AsyncWaitForCompletion();

                photo.gameObject.SetActive(false);
                
                waitChoose = false;
                rejectPhoto = true;
            });
            
            await UniTask.WaitUntil(() => !waitChoose);

            if (photo.acceptable)
            {
                if (!rejectPhoto)
                {
                    var l = linesContainer.parent;
                    l.GetComponent<ToolsButton>().enabled = true;
                    l.GetComponent<UIEffects>().yaInactiveList = false;
                    l.GetComponent<UIEffects>().yaActiveList = true;
                
                    waitCrossLine = true;
                    listEffects.OnPointerEnter(null);
                    await UniTask.WaitUntil(() => !waitCrossLine);
                }
                else
                {
                    PhotoToLastQueue(photo);
                    print("you reject right photo");
                }
            }
            else
            {
                if (!rejectPhoto)
                {
                    print("you accept wrong photo");
                }
            }

            await UniTask.Delay(1000);
        }

        public bool rejectPhoto;
        public UIEffects listEffects;

        public void PhotoToLastQueue(EditingPhoto photo)
        {
            photo.GetComponent<Image>().DOFade(0, 0);
            photo.GetComponent<Image>().raycastTarget = false;
            photo.gameObject.SetActive(true);
            photo.transform.SetAsLastSibling();
            buttonGroup.transform.SetAsLastSibling();
            _queue.Enqueue(photo);
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