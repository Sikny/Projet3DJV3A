using TerrainGeneration;
using Units.proto;
using UnitSystem;
using UnityEngine;

public class RemotedUnit : AbstractUnit
{
    private int isWalkable = 1;
    protected bool isSelected;
    
    public RemotedUnit(int numberEntity) : 
        base(numberEntity,new Vector3(-2,1,0))
    {
        speedEntity = 1.0f;
        isSelected = true;
    }
        
    public override bool init(GameObject gameobjectModel)
    {
        return base.init(gameobjectModel);
    }

    public override void update()
    {
        if (isSelected)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (!SetTargetPosition()) return;
            }
        }
        if(isMoving)
            Move();
        updateGameobject();
    }

    public override bool kill()
    {
        return true;
    }

    protected override void attack(AbstractUnit anotherUnit)
    {
            
    }
    
    public bool SetTargetPosition()
    {
        Ray ray = UnitLibData.cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;


        if (Physics.Raycast(ray, out hit, 100, 1 << 4) ||
            !Physics.Raycast(ray, out hit, 100, UnitLibData.groundMask)) return false;
        else
        {
            if (hit.transform.gameObject.layer == 4) //water 
                isWalkable = 0;
            else if (hit.transform.gameObject.layer == 8) //ground 
                isWalkable = 1;

            float xHit = Mathf.Floor(hit.transform.position.x);
            float zHit = Mathf.Floor(hit.transform.position.z);
            TerrainGrid.Instance.TileX = (int) xHit + (TerrainGrid.Width / 2);
            TerrainGrid.Instance.TileZ = (int) zHit + (TerrainGrid.Height / 2);

            Vector2 startPosition = new Vector2(Mathf.Floor(position.x) + (TerrainGrid.Width / 2),
                Mathf.Floor(position.z) + (TerrainGrid.Height / 2));
            Vector2 endPosition = new Vector2(TerrainGrid.Instance.TileX, TerrainGrid.Instance.TileZ);

            //pathFinder.BuildPath(startPosition, endPosition, isWalkable);


            //targetPosition = hit.point + offsetPosition;
            targetPosition = new Vector3(Mathf.Floor(hit.transform.position.x)-0.5f, 1,
                Mathf.Floor(hit.transform.position.z)-0.5f) ;


            lookAtTarget = new Vector3(targetPosition.x - position.x, position.y,
                targetPosition.z - position.z);

            //Vector of unit to point 
            Vector3 unitToTarget = (targetPosition - position);
            unitToTarget.Normalize();
            //Dot product of the two vectors and Acos 
            //rotationAngle = (Mathf.Acos(Vector3.Dot(unitToTarget, gameObject.transform.forward)) *
            //      (180 / Mathf.PI));
            //isRight = Vector3.Dot(unitToTarget, gameObject.transform.right) > 0;

            isMoving = true;
            isTurning = true;
            return true;
        }
    }
}
