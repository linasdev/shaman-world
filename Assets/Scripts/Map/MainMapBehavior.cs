using System;
using Save;
using UnityEngine;

namespace Map
{
    public class MainMapBehavior : MonoBehaviour
    {
        public Sprite mapNodeSprite;
        public Sprite mapLockSprite;
        
        public void Start()
        {
            GameStateManager.LoadFromDisk(); // TODO: Move into a game object from the starting scene
            MapProvider.MainMap.LoadGameObjects(transform, mapNodeSprite, mapLockSprite);
        }

        public void OnDestroy()
        {
            MapProvider.MainMap.UnloadGameObjects();
        }
    }
}