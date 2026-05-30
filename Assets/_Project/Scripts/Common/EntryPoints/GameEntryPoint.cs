using _Project.Scripts.Game.Locations;
using _Project.Scripts.Game.Menus;
using UnityEngine;

namespace _Project.Scripts.Common.EntryPoints
{
    public class GameEntryPoint : MonoBehaviour
    {
        public static GameEntryPoint Instance  { get; private set; }
        
        public LocationSwitcher locationSwitcher;
        public MainMenu mainMenu;

        private void Awake() => Instance = this;

        private void Start()
        {
            locationSwitcher.StartLocation();
            // mainMenu.ShowMenu();
        }
    }
}