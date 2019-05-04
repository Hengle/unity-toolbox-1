using UnityEngine;
using Toolbox;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

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
        List<Vector3> nodes = tileComponent.FindPathClosest(transform.position, target.position);
        LinePath lp = new LinePath(nodes);
        movement.steering = followPath.GetSteering(lp);
        lp.Draw();

        tileComponent.SetTile(transform.position);
    }
}
