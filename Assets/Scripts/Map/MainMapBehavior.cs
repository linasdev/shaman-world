using System;
using Save;
using UnityEngine;

namespace Map
{
    public class MainMapBehavior : MonoBehaviour
    {
        [Range(1, 50)]
        public float distanceBetweenNodes;
        public Sprite mapNodeSprite;
        public Sprite mapLockSprite;

        public void Start()
        {
            GameStateManager.LoadFromDisk(); // TODO: Move into a game object from the starting scene
            MapProvider.MainMap.LoadGameObjects(distanceBetweenNodes, transform, mapNodeSprite, mapLockSprite);
        }

        public void OnDestroy()
        {
            MapProvider.MainMap.UnloadGameObjects();
        }
    }
}
