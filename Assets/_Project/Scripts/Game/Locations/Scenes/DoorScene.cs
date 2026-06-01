using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Game.Locations.Scenes
{
    public class DoorScene : BaseScene
    {
        [SerializeField, Space(5)] private Image picture;
        [SerializeField] private Sprite noDoor;
        
        public override void Initialize(LocationSwitcher switcher)
        {
            base.Initialize(switcher);
            switcher.DoorAnomaly += OnDoorAnomaly;
        }

        private void OnDoorAnomaly()
        {
            picture.sprite = noDoor;
        }
    }
}