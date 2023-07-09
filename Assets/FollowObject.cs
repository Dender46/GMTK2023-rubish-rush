using UnityEngine;

public class FollowObject : MonoBehaviour
{
    [SerializeField] private Transform m_FollowObject;

    void Update()
    {
        transform.position = m_FollowObject.position;
        transform.rotation = m_FollowObject.rotation;
    }
}
