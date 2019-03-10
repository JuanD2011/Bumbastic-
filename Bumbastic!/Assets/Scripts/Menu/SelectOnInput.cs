using UnityEngine;
using UnityEngine.EventSystems;

public class SelectOnInput : MonoBehaviour
{
    public EventSystem eventSystem;
    public GameObject selectedObject;

    private bool buttonSelected;

    [SerializeField] InputManager inputManager;

    private void Start()
    {
        inputManager.UI.Move.performed += context => Move();
    }

    private void Move()
    {
        if (buttonSelected == false)
        {
            eventSystem.SetSelectedGameObject(selectedObject);
            buttonSelected = true;
        }
    }

    void Update()
    {
        if (Input.GetAxisRaw("Vertical") != 0)
        {
            Move();
        }
    }

    private void OnDisable()
    {
        buttonSelected = false;
    }
}
