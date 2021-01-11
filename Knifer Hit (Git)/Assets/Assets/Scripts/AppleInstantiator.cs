using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleInstantiator : MonoBehaviour
{
    [SerializeField]
    private GameObject apple;

    public GameObject applePresent;

    private WheelRotator wheelRotator;

    private int generateApple;

    // Start is called before the first frame update
    void Start()
    {
        generateApple = Random.Range(0, 5);
        if (generateApple == 0)
        {
            applePresent = Instantiate(apple);
        }
        wheelRotator = GetComponent<WheelRotator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (apple != null && applePresent!=null)
        {
            applePresent.transform.RotateAround(new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 1f), wheelRotator.speed * Time.deltaTime);
        }
    }
}