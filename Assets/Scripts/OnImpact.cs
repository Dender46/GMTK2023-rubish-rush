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
            }
        }

        Invoke(nameof(DestroyThis), 7.0f);
    }

    void DestroyThis()
    {
        Destroy(gameObject);
    }
}
