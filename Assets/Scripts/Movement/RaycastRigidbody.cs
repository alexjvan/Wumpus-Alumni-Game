using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastRigidbody : MonoBehaviour {

    //The object's collider, used to find collisions and bounds of the object. Needs to be a Box2D collider
    private new BoxCollider2D collider;
    //Is the object on the ground?
    private Dictionary<RayDirection, bool> collisions = new Dictionary<RayDirection, bool>();
    //Objects's current velocity on x and y.
    public Vector2 velocity = new Vector2();
    //How many raycasts will be sent out inside each unit?
    [SerializeField]
    private float raycastResolution = 16;
    //How many raycast widths will the object treat as a ramp, or something it can climb?
    [SerializeField]
    private int stepSize = 3;
    //Distance added as a buffer for all collision raycasts.
    [SerializeField]
    private float collisionBuffer = 0f;
    //How far away from the object the raycast will start.
    [SerializeField]
    private float skinBuffer = 0.001f;
    //The fastest the object can go in any direction (usually down).
    [SerializeField]
    private float terminalVelocity = 19f;

    private Coroutine stepCoroutine;

    //private RaycastHit2D hit;

    // Use this for initialization
    protected void Start()
    {
        collisions.Add(RayDirection.Up, false);
        collisions.Add(RayDirection.Right, false);
        collisions.Add(RayDirection.Down, false);
        collisions.Add(RayDirection.Left, false);
        collider = GetComponent<BoxCollider2D>();
        //Physics2D.queriesHitTriggers = false;
    }

    // Update is called once per frame
    protected void FixedUpdate()
    {

        MovementLogic();

        //detect collisions
        for (int i = 0; i <= 3; i++)
        {
            SetColliding((RayDirection)i, DetectCollisions((RayDirection)i));
        }

        if (velocity.magnitude > terminalVelocity) velocity = velocity.normalized * terminalVelocity;

        //Apply velocity to position
        transform.position += (Vector3)velocity * Time.fixedDeltaTime;
    }

    protected virtual void MovementLogic() { }

    private bool DetectCollisions(RayDirection direction)
    {
        Vector2 startingPoint;
        Vector2 endingPoint;
        Vector2 rayDirection;
        float rayDistance;

        switch (direction)
        {
            case RayDirection.Down:
                if (velocity.y > 0) return false;
                rayDirection = Vector2.down;
                startingPoint = new Vector2(collider.bounds.min.x, collider.bounds.min.y);
                endingPoint = new Vector2(collider.bounds.max.x, collider.bounds.min.y);
                rayDistance = Mathf.Abs(velocity.y) * Time.fixedDeltaTime;
                break;
            case RayDirection.Up:
                if (velocity.y < 0) return false;
                rayDirection = Vector2.up;
                startingPoint = new Vector2(collider.bounds.min.x, collider.bounds.max.y);
                endingPoint = new Vector2(collider.bounds.max.x, collider.bounds.max.y);
                rayDistance = Mathf.Abs(velocity.y) * Time.fixedDeltaTime;
                break;
            case RayDirection.Right:
                if (velocity.x < 0) return false;
                rayDirection = Vector2.right;
                startingPoint = new Vector2(collider.bounds.max.x, collider.bounds.min.y);
                endingPoint = new Vector2(collider.bounds.max.x, collider.bounds.max.y);
                rayDistance = Mathf.Abs(velocity.x) * Time.fixedDeltaTime;
                break;
            case RayDirection.Left:
                if (velocity.x > 0) return false;
                rayDirection = Vector2.left;
                startingPoint = new Vector2(collider.bounds.min.x, collider.bounds.min.y);
                endingPoint = new Vector2(collider.bounds.min.x, collider.bounds.max.y);
                rayDistance = Mathf.Abs(velocity.x) * Time.fixedDeltaTime;
                break;
            default:
                return false;
        }
        rayDistance += collisionBuffer;
        if (collisionBuffer > skinBuffer) rayDistance -= skinBuffer;

        float raySpacing = 1f / (raycastResolution * Vector2.Distance(startingPoint, endingPoint));

        bool isHit = false;
        bool stepping = false;

        for (float t = 0; t <= 1; t += raySpacing)
        {
            float stepDistance = -Mathf.Abs(raycastResolution * t - raycastResolution / 2f) + raycastResolution / 2f; ;
            Vector2 origin = Vector2.Lerp(startingPoint, endingPoint, t) + (rayDirection * skinBuffer);
            RaycastHit2D hit = Physics2D.Raycast(origin, rayDirection, rayDistance);

            if (hit && !hit.collider.isTrigger)
            {
                //we collided
                //handle step physics
                if (rayDirection.x != 0 && Mathf.Sign(rayDirection.x) == Mathf.Sign(velocity.x))
                {
                    if (stepDistance < stepSize)
                    {
                        //Start step check
                        stepping = true;
                    }
                    else
                    {
                        stepping = false;
                    }
                }

                if (!stepping)
                {
                    //regulate velocity based on collision
                    Vector2 tempVelocity = velocity;
                    velocity = rayDirection * hit.distance / Time.fixedDeltaTime;
                    if (rayDirection.x == 0) velocity.x = tempVelocity.x;
                    if (rayDirection.y == 0) velocity.y = tempVelocity.y;
                }


                isHit = true;
            }
            else if (stepping && stepCoroutine == null && stepDistance <= stepSize && IsColliding(RayDirection.Down))
            {
                //Step
                transform.position += Vector3.up * (1f / raycastResolution) * (stepDistance - (1f / raycastResolution) - 0.01f);
                stepping = false;
            }
        }

        return isHit;
    }

    public bool IsColliding(RayDirection dir)
    {
        if (!collisions.ContainsKey(dir))
            return false;

        return collisions[dir];
    }

    public void SetColliding(RayDirection dir, bool state)
    {
        string call = "";
        if (!collisions[dir] && state) call = "enter";
        if (collisions[dir] && !state) call = "exit";

        collisions[dir] = state;

        switch (call)
        {
            case "enter":
                CollisionEnter();
                break;
            case "exit":
                CollisionExit();
                break;
        }
    }

    protected virtual void CollisionEnter()
    {
    }

    protected virtual void CollisionExit() { }
}

public enum RayDirection
{
    Up = 0,
    Right = 1,
    Down = 2,
    Left = 3,
}