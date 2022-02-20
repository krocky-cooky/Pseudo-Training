using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PseudoRightHand : MonoBehaviour
{

    public Transform handCenter;
    public GameObject ovrCameraRig;
    public GameObject OVRHandPrefab;
    public float pseudoRange;

    private Transform pseudoHandTransform;
    private MeshRenderer pseudoHandRenderer;
    private HandGeneral _handGeneral;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Renderer>().material.color = Color.blue;
        this.pseudoHandRenderer = this.GetComponent<MeshRenderer>();
        this.pseudoHandRenderer.enabled = false;
        this.pseudoHandTransform = this.transform;
        this._handGeneral = ovrCameraRig.gameObject.GetComponent<HandGeneral>();
    }

    // Update is called once per frame
    void Update()
    {
        bool isTraining = this._handGeneral.isTraining;
        if (isTraining)
        {
            this.pseudoHandRenderer.enabled = true;
            OVRHandPrefab.SetActive(false);
        }
        else
        {
            this.pseudoHandRenderer.enabled = false;
            OVRHandPrefab.SetActive(true);
        }
        //手を回転させる
        this.pseudoHandTransform.rotation = _handGeneral.rightHandQuaternion * Quaternion.Euler(-40f, 90f, 180f);

        Vector3 handMove = this._handGeneral.rightHandCenterPosition - this._handGeneral.rightHandCenterPositionOrigin;
        this.pseudoHandTransform.position = _handGeneral.rightHandCenterPositionOrigin + handMove * pseudoRange;

    }
}