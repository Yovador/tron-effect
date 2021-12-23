using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionPedestal : MonoBehaviour
{
    [SerializeField] float heigthObject = 0.5f;
    GameObject currentDisplayed;
    public void OnSelectedVehicle(GameObject obj)
    {
        Destroy(currentDisplayed);
        currentDisplayed = Instantiate(obj, transform.position, Quaternion.identity, transform.parent);
        GameManager.instance.characterSelect.SelectedCharacter = currentDisplayed;
        currentDisplayed.transform.position = transform.position + new Vector3(0, heigthObject, 0);
    }

    private void Update()
    {
        if(currentDisplayed != null)
        {
            currentDisplayed.transform.eulerAngles += new Vector3(0, 1, 0);
        }
    }
}
