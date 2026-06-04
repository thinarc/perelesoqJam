using _Project.Scripts.Sound;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

namespace _Project.Scripts.Game.Editing.Photo
{
    public class SprayTool : MonoBehaviour
    {
        [Header("Setup")]
        private RectTransform _sprayContainer;
        [SerializeField] private GameObject[] sprayPrefabs;

        [Header("Settings")]
        [SerializeField] private float spawnInterval = 0.03f;
        [SerializeField] private Vector2 scaleRange = new(0.8f, 1.2f);

        private float _timer;

        public static bool InPhotoSpawn;
        
        public Canvas canvas;

        public Vector2 spawnOffset;
        public ParticleSystem sprayParticles;

        public static SprayTool Instance { get; private set; }
        private void Awake() => Instance = this;

        private void Start()
        {
            InPhotoSpawn = false;
        }
        
        public void UpdateContainer(RectTransform container)
        {
            _sprayContainer = container;
        }

        private void Update()
        {
            if (EditController.Instance.currentTool != EditController.ToolType.Spray)
            {
                sprayParticles.Stop();
                SoundManager.Instance.ActionHandle.StopSpray();
                return;
            }
            
            if (!Input.GetMouseButton(0))
            {
                sprayParticles.Stop();
                SoundManager.Instance.ActionHandle.StopSpray();
                return;
            }
            
            if (!InPhotoSpawn)
            {
                sprayParticles.Stop();
                SoundManager.Instance.ActionHandle.StopSpray();
                return;
            }
            
            if (_sprayContainer.childCount >= EditController.Instance.targetS * 2f) return;

            _timer += Time.deltaTime;
            sprayParticles.Play();
            SoundManager.Instance.ActionHandle.PlaySpray(); 

            if (!(_timer >= spawnInterval)) return;
            _timer = 0f;
            SpawnSpot();
            
        }

        private void SpawnSpot()
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _sprayContainer,
                Input.mousePosition,
                canvas.worldCamera,
                out var localPoint
            );
            
            var prefab = sprayPrefabs[Random.Range(0, sprayPrefabs.Length)];

            var obj = Instantiate(prefab, _sprayContainer);

            var rt = obj.GetComponent<RectTransform>();
            
            rt.anchoredPosition = localPoint + spawnOffset;

            var randomScale = Random.Range(scaleRange.x, scaleRange.y);

            rt.localScale = Vector3.one * randomScale;

            rt.localRotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));

            var pos = rt.localPosition;
            pos.z = 0;
            rt.localPosition = pos;
        }
    }
}