using UnityEngine;

public class OnImpact : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Window")
        {
            var windowStatus = collision.gameObject.GetComponent<WindowStatus>();
            if (windowStatus != null)
            {
                windowStatus.Shatter();
                GameManager.AddPoints(GameManager.PointsType.Window);
            }
        }

        Invoke(nameof(DestroyThis), 7.0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "DeliveryArea")
        {
            GameManager.AddPoints(GameManager.PointsType.Regular);
        }

    }

    void DestroyThis()
    {
        Destroy(gameObject);
    }
}
