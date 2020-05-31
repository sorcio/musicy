using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickManager : MonoBehaviour
{

    public Note noteSoundPrefab;
    public float interval;

    private Note noteSound;
    ParticleSystem sparks;

    // Start is called before the first frame update
    void Start()
    {
        var sparklinessPrefab = Resources.Load<GameObject>("Sparkliness");
        var sparkliness = Instantiate(sparklinessPrefab);
        sparks = sparkliness.GetComponent<ParticleSystem>();

        this.noteSound = Instantiate(noteSoundPrefab);
        int octave = ExtractOctaveFromName(this.transform.parent.name);
        this.noteSound.Pitch = Mathf.Pow(2.0f, (octave - 2) + (interval / 12));
    }

    static int ExtractOctaveFromName(string name)
    {
        string[] parts = name.Split(new char[] {' '}, 2);
        if (parts[0] == "Octave")
        {
            int result;
            if (int.TryParse(parts[1], out result))
            {
                return result;
            }
        }
        return 2;  // well this is the default octave for some reason
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Transform objectHit = hit.transform;
                if (objectHit == transform)
                {
                    this.MouseHit(hit);
                    return;
                }
            }
        }
        this.MouseNotHit();
    }

    void MouseHit(RaycastHit hit)
    {
        if (this.noteSound.Press())
        {
            this.sparks.Play();
        }
        this.sparks.transform.position = hit.point;
    }

    void MouseNotHit()
    {
        this.noteSound.Release();
        this.sparks.Stop();
    }
}
