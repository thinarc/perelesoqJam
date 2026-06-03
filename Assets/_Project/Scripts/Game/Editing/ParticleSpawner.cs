using UnityEngine;

namespace _Project.Scripts.Game.Editing
{
    public class ParticleSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject[] dustPrefabs;
        [SerializeField] private GameObject[] dirtPrefabs;

        [Header("Settings")]
        [SerializeField, Space(5)] private int spawnDust = 10;
        [SerializeField] private Vector2 scaleDustRange = new (0.8f, 1.3f);
        
        [SerializeField, Space(5)] private int spawnDirt = 5;
        [SerializeField] private Vector2 scaleDirtRange = new (0.7f, 1.2f);
        
        public void FillPhoto(EditingPhoto targetPhoto)
        {
            Clear(targetPhoto.GetDustRect());
            Clear(targetPhoto.GetDirtRect());

            var dustRect = targetPhoto.GetDustRect().rect;
            var dirtRect = targetPhoto.GetDirtRect().rect;

            for (var i = 0; i < spawnDust; i++)
            {
                var dust = dustPrefabs[Random.Range(0, dustPrefabs.Length)];

                var particle = Instantiate(dust, targetPhoto.GetDustRect());
                var rt = particle.GetComponent<RectTransform>();

                var randomX = Random.Range(dustRect.xMin, dustRect.xMax);
                var randomY = Random.Range(dustRect.yMin, dustRect.yMax);

                rt.anchoredPosition = new Vector2(randomX, randomY);

                var randomScale = Random.Range(scaleDustRange.x, scaleDustRange.y);
                rt.localScale = Vector3.one * randomScale;
                
                rt.localRotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
            }
            
            for (var i = 0; i < spawnDirt; i++)
            {
                var dirt = dirtPrefabs[Random.Range(0, dirtPrefabs.Length)];

                var particle = Instantiate(dirt, targetPhoto.GetDustRect());
                var rt = particle.GetComponent<RectTransform>();

                var randomX = Random.Range(dirtRect.xMin, dirtRect.xMax);
                var randomY = Random.Range(dirtRect.yMin, dirtRect.yMax);

                rt.anchoredPosition = new Vector2(randomX, randomY);

                var randomScale = Random.Range(scaleDustRange.x, scaleDustRange.y);
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