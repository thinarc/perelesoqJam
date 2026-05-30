using ClothDynamics;
using UnityEngine;

namespace _Project.Scripts.Curtain
{
    public class DraggableCurtain : MonoBehaviour
    {
        [SerializeField] private ClothObjectGPU cloth;
        [SerializeField] private Transform attachParent;

        [SerializeField] private Color curtainColor = Color.white;

        private void Start()
        {
            var attached = new Transform[attachParent.childCount];
            for (var i = 0; i < attachParent.childCount; i++) attached[i] = attachParent.GetChild(i);

            cloth._attachedObjects = attached;
            cloth.GetComponent<MeshRenderer>().material.color = curtainColor;
        }
    }
}
