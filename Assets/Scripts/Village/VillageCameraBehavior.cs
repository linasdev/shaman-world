using UnityEngine;

namespace Village
{
    public class VillageCameraBehavior : MonoBehaviour
    {
        public GameObject villagePlayer;

        [Range(0.1f, 1f)]
        public float smoothTime;

        private Vector3 _velocity = Vector3.zero;

        public void Start()
        {
            Vector2 playerPosition = villagePlayer.transform.position;
            transform.position = new Vector3(playerPosition.x, playerPosition.y, transform.position.z);
        }

        public void Update()
        {
            var targetPosition = new Vector3(villagePlayer.transform.position.x, villagePlayer.transform.position.y, transform.position.z);
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, smoothTime);
        }
    }
}
