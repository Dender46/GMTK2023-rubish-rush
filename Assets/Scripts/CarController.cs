using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] float m_Speed = 10.0f;
    [SerializeField] float m_Decceleration = 1.0f;
    [SerializeField] float m_MaxSpeedForward = 20.0f;
    [SerializeField] float m_MaxSpeedBackward = -20.0f;
    [SerializeField] float m_RotationSpeed = 20.0f;
    [Header("Audio")]
    [SerializeField] AudioClip m_AudioReversing;
    [SerializeField] float m_IdlePitch = 0.6f;
    [SerializeField] float m_RunningPitch = 1.0f;

    Rigidbody m_Rigidbody;
    AudioSource m_AudioSource;
    AudioSource m_AudioSourceReversing;
    float m_Velocity = 0.0f;
    float m_RotationY = 0.0f;

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_AudioSource = GetComponent<AudioSource>();
        m_AudioSourceReversing = GameObject.Find("AudioSourceReversing").GetComponent<AudioSource>();
        m_AudioSourceReversing.Stop();
    }

    void Update()
    {
        var dt = Time.deltaTime;
        var acceleration = Input.GetAxis("Vertical");
        var steering = Input.GetAxis("Horizontal");
        
        m_Velocity += acceleration * m_Speed * dt;
        m_Velocity += m_Velocity > 0.0f ? -m_Decceleration * dt : m_Decceleration * dt;
        m_Velocity = Mathf.Clamp(m_Velocity, m_MaxSpeedBackward, m_MaxSpeedForward);


        if (m_Rigidbody.velocity.magnitude > 0.0f)
        {
            m_RotationY += steering * m_RotationSpeed * m_Rigidbody.velocity.magnitude * dt;
            transform.rotation = Quaternion.Euler(0.0f, m_RotationY, 0.0f);
        }

        m_AudioSource.pitch = Mathf.Lerp(m_IdlePitch, m_RunningPitch, m_Velocity / m_MaxSpeedForward);
        if (acceleration < 0.0f)
        {
            if (!m_AudioSourceReversing.isPlaying)
            {
                m_AudioSourceReversing.PlayOneShot(m_AudioReversing);
            }
        }
        else
        {
            m_AudioSourceReversing.Stop();
        }
    }

    void FixedUpdate()
    {
        m_Rigidbody.velocity = m_Velocity * Time.fixedDeltaTime * transform.forward;
    }
}
