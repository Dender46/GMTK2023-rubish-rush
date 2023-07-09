using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum PointsType
    {
        Regular, Window
    }

    [Header("Points")]
    [SerializeField] int m_PointsRegular = 50;
    [SerializeField] int m_PointsWindow = 125;

    int m_TotalPoints = 0;
    TMP_Text m_PointsText;

    static GameManager instance;

    void Start()
    {
        instance = this;
        m_PointsText = GameObject.Find("Points").GetComponent<TMP_Text>();
    }

    static public void AddPoints(PointsType type)
    {
        int newAdditionalPoints= 0;
        switch (type)
        {
            case PointsType.Regular:
                newAdditionalPoints= instance.m_PointsRegular;
                break;
            case PointsType.Window:
                newAdditionalPoints= instance.m_PointsWindow;
                break;
        }

        instance.m_TotalPoints += newAdditionalPoints;
        instance.m_PointsText.text = "Score: " + instance.m_TotalPoints;
    }
}
