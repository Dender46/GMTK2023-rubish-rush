using System.Collections.Generic;
using UnityEngine;

public class WindowStatus : MonoBehaviour
{
    [SerializeField] List<Material> m_ShatteredGlassMats;
    [SerializeField] List<AudioClip> m_Sounds;

    bool m_IsShattered = false;
    public void Shatter()
    {
        if (m_IsShattered)
        {
            return;
        }

        var randomMaterial = m_ShatteredGlassMats[Random.Range(0, m_ShatteredGlassMats.Count)];
        GetComponent<MeshRenderer>().material = randomMaterial;

        GameManager.ShowBonusText(transform.position);

        PLaySound();

        m_IsShattered = true;
    }
    void PLaySound()
    {
        var sound = m_Sounds[Random.Range(0, m_Sounds.Count)];
        GetComponent<AudioSource>().PlayOneShot(sound);
    }
}
