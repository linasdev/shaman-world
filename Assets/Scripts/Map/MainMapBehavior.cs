using System;
using UnityEngine;

namespace Map
{
    public class MainMapBehavior : MonoBehaviour
    {
        public Sprite mapNodeSprite;
        public Sprite mapLockSprite;
        
        public void Start()
        {
            MapProvider.MainMap.LoadGameObjects(transform, mapNodeSprite, mapLockSprite);
        }

        public void OnDestroy()
        {
            MapProvider.MainMap.UnloadGameObjects();
        }
    }
}