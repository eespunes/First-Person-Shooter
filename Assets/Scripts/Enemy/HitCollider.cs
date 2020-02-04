using UnityEngine;

public class HitCollider : MonoBehaviour
{
    public enum THitColliderType
    {
        HEAD,
        HELIX,
        BODY
    }
    public THitColliderType m_HitColliderType;
    public DroneEnemy m_DroneEnemy;
}