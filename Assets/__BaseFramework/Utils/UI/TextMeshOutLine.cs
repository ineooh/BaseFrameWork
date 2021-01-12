using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextMeshOutLine : MonoBehaviour
{
    public Color[] Colors;
    public Color ColorForMain;
    public TextMesh[] textmeshs;
    public float gapz = 0.01f;
    public float gap = 0.01f;

    public void SetMain()
    {
        textmeshs[0].color = ColorForMain;
    }
    public void Set(string value)
    {
        for( int i = 0; i < textmeshs.Length; i++ )
        {
            textmeshs[i].text = value;
        }
    }

    private void OnDrawGizmosSelected()
    {
        textmeshs = transform.GetComponentsInChildren<TextMesh>();

        for (int i = 0; i < textmeshs.Length; i++)
        {
            if (i == 0)
            {
                textmeshs[i].color = Colors[0];
                textmeshs[i].transform.localPosition = new Vector3(0, 0, 0);
            }

            else
                textmeshs[i].color = Colors[1];

            if (i == 1)
                textmeshs[i].transform.localPosition  = new Vector3(gap,0, gapz);
            if (i == 2)
                textmeshs[i].transform.localPosition = new Vector3(-gap, 0, gapz);
            if (i == 3)
                textmeshs[i].transform.localPosition = new Vector3(0, gap, gapz);
            if (i == 4)
                textmeshs[i].transform.localPosition = new Vector3(0, -gap, gapz);

        }

    }
}
