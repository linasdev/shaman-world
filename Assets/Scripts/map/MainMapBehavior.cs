using UnityEngine;

namespace map
{
    public class MainMapBehavior : MonoBehaviour
    {
        public Sprite mapNodeSprite;
        
        private MainMap _mainMap;
        
        public void Start()
        {
            _mainMap = new MainMap(transform, mapNodeSprite);
        }
        
        public MapNode GetRootNode()
        {
            return _mainMap.GetRootNode();
        }
    }
}