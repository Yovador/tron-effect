using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableCharacter : MonoBehaviour
{
    public GameObject myModel { get; set; } 
    Transform modelGameObject;
    void Start()
    {
        
        modelGameObject = transform.GetChild(0);
        Instantiate(myModel, modelGameObject);

    }

    private void Update()
    {
        modelGameObject.eulerAngles += new Vector3(0, 1, 0);
    }

}
