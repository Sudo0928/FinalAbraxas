using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class FollowCamera : MonoBehaviour {
    public GameManager gameManager { get { return GameManager.gameManager; } }
    //  > 따라다니도록 할 타깃을 참조합니다.
    [SerializeField] private Transform _FollowTargetTransform = null;

    //  > 추적 속도
    [SerializeField] [Range(0.0f, 10.0f)] private float _FollowSpeed = 5.0f;

    //  > 카메라에 대한 프로퍼티
    public Camera camera { get; private set; }

    [SerializeField] private float MaxX , MinX;
    [SerializeField] private float MaxZ , MinZ;

    private void Awake() {
        IEnumerator FollowTarget() {
            while (true)
            {
                if (_FollowTargetTransform)
                {
                    Vector3 TargetPos = _FollowTargetTransform.position;
                  // TargetPos.x = Mathf.Clamp(TargetPos.x,MinX,MaxX);
                  // TargetPos.z = Mathf.Clamp(TargetPos.z,MinZ,MaxZ);

                    transform.position = Vector3.Lerp(
                        transform.position, TargetPos,
                        _FollowSpeed * Time.deltaTime);
                }
                    yield return null;
                
            }
        }

        camera = GetComponentInChildren<Camera>();

        StartCoroutine(FollowTarget());
    }

    public void GetMinMax(float maxX,float minX, float maxZ, float minZ  )
	{

        MaxX = maxX;
        MinX = minX;
        MaxZ = maxZ;
        MinZ = minZ;
	}

	private void Start()
	{
        
	}
}
