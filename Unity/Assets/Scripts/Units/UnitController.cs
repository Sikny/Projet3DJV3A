using TerrainGeneration;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Units {
    // Class for a single unit
    public class UnitController : Interactable {
        private UnitGroupController controller;
    
        private Camera cam;
   
        public LayerMask groundMask;

        public Interactable focus;
   
        public float rotationSpeed = 300f;
        public float speed = 5f;

        public MeshRenderer renderer;
    
        private float rotationAngle;
    
        private Vector3 lookAtTarget;
        public Vector3 targetPosition;
        private Vector3 offsetPosition;

        private int rotationTolerance = 1;

        private Color startColor;
        private Quaternion unitRotation;

        private bool isMoving;
        private bool isTurning;
        private bool isRight;
    
        private int isWalkable = 1; // 0 = not walkable 1 = normally walkable 2 = slow walk

        public override void Interact() {
            base.Interact();
        }

        private void Start() {
            offsetPosition = new Vector3(0f, this.gameObject.transform.localScale.y + 0.5f, 0f);
            cam = Camera.main;
        }

        public void SetManager(UnitGroupController controller) {
            this.controller = controller;
        }
        
        // Update is called once per frame
        void Update() {
            if (Input.GetMouseButtonDown(0)) {
                controller.SetSelected(this);
                //SetTargetPosition();
            }

            if (isTurning)
                Turn();
            if(isMoving)
                Move();  
        }

        public void SetTargetPosition(Vector3 position)
        {
            targetPosition = position;

            isMoving = true;
            isTurning = true;
        }
        
        public bool SetTargetPosition()
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 100, 1 << 4) 
                || !Physics.Raycast(ray, out hit, 100, groundMask))
                return false;
            
            if (hit.transform.gameObject.layer == 4) //water
                isWalkable = 0;
            else
                isWalkable = 1;

            float xHit = Mathf.Floor(hit.transform.position.x);
            float zHit = Mathf.Floor(hit.transform.position.z);
            TerrainGrid.Instance.TileX = (int) xHit + (TerrainGrid.Width / 2);
            TerrainGrid.Instance.TileZ = (int) zHit + (TerrainGrid.Height / 2);

            Vector2 startPosition = new Vector2(Mathf.Floor(transform.position.x) + (TerrainGrid.Width / 2),
                Mathf.Floor(transform.position.z) + (TerrainGrid.Height / 2));
            Vector2 endPosition = new Vector2(TerrainGrid.Instance.TileX, TerrainGrid.Instance.TileZ);

            targetPosition = new Vector3(Mathf.Floor(hit.transform.position.x) + 0.5f, 0,
                                 Mathf.Floor(hit.transform.position.z) + 0.5f) + offsetPosition;


            lookAtTarget = new Vector3(targetPosition.x - transform.position.x, transform.position.y,
                targetPosition.z - transform.position.z);

            //Vector of unit to point 
            Vector3 unitToTarget = (targetPosition - transform.position);
            unitToTarget.Normalize();
            //Dot product of the two vectors and Acos 
            rotationAngle = (Mathf.Acos(Vector3.Dot(unitToTarget, gameObject.transform.forward)) *
                             (180 / Mathf.PI));
            isRight = Vector3.Dot(unitToTarget, gameObject.transform.right) > 0;

            isMoving = true;
            isTurning = true;
            return true;
        }

        private void Move()
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        
            if (transform.position == targetPosition)
            {
                isMoving = false;
            }
        }

        private void Turn()
        {
            int factor = isRight ? 1 : -1;
            rotationAngle -= rotationSpeed * Time.deltaTime;
        
            gameObject.transform.Rotate(gameObject.transform.up, rotationSpeed * Time.deltaTime * factor);
        
            if (rotationAngle <= 0)
            {
                isTurning = false;
            }
        }

        private void SetFocus(Interactable newFocus)
        {
            if (focus != newFocus)
            {
                if(focus != null)
                    focus.OnDefocused();
                focus = newFocus;
            
            }
            Debug.Log("hi");
            newFocus.OnFocused();
            startColor = renderer.material.color;
            renderer.material.color = Color.yellow;
        }

        private void RemoveFocus()
        {
            if(focus != null)
                focus.OnDefocused();

            focus = null;
            renderer.material.color = startColor;

        }
    }
}
