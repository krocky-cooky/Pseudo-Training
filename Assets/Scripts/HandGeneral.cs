using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandGeneral : MonoBehaviour
{

    private int rightHandGesture, leftHandGesture;
    public Vector3 handCenterPosition;
    public Vector3 rightHandCenterPosition;
    public Vector3 leftHandCenterPosition;
    public Vector3 handCenterPositionOrigin;
    public Vector3 rightHandCenterPositionOrigin;
    public Vector3 leftHandCenterPositionOrigin;

    //手の姿勢を表すクォータニオン
    //手を水平にし、かつ手のひらを上に向けた時の姿勢を基準とする
    public Quaternion rightHandQuaternion;
    public Quaternion leftHandQuaternion;

    public GameObject rightHandPrefab, leftHandPrefab;
    public bool isTraining, trainingChangeWaiting;
    [SerializeField] float trainingStartWaitTime = 2f, trainingStopWaitTime = 2f;

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
        Transform leftHandCenterTransform = leftHandPrefab.GetComponent<HandBoneDot>().handCenter;

        this.handCenterPosition = (rightHandCenterTransform.position + leftHandCenterTransform.position) / 2;
        this.rightHandCenterPosition = rightHandCenterTransform.position;
        this.leftHandCenterPosition = leftHandCenterTransform.position;
        //Debug.Log(rightHandGesture);
        Debug.Log(new Vector2(rightHandGesture, leftHandGesture));


        if (rightHandGesture == 0 && leftHandGesture == 0 && !isTraining && !trainingChangeWaiting)
            StartCoroutine(TrainingStartWait(trainingStartWaitTime));
        else if (rightHandGesture == 2 && leftHandGesture == 2 && isTraining && !trainingChangeWaiting)
            StartCoroutine(TrainingStopWait(trainingStopWaitTime));


        //手のクォータニオンを取得
        //手の中指の付け根の骨(Middle1)のクォータニオンを、手全体のクォータニオンとする
        rightHandQuaternion = rightHandPrefab.GetComponent<OVRSkeleton>().Bones[9].Transform.rotation;
        leftHandQuaternion = leftHandPrefab.GetComponent<OVRSkeleton>().Bones[9].Transform.rotation;
    }

    IEnumerator TrainingStartWait(float waitime)
    {
        float t = Time.time;
        trainingChangeWaiting = true;
        while (Time.time < t + waitime)
        {

            if (!(rightHandGesture == 0 && leftHandGesture == 0))
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
        rightHandCenterPositionOrigin = rightHandCenterPosition;
        leftHandCenterPositionOrigin = leftHandCenterPosition;
        trainingChangeWaiting = false;
        Debug.Log("Training start");

    }

    IEnumerator TrainingStopWait(float waitime)
    {
        float t = Time.time;
        trainingChangeWaiting = true;
        while (Time.time < t + waitime)
        {
            if (!(rightHandGesture == 2 && leftHandGesture == 2))
            {
                this.isTraining = true;
                trainingChangeWaiting = false;
                yield break;
            }
            yield return null;
        }
        this.isTraining = false;
        trainingChangeWaiting = false;
        Debug.Log("Training stop");

    }
}
