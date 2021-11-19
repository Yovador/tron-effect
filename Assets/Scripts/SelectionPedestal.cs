using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionPedestal : MonoBehaviour
{
    [SerializeField] float heigthObject = 0.5f;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("SelectionPedestal " + other.tag + " / " + other.name);
        if (other.tag == "SelectableCharacter")
        {
            GameObject newSelected = other.gameObject.transform.GetChild(0).GetChild(0).gameObject;
            Debug.Log("SelectionPedestal in if " + newSelected.name);
            GameManager.instance.characterSelect.SelectedCharacter = newSelected;
            other.transform.position = transform.position + new Vector3(0, heigthObject, 0);
        }
    }

}
