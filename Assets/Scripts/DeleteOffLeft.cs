using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteOffLeft : MonoBehaviour
{
    Vector3 m_rightEdge;

    // Start is called before the first frame update
    void Start()
    {
        // Get the right-most boundary of this object's graphics
        Bounds bound = new Bounds(transform.position, Vector3.zero);
        Renderer[] renders = GetComponentsInChildren<Renderer>();
        foreach (Renderer render in renders)
        {
            bound.Encapsulate(render.bounds);
        }
        m_rightEdge = bound.max - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // where is the right-most edge of the object in screen space now?
        Vector3 pos = transform.TransformPoint(m_rightEdge);
        pos = Camera.main.WorldToViewportPoint(pos);
        if (pos.x < 0.0f)
        {   // it's off the left edge of the screen
            Destroy(gameObject);
        }
    }
}
