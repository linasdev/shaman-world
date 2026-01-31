using UnityEngine;

namespace Map
{
    public class MainMapBehavior : MonoBehaviour
    {
        public Sprite mapNodeSprite;
        public Sprite mapLockSprite;
        
        private MainMap _mainMap;
        
        public void Start()
        {
            _mainMap = new MainMap(transform, mapNodeSprite, mapLockSprite);
        }
        
        public MapNode GetRootNode()
        {
            return _mainMap.GetRootNode();
        }

        public bool UnlockAtPosition(Vector2Int position)
        {
            return _mainMap.GetRootNode().UnlockAtPosition(Vector2Int.zero, position);
        }
    }
}