using UnityEngine;

namespace _Project.Scripts.Game.Editing.Photo
{
    public class SprayTool : MonoBehaviour
    {
        [Header("Setup")]
        [SerializeField] private RectTransform sprayContainer;
        [SerializeField] private GameObject[] sprayPrefabs;

        [Header("Settings")]
        [SerializeField] private float spawnInterval = 0.03f;
        [SerializeField] private Vector2 scaleRange = new(0.8f, 1.2f);

        private float _timer;

        public static bool InPhotoSpawn;
        
        public Canvas canvas;

        private void Start()
        {
            InPhotoSpawn = false;
        }

        private void Update()
        {
            if (EditController.Instance.currentTool != EditController.ToolType.Spray)
                return;

            if (!Input.GetMouseButton(0))
                return;
            
            if (!InPhotoSpawn) return;
            
            if (sprayContainer.childCount >= EditController.Instance.targetS * 2f) return;

            _timer += Time.deltaTime;

            if (!(_timer >= spawnInterval)) return;
            _timer = 0f;
            SpawnSpot();
        }

        private void SpawnSpot()
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                sprayContainer,
                Input.mousePosition,
                canvas.worldCamera,
                out var localPoint
            );
            
            var prefab = sprayPrefabs[Random.Range(0, sprayPrefabs.Length)];

            var obj = Instantiate(prefab, sprayContainer);

            var rt = obj.GetComponent<RectTransform>();
            
            rt.anchoredPosition = localPoint;

            var randomScale = Random.Range(scaleRange.x, scaleRange.y);

            rt.localScale = Vector3.one * randomScale;

            rt.localRotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));

            var pos = rt.localPosition;
            pos.z = 0;
            rt.localPosition = pos;
        }
    }
}