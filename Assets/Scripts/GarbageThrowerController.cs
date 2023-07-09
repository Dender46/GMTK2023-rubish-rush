using UnityEngine;

public class GarbageThrowerController : MonoBehaviour
{
    [SerializeField] GameObject m_GarbageBagPrefab;
    [SerializeField] float m_H = 20.0f;
    [SerializeField] float m_Gravity = -18.0f;
    [SerializeField] float m_MaxDistanceRaycast = 20.0f;

    Transform m_GarbageBagParentContainer;

    void Start()
    {
        Physics.gravity = Vector3.up * m_Gravity;
        m_GarbageBagParentContainer = new GameObject("C_GarbageBags").transform;
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            var cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(cameraRay, out hitInfo, m_MaxDistanceRaycast, LayerMask.GetMask("Building")))
            {
                var spawnPos = transform.position + transform.up * 0.6f;
                var newBag = Instantiate(m_GarbageBagPrefab, spawnPos, Quaternion.identity, m_GarbageBagParentContainer);
                
                var rb = newBag.GetComponent<Rigidbody>();

                float displacementY = hitInfo.point.y - spawnPos.y;
                Vector3 displacementXZ = new Vector3(hitInfo.point.x - spawnPos.x, 0.0f, hitInfo.point.z - spawnPos.z);
                
                // We need this because if we aim too high for a variable m_H - equation will be wrong
                Debug.Log("displacementY" + displacementY);
                var correctedH = displacementY > m_H ? displacementY : m_H;
                Debug.Log("correctedH" + correctedH);

                Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2.0f * m_Gravity * correctedH);
                Vector3 velocityXZ = displacementXZ 
                    / (Mathf.Sqrt(-2 * correctedH / m_Gravity)
                    + Mathf.Sqrt(2.0f * (displacementY - correctedH) / m_Gravity));

                rb.velocity = velocityXZ + velocityY;
            } 

        }
    }
}
