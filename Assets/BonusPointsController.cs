using UnityEngine;

public class BonusPointsController : MonoBehaviour
{
    [SerializeField] float m_MaxTimer = 2.0f;
    [SerializeField] float m_Speed = 2.0f;

    float m_Timer = 0.0f;

    void Start()
    {
        m_Timer = m_MaxTimer;
    }

    void Update()
    {
        var dt = Time.deltaTime;

        var newPos = transform.position;
        newPos.y += m_Speed * dt;
        transform.position = newPos;

        m_Timer -= dt;
        if (m_Timer < 0.0f)
        {
            DestroyThis();
        }
    }

    void DestroyThis()
    {
        Destroy(gameObject);
    }
}
