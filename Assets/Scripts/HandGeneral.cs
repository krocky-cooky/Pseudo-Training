using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandGeneral : MonoBehaviour
{

    private int rightHandGesture,leftHandGesture;
    public Vector3 handCenterPosition;
    public Vector3 handCenterPositionOrigin;
    public GameObject rightHandPrefab,leftHandPrefab;
    public bool isTraining,trainingChangeWaiting;
    [SerializeField] float trainingStartWaitTime = 2f,trainingStopWaitTime = 2f,pseudoRange = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
        this.rightHandGesture = -1;
        this.leftHandGesture = -1;
        this.isTraining = false;
        this.trainingChangeWaiting = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        
        rightHandGesture = rightHandPrefab.GetComponent<HandSignDetector>().sign;
        leftHandGesture = leftHandPrefab.GetComponent<HandSignDetector>().sign;
        
        Transform rightHandCenterTransform = rightHandPrefab.GetComponent<HandBoneDot>().handCenter;
        Transform leftHandCenterTransform =  leftHandPrefab.GetComponent<HandBoneDot>().handCenter;
        
        this.handCenterPosition = (rightHandCenterTransform.position + leftHandCenterTransform.position)/2;
        //Debug.Log(rightHandGesture);
        //Debug.Log(new Vector2(rightHandGesture,leftHandGesture));
        
        
        if(rightHandGesture == 0 && leftHandGesture == 0 && !isTraining && !trainingChangeWaiting)
            StartCoroutine(TrainingStartWait(trainingStartWaitTime));
        else if(rightHandGesture == 2 && leftHandGesture == 2 && isTraining && !trainingChangeWaiting)
            StartCoroutine(TrainingStopWait(trainingStopWaitTime));

        Debug.Log(isTraining);
        
    }

    IEnumerator TrainingStartWait(float waitime)
    {
        float t = Time.time;
        trainingChangeWaiting = true;
        while (Time.time < t + waitime)
        {
            
            if(!(rightHandGesture == 0 && leftHandGesture == 0))
            {
                this.isTraining = false;
                trainingChangeWaiting = false;
                yield break;
            }
            //Debug.Log("Starting");
            yield return null;
            
        }
        this.isTraining = true;
        handCenterPositionOrigin = handCenterPosition;
        trainingChangeWaiting = false;
        this.GetComponent<OVRCameraRig>().TrainingStart(pseudoRange);
        
        Debug.Log("Training start");

    }

    IEnumerator TrainingStopWait(float waitime)
    {
        float t = Time.time;
        trainingChangeWaiting = true;
        while (Time.time < t + waitime)
        {
            if(!(rightHandGesture == 2 && leftHandGesture == 2))
            {
                this.isTraining = true;
                trainingChangeWaiting = false;
                yield break;
            }
            yield return null;
        }
        this.isTraining = false;
        trainingChangeWaiting = false;
        this.GetComponent<OVRCameraRig>().TrainingEnd();
        Debug.Log("Training stop");

    }
}
