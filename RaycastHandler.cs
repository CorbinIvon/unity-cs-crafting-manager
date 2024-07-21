using UnityEngine;

public class RaycastHandler : MonoBehaviour
{
    private Camera _mainCamera;

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button click
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                CraftingStation craftingStation = hit.collider.GetComponent<CraftingStation>();
                if (craftingStation != null)
                {
                    craftingStation.TestCraft();
                }
            }
        }
    }
}
