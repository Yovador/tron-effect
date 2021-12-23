using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableCharacter : MonoBehaviour
{
    public GameObject myModel { get; set; } 
    Transform modelGameObject;
    SelectionPedestal selectionPedestal;
    void Start()
    {
        selectionPedestal = GameObject.FindGameObjectWithTag("SelectionPedestal").GetComponent<SelectionPedestal>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject button = YovaUtilities.FindChildrenWithTag(gameObject, "Button")[0];
        button.transform.LookAt(player.transform);

        modelGameObject = transform.GetChild(0);
        Instantiate(myModel, modelGameObject);

    }

    private void Update()
    {
        modelGameObject.eulerAngles += new Vector3(0, 1, 0);
    }

    public void OnClick()
    {
        selectionPedestal.OnSelectedVehicle(myModel);
    }

}
