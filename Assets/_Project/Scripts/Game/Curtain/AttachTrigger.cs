using ClothDynamics;
using UnityEngine;

namespace _Project.Scripts.Curtain
{
    public class AttachTrigger : MonoBehaviour
    {
        [SerializeField] private Transform attachedDot;

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Curtain"))
            {
                var curtain = other.gameObject.GetComponent<ClothObjectGPU>();
                if (curtain == null) return;
                var att = curtain._attachedObjects;
                var newAtt = new Transform[att.Length + 1];
                for (var i = 0; i < att.Length; i++) newAtt[i] = att[i];
                newAtt[att.Length] = attachedDot;
                curtain._attachedObjects = newAtt;
                
                print("Connect Dot");
            }
        }
    }
}
