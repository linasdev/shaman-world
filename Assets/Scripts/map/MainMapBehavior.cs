using UnityEngine;

namespace map
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
    }
}