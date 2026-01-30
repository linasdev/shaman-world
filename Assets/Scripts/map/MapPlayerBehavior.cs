using UnityEngine;
using UnityEngine.InputSystem;

namespace map
{
    public class MapPlayerBehavior : MonoBehaviour
    {
        private MapNode _selectedMapNode;
        private Transform _transform;
        private InputAction _moveAction;

        public Sprite MapNodeSprite;
        
        void Start()
        {
            _selectedMapNode = new MainMap(MapNodeSprite).GetRootNode();
            _transform = GetComponent<Transform>();
            _moveAction = InputSystem.actions.FindAction("Move");
        }

        void Update()
        {
            if (!_moveAction.triggered)
            {
                return;
            }
            
            Vector2Int movementDirection = Vector2Int.RoundToInt(_moveAction.ReadValue<Vector2>());

            if (movementDirection.sqrMagnitude == 0)
            {
                return;
            }

            MapDirection? direction = MapDirectionMethods.FromVector2Int(movementDirection);

            if (direction == null)
            {
                return;
            }

            MapNode nextMapNode =  _selectedMapNode.GetNeighbor((MapDirection)direction);

            if (nextMapNode == null)
            {
                return;
            }
            
            _selectedMapNode = nextMapNode;
            _transform.position = new Vector3(_selectedMapNode.GetPosition().x, _selectedMapNode.GetPosition().y, 0);
        }
    }
}