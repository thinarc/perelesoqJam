using UnityEngine;
using UnityEngine.EventSystems;
using _Project.Scripts.Sound;

namespace _Project.Scripts.Game.Menus
{
   
    [RequireComponent(typeof(EventTrigger))]
    public class UIButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
    {
        // Метод срабатывает при наведении мыши на кнопку
        public void OnPointerEnter(PointerEventData eventData)
        {
            // Проверяем, активна ли кнопка для нажатия в данный момент
            if (Time.timeScale == 0 && !EventSystem.current.alreadySelecting) 
            
            
            // Запускаем звук наведения из твоего UIHandle
            SoundManager.Instance.UIHandle.PlayHover(); 
        }

        
        public void OnPointerClick(PointerEventData eventData)
        {
           
            SoundManager.Instance.UIHandle.PlayClick(); 
        }
    }
}