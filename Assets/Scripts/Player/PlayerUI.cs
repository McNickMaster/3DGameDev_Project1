using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    public GameObject waypointArrow;
    public Transform currentMarker;

    
    float angle = 0;

    void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PointArrowToMarker();

        ScaleArrow();
        
    }

    void PointArrowToMarker()
    {
        waypointArrow.transform.rotation = Quaternion.Euler(80, angle, 0);
        Vector3 dist = currentMarker.position - transform.position;
        angle = Mathf.Atan2(dist.x, dist.z) * Mathf.Rad2Deg;
    }

//so bad
    void ScaleArrow()
    {
        if(Player.instance.crouched)
        {
            waypointArrow.transform.localScale = new Vector3(0.15f, 0.15f, 0.23f);
        } else 
        {
            waypointArrow.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
        }
    }


}
