using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArrowBehaviour : MonoBehaviour
{
    private TargetJoint2D targetJoint2D;
    private Rigidbody2D arrow;

    public WheelRotator wheelRotator;
    public AppleInstantiator appleInstantiator;
    public UIManager UIManager;
    private SaveData saveData;
    private GameObject savedDataObject;

    private bool hitTarget;

    [SerializeField]
    private GameObject arrowGameObject;

    [SerializeField]
    private GameObject arrowSpawn;

    public int numberOfArrowsToWin=5;

    [SerializeField]
    private GameObject WheelBroken;

    private void Start()
    {
        UIManager.GameOver.gameObject.SetActive(false);
        savedDataObject = GameObject.FindGameObjectWithTag("ScoreSaver");
        saveData = savedDataObject.GetComponent<SaveData>();
        targetJoint2D = GetComponent<TargetJoint2D>();
        arrow = GetComponent<Rigidbody2D>();
        UIManager.numberOfArrowsToWin = numberOfArrowsToWin;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wheel"))
        {
            Handheld.Vibrate();
            arrow.velocity = new Vector2(0, 0);
            arrow.isKinematic = true;
            hitTarget = true;
            numberOfArrowsToWin--;
            if (gameObject.CompareTag("Arrow"))
            {
                UIManager.numberOfArrowsToWin = numberOfArrowsToWin;
            }
        }

        if(collision.CompareTag("Arrow") || collision.CompareTag("ArrowStatic"))
        {
            saveData.gameOver = true;
            Handheld.Vibrate();
            saveData.maxLevel = 1;
            DontDestroyOnLoad(saveData.gameObject);
            Destroy(targetJoint2D);
            Destroy(gameObject.GetComponent<BoxCollider2D>());
            Destroy(gameObject.GetComponent<CapsuleCollider2D>());
            Destroy(arrowSpawn);
            arrow.AddForce(new Vector2(Random.Range(-200, 200), -800));
            StartCoroutine(GameOver());
        }

        if (collision.CompareTag("Apple"))
        {
            Destroy(collision.gameObject);
            saveData.ReadApple();
        }
    }

    private void Update()
    {
        if (hitTarget == true)
        {
            gameObject.transform.RotateAround(new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 1f), wheelRotator.speed * Time.deltaTime);
        }

        if (Input.touchCount == 1 && Input.touches[0].phase == TouchPhase.Began /*|| Input.GetKeyUp(KeyCode.Space) - USE TO TEST ON DESKTOP*/ && hitTarget == false && Time.timeScale==1)
        {
            arrow.isKinematic = false;
            if (targetJoint2D != null)
            {
                targetJoint2D.enabled = true;
            }
            if (arrowSpawn != null)
            {
                StartCoroutine(CreateNewArrow());
            }
        }

        if(numberOfArrowsToWin==0)
        {

            //Arrows
            GameObject[] Arrows = GameObject.FindGameObjectsWithTag("Arrow");
            foreach (GameObject arrow in Arrows)
            {
                arrow.GetComponent<Rigidbody2D>().isKinematic = false;
                arrow.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-100, 100), Random.Range(100, 500)));

                BoxCollider2D[] arrowColliders = arrow.GetComponents<BoxCollider2D>();
                foreach(var collider in arrowColliders)
                {
                    collider.GetComponent<BoxCollider2D>().enabled = false;
                }
                Destroy(arrow.GetComponent<TargetJoint2D>());
            }

            GameObject[] ArrowsStatic = GameObject.FindGameObjectsWithTag("ArrowStatic");
            foreach (GameObject arrow in ArrowsStatic)
            {
                arrow.GetComponent<Rigidbody2D>().isKinematic = false;
                arrow.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-100, 100), Random.Range(100, 500)));

                BoxCollider2D[] arrowColliders = arrow.GetComponents<BoxCollider2D>();
                foreach (var collider in arrowColliders)
                {
                    collider.GetComponent<BoxCollider2D>().enabled = false;
                }
                Destroy(arrow.GetComponent<TargetJoint2D>());
            }

            //Apple
            if (appleInstantiator.applePresent != null)
            {
                Rigidbody2D appleMass = GameObject.FindGameObjectWithTag("Apple").GetComponent<Rigidbody2D>();
                appleMass.isKinematic = false;
            }


            //Broken Wheel
            GameObject Broken = Instantiate(WheelBroken);
            Rigidbody2D[] WheelParts = Broken.GetComponentsInChildren<Rigidbody2D>();
            for(int i=0;i<WheelParts.Length;i++)
            {
                WheelParts[i].transform.rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(-10, 10)));
                WheelParts[i].AddForce(new Vector2(Random.Range(-100, 100), Random.Range(100, 500)));
            }

            Handheld.Vibrate();
            if (wheelRotator.gameObject != null)
            {
                Destroy(wheelRotator.gameObject);
            }
            StartCoroutine(Restarter());
            numberOfArrowsToWin = 1;
        }
    }

    IEnumerator CreateNewArrow()
    {
        yield return new WaitForSeconds(0.5f);
        UIManager.firstShot = true;
        if (wheelRotator != null && arrowSpawn != null)
        {
            Instantiate(arrow, arrowSpawn.transform.position, Quaternion.Euler(0, 0, 90));
        }
    }

    IEnumerator Restarter()
    {
        yield return new WaitForSeconds(3f);
        saveData.highScoreNew += 1;
        saveData.maxLevel += 1;
        saveData.SaveScore();
        DontDestroyOnLoad(saveData.gameObject);
        UIManager.LevelConmpleteMenu.SetActive(true);
    }

    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(2f);
        saveData.SaveScore();
        UIManager.GameOver.gameObject.SetActive(true);
        saveData.highScoreNew = 0;
        StartCoroutine(RestartGame());
    }

    IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("MainScene");
        UIManager.canvasMenu.gameObject.SetActive(true);
    }
}