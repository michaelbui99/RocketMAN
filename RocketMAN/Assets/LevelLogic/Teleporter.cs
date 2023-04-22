using UnityEngine;

namespace LevelLogic
{
    public class Teleporter : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private Transform targetPointTransform;

        private void OnCollisionEnter(Collision collision)
        {
            if (!collision.gameObject.CompareTag("Player"))
            {
                return;
            }

            var rb = collision.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
            }

            collision.gameObject.transform.position = targetPointTransform.position;
        }
    }
}