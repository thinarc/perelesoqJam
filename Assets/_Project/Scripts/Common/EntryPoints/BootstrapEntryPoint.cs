using _Project.Scripts.Sound;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.Common.EntryPoints
{
    public class BootstrapEntryPoint : MonoBehaviour
    {
        private void Start()
        {
            Cursor.lockState = CursorLockMode.Confined;
            
            SoundManager.Instance.Initialize();
            
            DontDestroyOnLoad(this);
            SceneManager.LoadScene(1);
        }
    }
}