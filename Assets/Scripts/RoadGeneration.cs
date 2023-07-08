using System.Collections;
using UnityEngine;

public class RoadGeneration : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] GameObject m_RoadStraightPrefab;
    [SerializeField] GameObject m_RoadTurnPrefab;
    [Header("Parameters")]
    [SerializeField] int m_RoadDistance = 20;
    [SerializeField] float m_ChanceOfTurning = 0.5f;
    [Header("Demo")]
    [SerializeField] bool m_Demo_ShowDebugLog = true;
    [SerializeField] float m_Demo_WaitForSeconds = 1;

    enum TurnDirection {Left, Right};
    TurnDirection m_LastTurn = TurnDirection.Left;

    Transform m_RoadBuilder;
    Vector3 m_NextIterationPos = Vector3.forward;

    IEnumerator Start()
    {
        m_RoadBuilder = new GameObject("RoadBuilder").transform;
        m_RoadBuilder.position = m_NextIterationPos * 3.0f;

        int i = 0;
        while (i++ < m_RoadDistance)
        {
            bool shouldTurn = Random.Range(0.0f, 1.0f) < m_ChanceOfTurning;
            // Spawn and new road on the next iteration tile
            var roadInstance = Instantiate(shouldTurn ? m_RoadTurnPrefab : m_RoadStraightPrefab, m_RoadBuilder, false);

            yield return WaitWithDebugMessage("Spawned new road in builder");
            
            if (shouldTurn)
            {
                var currentTurn = m_LastTurn == TurnDirection.Left ? TurnDirection.Right : TurnDirection.Left;
                // Set current rotation for new turning road tile
                roadInstance.transform.localScale = new Vector3(
                    currentTurn == TurnDirection.Left ? 1.0f/*left*/ : -1.0f/*right*/,
                    1.0f,
                    1.0f
                );
                yield return WaitWithDebugMessage("Set correct rotation for a turn road");

                // Place road in the world space before rotating builder
                roadInstance.transform.SetParent(null, true);

                m_RoadBuilder.Rotate(Vector3.up, currentTurn == TurnDirection.Left ? -90.0f : 90.0f);
                yield return WaitWithDebugMessage("Rotated builder");

                m_LastTurn = currentTurn;
            }

            // Place road in the world space
            roadInstance.transform.SetParent(null, true);
            yield return WaitWithDebugMessage("Placed tile in a world space");

            m_RoadBuilder.position += m_RoadBuilder.forward * 3.0f;

            yield return WaitWithDebugMessage("Moved builder forward");
        }
    }

    IEnumerator WaitWithDebugMessage(string debugMessage)
    {
        if (m_Demo_ShowDebugLog)
        {
            Debug.Log(debugMessage);
            yield return new WaitForSeconds(m_Demo_WaitForSeconds);
        }
    }

    void Update()
    {
        
    }
}
