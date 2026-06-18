using _Project.Scripts.Game.Locations;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Project.Scripts.Game.Menus
{
    public class GameEnd : MonoBehaviour
    {
        public static GameEnd Instance { get; private set; }

        [SerializeField] private CanvasGroup reloadGroup;
        [SerializeField] private Image[] goodImages;
        // [SerializeField] private Image[] badImages;
        [SerializeField] private CanvasGroup badGroup;
        [SerializeField] private Image[] averageImages;
        
        private CanvasGroup _group;

        private void Awake() => Instance = this;

        private void Start()
        {
            _group = GetComponentInChildren<CanvasGroup>();
            
            _group.alpha = 0;
            _group.blocksRaycasts = false;
            _group.interactable = false;
            
            reloadGroup.alpha = 0;
            reloadGroup.blocksRaycasts = false;
            reloadGroup.interactable = false;

            goodImages.ForEach(i => i.DOFade(0, 0));
            // badImages.ForEach(i => i.DOFade(0, 0));
            badGroup.DOFade(0, 0);
            badGroup.blocksRaycasts = false;
            badGroup.interactable = false;
            averageImages.ForEach(i => i.DOFade(0, 0));
        }
        
        [Button]
        public async void PassGame()
        {
            reloadGroup.alpha = 0;
            reloadGroup.blocksRaycasts = false;
            reloadGroup.interactable = false;
            
            print("Game Pass");
            _group.alpha = 1;
            _group.blocksRaycasts = true;
            _group.interactable = true;

            foreach (var i in goodImages)
            {
                await i.DOFade(1, 0.4f).SetEase(Ease.Linear).AsyncWaitForCompletion();
                await UniTask.Delay(1000);
            }
            
            await UniTask.Delay(1000);

            reloadGroup.DOFade(1, 0.4f).SetEase(Ease.InOutSine).OnComplete(() =>
            {
                reloadGroup.interactable = true;
                reloadGroup.blocksRaycasts = true;
            });
            
            BaseScene.start = false;
        }

        public void ReloadScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        
        public void QuitGame()
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }

        [Button]
        public async void DefeatGame()
        {
            reloadGroup.alpha = 0;
            reloadGroup.blocksRaycasts = false;
            reloadGroup.interactable = false;
            
            print("Game Defeat");
            _group.alpha = 1;
            _group.blocksRaycasts = true;
            _group.interactable = true;
            
            // foreach (var i in badImages)
            // {
            //     await i.DOFade(1, 0.4f).SetEase(Ease.Linear).AsyncWaitForCompletion();
            //     await UniTask.Delay(1000);
            // }

            await badGroup.DOFade(1, 0.4f).SetEase(Ease.Linear).AsyncWaitForCompletion();
            await UniTask.Delay(2000);
            
            badGroup.GetComponent<Animator>().enabled = true;
            await UniTask.Delay(1000);

            await UniTask.Delay(1000);

            reloadGroup.DOFade(1, 0.4f).SetEase(Ease.InOutSine).OnComplete(() =>
            {
                reloadGroup.interactable = true;
                reloadGroup.blocksRaycasts = true;
            });

            BaseScene.start = false;
        }
        
        [Button]
        public async void AverageGame()
        {
            reloadGroup.alpha = 0;
            reloadGroup.blocksRaycasts = false;
            reloadGroup.interactable = false;
            
            print("Game Average");
            _group.alpha = 1;
            _group.blocksRaycasts = true;
            _group.interactable = true;
            
            foreach (var i in averageImages)
            {
                await i.DOFade(1, 0.4f).SetEase(Ease.Linear).AsyncWaitForCompletion();
                await UniTask.Delay(1000);
            }
            
            await UniTask.Delay(1000);

            reloadGroup.DOFade(1, 0.4f).SetEase(Ease.InOutSine).OnComplete(() =>
            {
                reloadGroup.interactable = true;
                reloadGroup.blocksRaycasts = true;
            });
            
            BaseScene.start = false;
        }
    }
}