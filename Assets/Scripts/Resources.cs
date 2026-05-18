using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
[System.Serializable]
public class Resources : MonoBehaviour
{
    public float wood;
    public float stone;
    public float food;
    public float workers;
    public float planks;
    public float clay;
    public float bricks;
    public float[] resources = new float[7];


    public void Start()
    {
        if (workers == 0)
            workers = 5;

    }
    public TextMeshProUGUI resourcesText;
    private void Update()
    {
        //resources = (wood, stone, food);
    }
    void FixedUpdate()
    {
        resourcesText.text = "Wood: " + wood.ToString("F0") + " | Stone: "
            + stone.ToString("F0") + " | Food: " + food.ToString("F0") +
            " | Workers: " + workers.ToString("F0") + " Planks | " + planks.ToString("F0")
            + " Clay | " + clay.ToString("F0") + " Bricks | " + bricks.ToString("F0");
    }
}
