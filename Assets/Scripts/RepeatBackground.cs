using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatBackground : MonoBehaviour
{
    Bounds m_bounds;

    // Start is called before the first frame update
    void Start()
    {
        Renderer renderer = GetComponentInChildren<Renderer>();
        if (null == renderer)
        {
            Debug.LogError("RepeatBackground with no renderer");
            Destroy(this);
        }
        m_bounds = renderer.bounds;
    }

    // Update is called once per frame
    void Update()
    {
        {   // TODO add new tiles to fill to the right
            int r_mostImage = transform.childCount - 1;
            Transform r_mostChild = transform.GetChild(r_mostImage);

            Renderer ren_r = r_mostChild.GetComponent<Renderer>();
            float r_edgeW = ren_r.bounds.max.x;

            float r_edgeV = Camera.main.WorldToViewportPoint(new Vector3(r_edgeW, 0, 0)).x;

            if (r_edgeV < 1.0f)
            {
                // instantiate new copy
                GameObject copy = Instantiate(r_mostChild.gameObject);

                // position directly to the right of the curr right-most
                copy.transform.position = r_mostChild.transform.position + new Vector3(m_bounds.size.x, 0, 0)

                // make a sibling
                copy.transform.SetParent(transform);
            }
        }

        {   // TODO remove tiles on the left
            Transform l_mostChild = transform.GetChild(0);

            Renderer ren_l = l_mostChild.GetComponent<Renderer>();
            float l_edgeW = ren_l.bounds.max.x;

            float l_edgeV = Camera.main.WorldToViewportPoint(new Vector3(l_edgeW, 0, 0)).x;

            if (l_edgeV < 0.0f)
            {
                Destroy(l_mostChild.gameObject);
            }
        }
    }
}
