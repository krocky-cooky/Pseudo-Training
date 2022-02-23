using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightManipulation : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] 
    private GameObject Weight;
    private GameObject Camera;

    private HandGeneral _handGeneral;
    void Start()
    {
        this.Camera = GameObject.FindGameObjectsWithTag("MainCamera")[0];
        this._handGeneral = this.Camera.GetComponent<HandGeneral>();
    }

    // Update is called once per frame
    void Update()
    {
        this.GetComponent<MeshRenderer>().enabled = this._handGeneral.isTraining;
        if(this._handGeneral.isTraining)
        {
            this.transform.position = this._handGeneral.handCenterPosition;
        }
        Vector3 aim = 
    }
}
