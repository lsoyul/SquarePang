using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardStatics : MonoBehaviour
{
    public static readonly int BOARD_WIDTH_MIN = 5;
    public static readonly int BOARD_WIDTH_MAX = 10;
    public static readonly int BOARD_HEIGHT_MIN = 5;
    public static readonly int BOARD_HEIGHT_MAX = 10;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
