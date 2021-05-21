using UnityEngine.EventSystems;
using UnityEngine;

public class SwitchObjectBtn : MonoBehaviour
{
    public GameObject objectToToggle_1;
    public GameObject objectToToggle_2;
    public bool resetSelectionAfterClick;

    void Update()
    {
        if (objectToToggle_2.activeSelf && Input.GetButtonDown(GameConstants.k_ButtonNameCancel))
        {
            objectToToggle_2.SetActive(false);
            objectToToggle_1.SetActive(true);
        }
    }

    public void SetGameObjectActive(bool active)
    {
        objectToToggle_1.SetActive(false);
        objectToToggle_2.SetActive(active);

        if (resetSelectionAfterClick)
            EventSystem.current.SetSelectedGameObject(null);
    }
}
