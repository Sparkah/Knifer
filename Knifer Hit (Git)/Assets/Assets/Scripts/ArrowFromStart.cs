using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowFromStart : MonoBehaviour
{
    public GameObject[] arrowsFromStart;

    public int howManyArrows;
    // Start is called before the first frame update
    void Start()
    {
        howManyArrows = Random.Range(0, 3);
        for(int i = 0; i<howManyArrows;i++)
        {
            Destroy(arrowsFromStart[i]);
        }
    }
}
