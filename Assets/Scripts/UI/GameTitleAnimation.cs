using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameTitleAnimation : MonoBehaviour
{
    public List<TextMeshProUGUI> lettersList;
    public float floatAmplitude;
    public float floatFrequency;

    public float defaultYPosition = 200f;

    protected void FloatUpDown()
    {
        // Per cada lletra en la llista, donem-li un moviment independent
        for (int i = 0; i < lettersList.Count; i++)
        {
            float letterOffset = Mathf.Sin((Time.time + i * 1f) * floatFrequency) * floatAmplitude;
            float newY = defaultYPosition + letterOffset;
            lettersList[i].transform.position = new Vector2(lettersList[i].transform.position.x, newY);
        }
    }

    void Update()
    {
        FloatUpDown();
    }
}