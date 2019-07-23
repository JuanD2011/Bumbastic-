using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectOnInput : MonoBehaviour
{
    public GameObject selectedObject;

    private Button selectedButton;

    private void Awake()
    {
        selectedButton = selectedObject.GetComponent<Button>(); 
    }

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(selectedObject);
        selectedButton.OnSelect(null);
    }
}
