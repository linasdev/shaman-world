using UnityEngine;
using UnityEngine.InputSystem;

namespace map
{
    public class MapPlayerBehavior : MonoBehaviour
    {
        public GameObject mainMap;
        
        private MapNode _selectedMapNode;
        private InputAction _moveAction;
        
        public void Start()
        {
            _selectedMapNode = mainMap.GetComponent<MainMapBehavior>().GetRootNode();
            _moveAction = InputSystem.actions.FindAction("Move");
        }

        public void Update()
        {
            if (!_moveAction.triggered)
            {
                return;
            }
            
            var movementDirection = Vector2Int.RoundToInt(_moveAction.ReadValue<Vector2>());

            if (movementDirection.sqrMagnitude == 0)
            {
                return;
            }

            var direction = MapDirectionMethods.FromVector2Int(movementDirection);

            if (direction == null)
            {
                return;
            }

            var nextMapNode =  _selectedMapNode.GetNeighbor((MapDirection)direction);

            if (nextMapNode == null || nextMapNode.IsLocked())
            {
                return;
            }
            
            _selectedMapNode = nextMapNode;
            transform.position = new Vector3(_selectedMapNode.GetPosition().x, _selectedMapNode.GetPosition().y, 0);
        }
    }
}