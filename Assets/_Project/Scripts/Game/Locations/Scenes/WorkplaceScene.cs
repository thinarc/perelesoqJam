using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Game.Locations.Scenes
{
    public class WorkplaceScene : BaseScene
    {
        [SerializeField, Space(5)] private Image picture;
        [SerializeField] private Sprite darkplace;
        
        public override void Initialize(LocationSwitcher switcher)
        {
            base.Initialize(switcher);
            switcher.LampBroken += OnLampBroken;
        }

        private void OnLampBroken()
        {
            picture.sprite = darkplace;
        }
    }
}