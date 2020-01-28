using UnityEngine;
using Toolbox;

public class FollowPathUnit : MonoBehaviour
{
    public Transform target;

    TileComponent tileComponent;
    Movement2D movement;
    FollowPath2D followPath;

    void Start()
    {
        tileComponent = GetComponent<TileComponent>();
        movement = GetComponent<Movement2D>();
        followPath = GetComponent<FollowPath2D>();
    }

    void FixedUpdate()
    {
        LinePath lp = tileComponent.FindLinePathClosest(transform.position, target.position);
        movement.steering = followPath.GetSteering(lp);
        lp.Draw();

        tileComponent.SetTile(transform.position);
    }
}
