using UnityEngine;

public class Bummie : MonoBehaviour
{
    [SerializeField] Transform catapult;

    public Transform Catapult { get => catapult; set => catapult = value; }
}
