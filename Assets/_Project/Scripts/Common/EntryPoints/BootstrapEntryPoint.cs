using _Project.Scripts.Game.Locations;
using _Project.Scripts.Sound;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.Common.EntryPoints
{
    public class BootstrapEntryPoint : MonoBehaviour
    {
        private void Start()
        {
#if !UNITY_EDITOR
            Cursor.lockState = CursorLockMode.Confined;
#endif
            // Cursor.visible = false;

            BaseScene.start = false;
            
            SoundManager.Instance.Initialize();
            
            DontDestroyOnLoad(this);
            SceneManager.LoadScene(1);
        }
    }
}