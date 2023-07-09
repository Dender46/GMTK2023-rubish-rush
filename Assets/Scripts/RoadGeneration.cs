using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGeneration : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] GameObject m_RoadStraightPrefab;
    [SerializeField] GameObject m_RoadTurnPrefab;
    [SerializeField] List<GameObject> m_HousePrefabs;
    [Header("Parameters")]
    [SerializeField] int m_RoadDistance = 20;
    [SerializeField] float m_ChanceOfTurning = 0.5f;
    [SerializeField] float m_HouseY = 2.0f;
    [SerializeField] float m_HouseScale = 2.2f;
    [Header("Demo")]
    [SerializeField] bool m_Demo_ShowDebugLog = true;
    [SerializeField] float m_Demo_WaitForSeconds = 1;

    // We remember last turn so that road in the future might not lead into itself
    enum TurnDirection {Left, Right};
    TurnDirection m_LastTurn = TurnDirection.Left;

    Transform m_RoadBuilder;
    Transform m_RoadsContainer;

    struct BuildingLocation
    {
        public Vector3 pos;
        public Vector3 lookAt;
    }
    List<BuildingLocation> m_PossibleLocationsOfBuildings = new();

    IEnumerator Start()
    {
        yield return GenerateRoad();
        yield return GenerateBuildings();
    }

    IEnumerator GenerateRoad()
    {
        m_RoadsContainer = new GameObject("RoadsContainer").transform;
        m_RoadsContainer.SetParent(transform);

        m_RoadBuilder = new GameObject("RoadBuilder").transform;
        m_RoadBuilder.SetParent(transform);

        // First road
        Instantiate(m_RoadStraightPrefab, m_RoadsContainer);
        m_RoadBuilder.position += m_RoadBuilder.forward * 3.0f;

        for (int i = 0; i < m_RoadDistance; i++)
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
                roadInstance.transform.SetParent(m_RoadsContainer, true);

                m_RoadBuilder.Rotate(Vector3.up, currentTurn == TurnDirection.Left ? -90.0f : 90.0f);
                yield return WaitWithDebugMessage("Rotated builder");

                m_LastTurn = currentTurn;

                // Push new location for buildings
                //m_PossibleLocationsOfBuildings.Add(new BuildingLocation {
                //    pos = roadInstance.transform.position,
                //    dir = roadInstance.transform.position,
                //});
            }
            else
            {
                var pos = m_RoadBuilder.position;
                var lookAt = pos;
                lookAt.y = m_HouseY;

                // Push new location for buildings
                m_PossibleLocationsOfBuildings.Add(new BuildingLocation {
                    pos = pos + m_RoadBuilder.transform.right * 3.0f,
                    lookAt = lookAt,
                });
                m_PossibleLocationsOfBuildings.Add(new BuildingLocation {
                    pos = pos - m_RoadBuilder.transform.right * 3.0f,
                    lookAt = lookAt,
                });
            }

            // Place road from RoadBuilder space in to the world space
            roadInstance.transform.SetParent(m_RoadsContainer, true);
            yield return WaitWithDebugMessage("Placed tile in a world space");

            m_RoadBuilder.position += m_RoadBuilder.forward * 3.0f;
            yield return WaitWithDebugMessage("Moved builder forward");
        }
    }

    IEnumerator GenerateBuildings()
    {
        foreach (var location in m_PossibleLocationsOfBuildings)
        {
            var pos = location.pos;
            pos.y = m_HouseY;

            var prefab = m_HousePrefabs[Random.Range(0, m_HousePrefabs.Count)];

            var newBuilding = Instantiate(prefab).transform;
            newBuilding.position = pos;
            newBuilding.localScale *= m_HouseScale;
            newBuilding.LookAt(location.lookAt);
        }
        yield return WaitWithDebugMessage("Completed buildings");
    }

    IEnumerator WaitWithDebugMessage(string debugMessage)
    {
        if (m_Demo_ShowDebugLog)
        {
            Debug.Log(debugMessage);
            yield return new WaitForSeconds(m_Demo_WaitForSeconds);
        }
    }
}
