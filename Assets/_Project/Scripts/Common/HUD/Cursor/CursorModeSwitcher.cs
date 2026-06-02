using UnityEngine;

namespace _Project.Scripts.Common.HUD.Cursor
{
    public class CursorModeSwitcher : MonoBehaviour
    {
        public static CursorModeSwitcher Instance { get; private set; }
        private void Awake() => Instance = this;
        
        [SerializeField] private UICursorController cursor;

        public void SetDefaultMode()
        {
            cursor.SetMode(UICursorController.CursorMod.Default);
        }

        public void SetToolMode()
        {
            cursor.SetMode(UICursorController.CursorMod.Tool);
        }
    }
}