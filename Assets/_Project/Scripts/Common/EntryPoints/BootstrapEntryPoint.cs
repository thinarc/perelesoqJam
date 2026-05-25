using _Project.Scripts.Sound;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.Common.EntryPoints
{
    public class BootstrapEntryPoint : MonoBehaviour
    {
        private void Start()
        {
            PlayerPrefs.SetFloat("bg", 1f);
            PlayerPrefs.SetFloat("sfx", 0.75f);
            
            SoundManager.Instance.Initialize();
            
            SceneManager.LoadScene(1);
        }
    }
}