using UnityEngine;

public class ChildTrigger : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        // 부모 오브젝트의 GroundTile 스크립트에서 OnTriggerExit 메서드 호출
        GroundTile parentGroundTile = GetComponentInParent<GroundTile>();
        if (parentGroundTile != null)
        {
            parentGroundTile.EndPointReact(other);
        }
    }
}
