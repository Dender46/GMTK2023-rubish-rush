using UnityEngine;
using UnityEngine.UI;

public class GarbageThrowerController : MonoBehaviour
{
    [SerializeField] GameObject m_GarbageBagPrefab;
    [SerializeField] float m_H = 20.0f;
    [SerializeField] float m_Gravity = -18.0f;
    [SerializeField] float m_MaxDistanceRaycast = 20.0f;
    [SerializeField] float m_ReloadTime = 2.0f;

    Image m_ReticleImage;
    Transform m_GarbageBagParentContainer;
    float m_ReloadCooldown = 0.0f;

    void Start()
    {
        m_ReticleImage = GameObject.Find("Reticle").GetComponent<Image>();

        Physics.gravity = Vector3.up * m_Gravity;
        m_GarbageBagParentContainer = new GameObject("C_GarbageBags").transform;
    }

    void Update()
    {
        m_ReloadCooldown -= Time.deltaTime;
        m_ReloadCooldown = Mathf.Max(0.0f, m_ReloadCooldown);

        m_ReticleImage.fillAmount = (m_ReloadTime - m_ReloadCooldown) / m_ReloadTime;

        if (Input.GetButtonDown("Fire1") && m_ReloadCooldown == 0.0f)
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
                var correctedH = displacementY > m_H ? displacementY : m_H;

                Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2.0f * m_Gravity * correctedH);
                Vector3 velocityXZ = displacementXZ 
                    / (Mathf.Sqrt(-2 * correctedH / m_Gravity)
                    + Mathf.Sqrt(2.0f * (displacementY - correctedH) / m_Gravity));

                rb.velocity = velocityXZ + velocityY;

                m_ReloadCooldown = m_ReloadTime;
            } 
        }
    }
}
