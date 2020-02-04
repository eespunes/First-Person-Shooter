using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyCounter : MonoBehaviour
{
    private GameController mGameController;

    public Transform mKey;

    public bool end;
    // Start is called before the first frame update
    void Start()
    {
        mGameController = GameObject.Find("GameController").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.childCount == 0)
        {
            if (end)
                SceneManager.LoadScene("Win");
            else
            {
                mGameController.GetKey(mKey);
                Destroy(gameObject);
            }
        }
    }
}
