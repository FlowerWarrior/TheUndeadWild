using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightFollow : MonoBehaviour
{
	public Transform p_Transform;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3 (p_Transform.position.x, transform.position.y, p_Transform.position.z);
    }
}
