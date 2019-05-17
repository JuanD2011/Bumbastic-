using UnityEngine;
using UnityEngine.UI;

public class Scroll : MonoBehaviour
{
    Scrollbar m_Scrollbar;
    float velocity = 0.7f;

    private void Start()
    {
        m_Scrollbar = GetComponent<Scrollbar>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.DownArrow))
        {
            m_Scrollbar.value -= velocity * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            m_Scrollbar.value += velocity * Time.deltaTime;
        }
    }
}
