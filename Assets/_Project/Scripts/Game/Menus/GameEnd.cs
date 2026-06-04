using UnityEngine;

namespace _Project.Scripts.Game.Menus
{
    public class GameEnd : MonoBehaviour
    {
        public static GameEnd Instance { get; private set; }

        private void Awake() => Instance = this;
        
        public void PassGame()
        {
            print("Game Pass");
        }

        public void DefeatGame()
        {
            print("Game Defeat");
        }
        
        public void AverageGame()
        {
            print("Game Average");
        }
    }
}