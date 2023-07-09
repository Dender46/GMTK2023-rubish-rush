using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] float m_Speed = 10.0f;
    [SerializeField] float m_Decceleration = 1.0f;
    [SerializeField] float m_MaxSpeedForward = 20.0f;
    [SerializeField] float m_MaxSpeedBackward = -20.0f;
    [SerializeField] float m_RotationSpeed = 20.0f;

    Rigidbody m_Rigidbody;
    float m_Velocity = 0.0f;
    float m_RotationY = 0.0f;

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        var dt = Time.deltaTime;
        var acceleration = Input.GetAxis("Vertical");
        var steering = Input.GetAxis("Horizontal");
        
        m_Velocity += acceleration * m_Speed * dt;
        if (m_Velocity > 0.0f)
        {
            m_Velocity -= m_Decceleration * dt;
        }
        else if (m_Velocity < 0.0f)
        {
            m_Velocity += m_Decceleration * dt;
        }
        m_Velocity = Mathf.Clamp(m_Velocity, m_MaxSpeedBackward, m_MaxSpeedForward);

        if (m_Rigidbody.velocity.magnitude > 0.0f)
        {
            m_RotationY += steering * m_RotationSpeed * m_Rigidbody.velocity.magnitude * dt;
            transform.rotation = Quaternion.Euler(0.0f, m_RotationY, 0.0f);
        }
    }

    void FixedUpdate()
    {
        m_Rigidbody.velocity = m_Velocity * Time.fixedDeltaTime * transform.forward;
    }
}
