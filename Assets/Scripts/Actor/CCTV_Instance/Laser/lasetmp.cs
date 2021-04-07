using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lasetmp : MonoBehaviour
{

    public float leftValue =0;
    public float rightValue = 0;
    float z;
    public Vector3 originStart { get; set; }
    public Vector3 OriginEnd { get; set; }

    private Vector3 currentVec;
    private Vector3 targetVec;

    public enum Direction {Left, Right };
    public Direction thisDirection;

    LineRenderer Line;
    // Start is called before the first frame update
    void Start()
    {
        Line = gameObject.GetComponent<LineRenderer>();
        originStart = Line.GetPosition(0);
        OriginEnd = Line.GetPosition(1);

        z = transform.localPosition.z;
    }

    // Update is called once per frame
    void Update()
    {
        if (leftValue == 0 && rightValue == 0)
        {
            switch (thisDirection)
            {
                case Direction.Left:
                    Line.SetPosition(0, originStart);
                    break;

                case Direction.Right:
                    Line.SetPosition(1, OriginEnd);
                    break;
            }
        }
        else
        {
            switch (thisDirection)
            {
                case Direction.Left:

                    targetVec = new Vector3(originStart.x, originStart.y, leftValue);
                    currentVec = Vector3.Lerp(Line.GetPosition(0), targetVec, 0.3f);

                    Line.SetPosition(0, currentVec);
                    break;

                case Direction.Right:
                    targetVec = new Vector3(OriginEnd.x, OriginEnd.y, rightValue);
                    currentVec = Vector3.Lerp(Line.GetPosition(1), targetVec, 0.3f);
                    Line.SetPosition(1, currentVec);
                    break;
            }
        }

        // transform.localScale = new Vector3(1, o, 1);
        //transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, z - o);
        
    }

    public void Zero(float f)
    {
        switch (thisDirection)
        {
            case Direction.Left:
                leftValue = f;
                break;

            case Direction.Right:
                rightValue = f;
                break;
        }

    }
}
