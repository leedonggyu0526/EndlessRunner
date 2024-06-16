using UnityEngine;

public class CameraFollow : MonoBehaviour {

    [SerializeField] Transform player;
    Vector3 offset;

	private void Start () {
        offset = transform.position - player.position;
	}

    private void Update()
    {
        Vector3 targetPos = player.position - player.forward * offset.magnitude;
        targetPos.y = offset.y;
        transform.position = targetPos;
    }
}