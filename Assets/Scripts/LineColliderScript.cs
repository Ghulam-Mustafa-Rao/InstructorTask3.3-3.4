using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class LineColliderScript : MonoBehaviour
{
    PolygonCollider2D polygonCollider;
    MoveBullet moveBullet;
    List<Vector2> colliderPoints = new List<Vector2>();

    // Start is called before the first frame update
    void Start()
    {
        polygonCollider = this.GetComponent<PolygonCollider2D>();
       
        moveBullet = this.GetComponent<MoveBullet>();
    }

    // Update is called once per frame
    void Update()
    {
        //set collider
        colliderPoints = CalculateColliderPoints();
        polygonCollider.SetPath(0, colliderPoints);
        //SetEdgeCollider(myLine);
    }

    List<Vector2> CalculateColliderPoints()
    {
        Vector3[] positions = moveBullet.GetPositions();
        List<Vector2> collidepositions = new List<Vector2>
        {
            new Vector2 (-20,0),
            new Vector2 (0,-0.1f),
            new Vector2 (0,0),
            new Vector2 (0,0.1f)
        };

        return collidepositions;
    }


}