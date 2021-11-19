using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    [Header("Background Neons Settings")]
    [SerializeField] Material backgroundMaterial;
    [SerializeField] int dominionThreshold = 10;
    [SerializeField] float neonAnimationTime = 0.5f;
    [SerializeField, Range(1, 8)] int backgroundFreqToUse = 3;
    [Header("Beam Settings")]
    [SerializeField, Range(1, 8)] int beamFreqToUse = 2;
    GameObject beamObject;
    [SerializeField, Range(0, 360)] float maxBeamWidth = 360;
    [SerializeField, Range(0, 360)] float minBeamWidth = 0;
    bool areNeonUpdating = false;

    private void Awake()
    {
        backgroundMaterial.SetFloat("_PlayerRatio", 0);
        backgroundMaterial.SetFloat("_BassRatio", 0);
        beamObject = GameObject.FindGameObjectWithTag("BEAM");
        beamObject.transform.localScale = new Vector3(0, beamObject.transform.localScale.y, beamObject.transform.localScale.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (!areNeonUpdating)
        {
            StartCoroutine(UpdateBackgroundWinRatio());
        }
        UpdateBackgroundHeigth();
        UpdateBeam();
    }

    IEnumerator UpdateBackgroundWinRatio()
    {
        areNeonUpdating = true;
        Debug.Log("Update Background");
        float newRatio = 0;
        int computerDominion = dominionThreshold + GameManager.instance.computerScore - GameManager.instance.playerScore;
        int playerDominion = dominionThreshold + GameManager.instance.playerScore - GameManager.instance.computerScore;
        int totalPoint = computerDominion+playerDominion;
        Debug.Log("Total point : " + totalPoint);
        if(totalPoint != 0)
        {
            newRatio = (float)playerDominion / totalPoint;
            newRatio = (newRatio * 2) - 1;
            Debug.Log("newRatio : " + newRatio);
        }
        float currentRatio = backgroundMaterial.GetFloat("_PlayerRatio");
        float speed = Mathf.Abs(newRatio - currentRatio) / neonAnimationTime * Time.deltaTime;
        Debug.Log("Speed : " + speed);
        if (currentRatio > newRatio)
        {
            while (currentRatio > newRatio)
            {
                Debug.Log("Sup ! " + currentRatio + " / " + newRatio);
                currentRatio -= Mathf.Abs(speed);
                backgroundMaterial.SetFloat("_PlayerRatio", currentRatio);

                yield return new WaitForEndOfFrame();
            }

            backgroundMaterial.SetFloat("_PlayerRatio", newRatio);
            areNeonUpdating = false;
            yield break;
        }
        else
        {
            while (currentRatio < newRatio)
            {
                Debug.Log("Inf ! " + currentRatio + " / " + newRatio);
                currentRatio += Mathf.Abs(speed);
                backgroundMaterial.SetFloat("_PlayerRatio", currentRatio);
                yield return new WaitForEndOfFrame();
            }

            backgroundMaterial.SetFloat("_PlayerRatio", newRatio);
            areNeonUpdating = false;
            yield break;
        }

    }

    void UpdateBackgroundHeigth()
    {
        float newValue = GameManager.instance.audioSync.DisplayableSpectrum[ backgroundFreqToUse - 1];
        //newValue = YovaUtilities.GetSum(GameManager.instance.audioSync.DisplayableSpectrum) / GameManager.instance.audioSync.DisplayableSpectrum.Length;
        backgroundMaterial.SetFloat("_BassRatio", newValue );
    }

    void UpdateBeam()
    {
        float newValue = GameManager.instance.audioSync.DisplayableSpectrum[beamFreqToUse - 1] * maxBeamWidth;


        if(newValue < minBeamWidth)
        {
            newValue = 0;
            beamObject.SetActive(false);
        }
        else
        {
            beamObject.SetActive(true);
        }

        beamObject.transform.localScale = new Vector3(newValue, beamObject.transform.localScale.y, beamObject.transform.localScale.z);


    }
}
