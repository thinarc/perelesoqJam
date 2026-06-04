using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Common.HUD;
using _Project.Scripts.Game.Editing.Photo;
using _Project.Scripts.Game.Menus;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using _Project.Scripts.Sound;

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
            ShuffleChildren(GetComponent<RectTransform>());
            
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
            
            //звук вычеркивания
            SoundManager.Instance.ActionHandle.PlayCross();
            
            var l = _fillLines[_currentPhoto.numberPhoto];
            while (true)
            {
                l.fillAmount = Mathf.MoveTowards(l.fillAmount, 1f, Time.deltaTime * 4f * Random.Range(2f, 13));
                await UniTask.Delay(Random.Range(4, 7));
                if (Mathf.Approximately(l.fillAmount, 1f)) break;
            }

            currentProgress++;
            ProgressUpdate?.Invoke();
            if (currentProgress is 2 or 3 or 5 or 6 or 7 or 8 or 9)
            {
                await UniTask.Delay(Random.Range(0, 5000));
                YaStartAnomaliyaStreet?.Invoke();
            }
            if (currentProgress is 10) GameEnd.Instance.PassGame();
            
            waitCrossLine = false;
        }

        public event Action ProgressUpdate;
        public event Action YaStartAnomaliyaStreet;

        public bool firstShow = true;
        public bool currentWrongPhoto;

        public async void ShowButtons()
        {
            if (!firstShow) await UniTask.Delay(400);
            else
            {
                await UniTask.Delay(800);
                firstShow = false;
            }

            buttonGroup.DOFade(1, 0.4f).SetEase(Ease.InBack);
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

            currentWrongPhoto = !photo.acceptable;
            
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
                
                var completeClean = d == 0 && r == 0 && s >= targetS;
                if (!photo.acceptable) completeClean = d <= 2 && s >= targetS;
                
                await UniTask.Yield();
                if (completeClean) break;
            }
            
            
            
            photo.PrepareToRotate();
            prepareRotate = true;
            waitChoose = true;

            acceptButton.onClick.RemoveAllListeners();
            rejectButton.onClick.RemoveAllListeners();
            
            
            
            acceptButton.onClick.AddListener(async () =>
            {
                SoundManager.Instance.UIHandle.PlayHover();
                
                rejectPhoto = false;
                CloseButtons();

                // if (!photo.acceptable && !rejectPhoto) ;
                // else waitChoose = false;
                
                waitChoose = false;
                
                // SOUND?
                SoundManager.Instance.ActionHandle.PlayPhoto();

                photo.GetComponent<UIEffects>().onAnotherAnim = true;
                photo.transform.DORotate(new Vector3(0f, 0f, 15f), 0.15f);

                await photo.GetComponent<RectTransform>().DOAnchorPosX(photo.GetComponent<RectTransform>().anchoredPosition.x + 45, 0.9f).SetEase(Ease.InBack).AsyncWaitForCompletion();
                await photo.GetComponent<Image>().DOFade(0, 0.3f).SetEase(Ease.InBack).AsyncWaitForCompletion();

                photo.GetComponent<UIEffects>().onAnotherAnim = false;
                photo.gameObject.SetActive(false);

                waitChoose = false;
            });
            
            rejectButton.onClick.AddListener(async () =>
            {
                SoundManager.Instance.UIHandle.PlayExit();
                
                CloseButtons();
                
                var image = photo.GetComponent<Image>();

                var direction = Random.value > 0.5f ? -1f : 1f;
                
                photo.GetComponent<UIEffects>().onAnotherAnim = true;
                
                // SOUND?
                SoundManager.Instance.ActionHandle.PlayPhoto();

                photo.transform
                    .DORotate(
                        new Vector3(0f, 0f, 10f * direction),
                        0.2f
                    )
                    .SetEase(Ease.OutQuad);

                await image
                    .DOFade(0f, 0.3f)
                    .SetEase(Ease.OutQuad)
                    .AsyncWaitForCompletion();

                photo.GetComponent<UIEffects>().onAnotherAnim = false;
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
                    await UniTask.Delay(800);
                    listEffects.OnPointerEnter(null);
                    await UniTask.Delay(180);
                    listEffects.OnPointerExit(null);
                    await UniTask.WaitUntil(() => !waitCrossLine);
                }
                else PhotoToLastQueue(photo);
            }
            else
            {
                if (!rejectPhoto)
                {
                    AnomalyEditAAAAA.Instance.CatchControl();
                    AnomalyEditAAAAA.Instance.waitReturn = true;
                    await UniTask.WaitWhile(() => AnomalyEditAAAAA.Instance.waitReturn);
                }
            }

            await UniTask.Delay(500);
        }

        public bool rejectPhoto;
        public UIEffects listEffects;

        public int currentProgress;
        
        private void ShuffleChildren(RectTransform parent)
        {
            var count = parent.childCount;

            for (var i = count - 1; i > 0; i--)
            {
                var randomIndex = Random.Range(0, i + 1);
                parent.GetChild(randomIndex).SetSiblingIndex(i);
            }

            // Неудачные фото смещаем в последнюю треть списка
            var startBadIndex = Mathf.Max(count * 2 / 3, 1);

            for (var i = 0; i < count; i++)
            {
                var photo = parent.GetChild(i).GetComponent<EditingPhoto>();

                if (photo != null && !photo.acceptable)
                {
                    var targetIndex = Random.Range(startBadIndex, count);
                    photo.transform.SetSiblingIndex(targetIndex);
                }
            }

            // Первая фотография должна быть acceptable
            if (count > 0)
            {
                var firstPhoto = parent.GetChild(0).GetComponent<EditingPhoto>();

                if (firstPhoto != null && !firstPhoto.acceptable)
                {
                    for (var i = 1; i < count; i++)
                    {
                        var photo = parent.GetChild(i).GetComponent<EditingPhoto>();

                        if (photo != null && photo.acceptable)
                        {
                            photo.transform.SetSiblingIndex(0);
                            break;
                        }
                    }
                }
            }

            buttonGroup.transform.SetAsLastSibling();
        }

        private void PhotoToLastQueue(EditingPhoto photo)
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