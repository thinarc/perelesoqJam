using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Game.Editing.Photo;
using Cysharp.Threading.Tasks;
using UnityEngine;

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
        
        private ParticleSpawner _spawner;
        
        private Queue<EditingPhoto> _queue;

        private void Awake() => Instance = this;

        private async void Start()
        {
            _spawner = GetComponent<ParticleSpawner>();
            
            GetComponentsInChildren<EditingPhoto>().ToList().ForEach(photo => _queue.Enqueue(photo));

            while (true)
            {
                await EditCycle();
                if (_queue.Count == 0) break;
            }
        }

        private async UniTask EditCycle()
        {
            var photo = _queue.Dequeue();
            _spawner.FillPhoto(photo);
            
            var targetS = Random.Range(minTargetShine, maxTargetShine);

            var d = dust.childCount;
            var r = dirt.childCount;
            var s = shine.childCount;
            
            bool complete = d == 0 && r == 0 && 
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