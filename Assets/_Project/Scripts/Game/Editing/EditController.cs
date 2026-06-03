using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Game.Editing
{
    public class EditController : MonoBehaviour
    {
        private ParticleSpawner _spawner;
        
        private List<EditingPhoto> _photos;

        private void Start()
        {
            _spawner = GetComponent<ParticleSpawner>();
            
            _photos = new List<EditingPhoto>(GetComponentsInChildren<EditingPhoto>());
            _spawner.FillPhoto(_photos[0]);
        }
    }
}