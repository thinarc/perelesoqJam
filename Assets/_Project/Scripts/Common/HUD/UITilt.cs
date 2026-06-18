using _Project.Scripts.Game.Locations;
using UnityEngine;

namespace _Project.Scripts.Common.HUD
{
    [RequireComponent(typeof(RectTransform))]
    public class UITilt : MonoBehaviour
    {
        [Header("Rotation")]
        [SerializeField] private float rotationX = 8f;
        [SerializeField] private float rotationY = 8f;

        [Header("Position")]
        [SerializeField] private float moveX = 20f;
        [SerializeField] private float moveY = 20f;

        [Header("Smoothing")]
        [SerializeField] private float smoothSpeed = 8f;

        private RectTransform rectTransform;

        private Vector3 initialPosition;
        private Quaternion initialRotation;

        [SerializeField, Space(5)] private bool yaStreet;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();

            initialPosition = rectTransform.localPosition;
            initialRotation = rectTransform.localRotation;
        }

        private void Update()
        {
            if (yaStreet && LocationSwitcher.Instance._currentLoc.name != "street") return;
            
            Vector2 mouse = Input.mousePosition;

            float normalizedX = (mouse.x / Screen.width - 0.5f) * 2f;
            float normalizedY = (mouse.y / Screen.height - 0.5f) * 2f;

            Quaternion targetRotation = initialRotation *
                                        Quaternion.Euler(
                                            -normalizedY * rotationX,
                                            normalizedX * rotationY,
                                            0f);

            Vector3 targetPosition = initialPosition +
                                     new Vector3(
                                         normalizedX * moveX,
                                         normalizedY * moveY,
                                         0f);

            rectTransform.localRotation = Quaternion.Lerp(
                rectTransform.localRotation,
                targetRotation,
                Time.unscaledDeltaTime * smoothSpeed);

            rectTransform.localPosition = Vector3.Lerp(
                rectTransform.localPosition,
                targetPosition,
                Time.unscaledDeltaTime * smoothSpeed);
        }
    }
}