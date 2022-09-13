using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveBullet : MonoBehaviour
{
    public bool moveBulletUp = false;
    public float speed;
    public bool positionset = false;
    public Vector3 startOffSet;
    public LineRenderer lineRenderer;

    public Texture[] textures;
    float fps = 30;
    float fpsCounter;
    int animationStep;
    private void Awake()
    {
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, transform.position - startOffSet);
        lineRenderer.SetWidth(7, 6);
        positionset = true;

    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (positionset)
        {
            lineRenderer.SetPosition(1, transform.position);
        }
        transform.localScale = new Vector3(10, 10, 1);

        //Move object upWards
        if (moveBulletUp)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
        //Destroy object if out of screen
        if (transform.position.y > 60)
        {
            Destroy(this.gameObject);
        }


        //change texture of Line Rendere after 0.30s
        fpsCounter += Time.deltaTime;
        if (fpsCounter >= 1 / fps)
        {
            animationStep++;
            if (animationStep == textures.Length)
            {
                animationStep = 0;
            }
            lineRenderer.material.SetTexture("_MainTex", textures[animationStep]);
            fpsCounter = 0f;

        }

    }

    public Vector3[] GetPositions()
    {
        Vector3[] positions = new Vector3[lineRenderer.positionCount];
        lineRenderer.GetPositions(positions);
        return positions;

    }
}
