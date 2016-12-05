using UnityEngine;
using System.Collections;

public class ParentWebCamFilter : MonoBehaviour {

    public RectTransform ActivateCheck;
    // Use this for initialization
    void OnEnable()
    {
        ActivateCheck.gameObject.SetActive(ParentsFilter.FailCount > 14);
    }
}
