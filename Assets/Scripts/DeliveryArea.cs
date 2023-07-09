using UnityEngine;

public class DeliveryArea : MonoBehaviour
{
    [SerializeField] int m_MaxGarbage = 3;
    int m_AccumulatedGarbage = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "GarbageBag")
        {
            m_AccumulatedGarbage++;
            if (m_AccumulatedGarbage >= m_MaxGarbage)
            {
                Destroy(gameObject);
            }
        }
    }
}
