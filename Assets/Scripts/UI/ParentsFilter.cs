using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class ParentsFilter : MonoBehaviour {

    public Text Sum;
    public List<Text> Results = new List<Text>();
    public RectTransform WrongPanel, QuestionPanel;

    public static int FailCount
    {
        get { return _failCount; }
    }

	// Use this for initialization
	void OnEnable() {
        _failCount = 0;
        ResetValues();
	}

    public void clickResult(Text result)
    {
        if (Convert.ToInt32(result.text) == _sum)
        {
            SceneLoader.Instance.GoToParentWebView();
        }
        else
        {
            _failCount++;
            ResetValues();
            WrongPanel.gameObject.SetActive(true);
            QuestionPanel.gameObject.SetActive(false);
        }
    }

    private void ResetValues()
    {
        int valA, valB;
        valA = UnityEngine.Random.Range(1, 50);
        valB = UnityEngine.Random.Range(1, 50);
        _sum = valA + valB;
        Sum.text = valA + " + " + valB;
        int selected = UnityEngine.Random.Range(1, Results.Count);
        for(int i=0; i<Results.Count; ++i)
        {
            if (i == selected)
            {
                Results[i].text = _sum.ToString();
            }
            else
            {
                int auxValue = UnityEngine.Random.Range(1, 100);
                if (auxValue == _sum) auxValue = (auxValue + 1) % 100;
                Results[i].text = auxValue.ToString();
            }
        }
    }

    private int _sum;
    private static int _failCount;
}
