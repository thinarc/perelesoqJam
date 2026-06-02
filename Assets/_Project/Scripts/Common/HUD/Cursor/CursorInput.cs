using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.Common.HUD.Cursor
{
    public class CursorInput : MonoBehaviour
    {
        [SerializeField] private UICursorController cursor;

        private void Start()
        {
            InputLoop().Forget();
        }

        private async UniTaskVoid InputLoop()
        {
            while (true)
            {
                if (Input.GetMouseButtonDown(0))
                    cursor.SetState(UICursorController.CursorState.Click);

                if (Input.GetMouseButtonUp(0))
                    cursor.SetState(UICursorController.CursorState.Idle);

                await UniTask.Yield();
            }
        }
    }
}