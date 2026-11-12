using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager _GM;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    
    public void GameOver()
    {
        Debug.Log("Game Over!");
    }

    // Update is called once per frame
    void Update()
    {
        if (_GM == null)
        {
            _GM = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
