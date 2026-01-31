using UnityEngine;

namespace Map
{
    public class MapCameraBehavior : MonoBehaviour
    {
        public GameObject mapPlayer;

        [Range(0, 1)]
        public float damping;

        public void Start()
        {
            Vector2 playerPosition = mapPlayer.transform.position;
            transform.position = new Vector3(playerPosition.x, playerPosition.y, transform.position.z);
        }

        public void Update()
        {
            Vector2 towardsPlayer = mapPlayer.transform.position - transform.position;
            var positionDelta = towardsPlayer * (damping * Time.deltaTime);

            transform.position += new Vector3(positionDelta.x, positionDelta.y, 0f);
        }
    }
}
