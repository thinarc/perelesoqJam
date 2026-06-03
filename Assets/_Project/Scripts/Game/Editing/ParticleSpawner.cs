using UnityEngine;

namespace _Project.Scripts.Game.Editing
{
    public class ParticleSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject[] dustPrefabs;
        [SerializeField] private GameObject[] dirtPrefabs;

        [Header("Settings")]
        [SerializeField, Space(5)] private int spawnCount = 10;
        [SerializeField] private Vector2 scaleRange = new (0.8f, 1.3f);
        
        public void FillPhoto(EditingPhoto targetPhoto)
        {
            Clear(targetPhoto.GetDustRect());

            var rect = targetPhoto.GetDustRect().rect;

            for (var i = 0; i < spawnCount; i++)
            {
                var dust = dustPrefabs[Random.Range(0, dustPrefabs.Length)];

                var particle = Instantiate(dust, targetPhoto.GetDustRect());
                var rt = particle.GetComponent<RectTransform>();

                var randomX = Random.Range(rect.xMin, rect.xMax);
                var randomY = Random.Range(rect.yMin, rect.yMax);

                rt.anchoredPosition = new Vector2(randomX, randomY);

                var randomScale = Random.Range(scaleRange.x, scaleRange.y);
                rt.localScale = Vector3.one * randomScale;
                
                rt.localRotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
            }
        }
        
        private void Clear(RectTransform targetPhoto)
        {
            for (var i = targetPhoto.childCount - 1; i >= 0; i--)
            {
                Destroy(targetPhoto.GetChild(i).gameObject);
            }
        }
    }
}