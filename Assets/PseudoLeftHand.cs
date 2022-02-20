using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PseudoLeftHand : MonoBehaviour
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
        this.pseudoHandTransform.rotation = _handGeneral.leftHandQuaternion * Quaternion.Euler(60f, -90f, 0f);

        Vector3 handMove = this._handGeneral.leftHandCenterPosition - this._handGeneral.leftHandCenterPositionOrigin;
        this.pseudoHandTransform.position = _handGeneral.leftHandCenterPositionOrigin + handMove * pseudoRange;

    }
}
