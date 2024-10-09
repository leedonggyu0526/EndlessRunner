using UnityEngine;

public class ChildTrigger : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        // �θ� ������Ʈ�� GroundTile ��ũ��Ʈ���� OnTriggerExit �޼��� ȣ��
        GroundTile parentGroundTile = GetComponentInParent<GroundTile>();
        if (parentGroundTile != null)
        {
            parentGroundTile.EndPointReact(other);
        }
    }
}
