using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelect : MonoBehaviour
{

    [SerializeField] private List<GameObject> playabaleCharacters = new List<GameObject>(3);
    [SerializeField] private GameObject selectableCharacterPrefab;
    [SerializeField, Range(0.1f, 2)] private float spaceBetweenChar = 2;
    private GameObject selectedCharacter;
    public GameObject SelectedCharacter { 
        get { return selectedCharacter; }
        set 
        {
            for (int i = 0; i < playabaleCharacters.Count; i++)
            {
                GameObject pChar = playabaleCharacters[i];
                if(pChar.tag == value.tag)
                {
                    Debug.Log("Setting char to " + value.name);
                    selectedCharacter = pChar;
                }
            }
            Debug.Log("No correspondance, ignoring change to selected character " + value.name);
        } 
    }
    private GameObject charSelectBase;

    private void Start()
    {
        SelectedCharacter = playabaleCharacters[0];
    }

    public void DisplaySelectableCharacter()
    {
        charSelectBase = GameObject.FindGameObjectWithTag("CharSelectBase");
        for (int i = 0; i < playabaleCharacters.Count; i++)
        {
            GameObject charac = playabaleCharacters[i];
            Debug.Log("DisplaySelectableCharacter " + charac.name + " / " + selectableCharacterPrefab.name + " / ");
            GameObject selectableCharac = Instantiate(selectableCharacterPrefab, charSelectBase.transform);
            Debug.Log(selectableCharac);
            selectableCharac.transform.localPosition = charSelectBase.transform.forward * i * spaceBetweenChar;
            selectableCharac.GetComponent<SelectableCharacter>().myModel = charac;
        }
    }



}
