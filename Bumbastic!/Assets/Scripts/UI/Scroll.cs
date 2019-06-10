using System;
using UnityEngine;
using UnityEngine.UI;

public class Scroll : MonoBehaviour
{
    Scrollbar m_Scrollbar;
    [SerializeField] float velocity = 0.7f;

    private void Start()
    {
        m_Scrollbar = GetComponent<Scrollbar>();

        PlayerMenu.onScrolling += ScrollMovement;
    }

    private void ScrollMovement(float _context)
    {
        print(_context);
        if (_context < -0.2f || _context > 0.2f)
        {
            m_Scrollbar.value += velocity * Time.deltaTime * _context; 
        }
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
