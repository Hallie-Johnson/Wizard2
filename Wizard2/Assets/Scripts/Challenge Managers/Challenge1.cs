using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Challenge1 : MonoBehaviour
{
    #region Section 1
    public GameObject s1_targetObject;
    public GameObject s1_cube1;
    public GameObject s1_cube2;
    private float s1_targetPos = -4f;
    #endregion

    #region Section 2
    public GameObject s2_targetObject;
    public GameObject s2_cube1;
    public GameObject s2_cube2;
    private float s2_targetPos_1 = 0.07f;


    public GameObject s2_targetObject_1;
    public GameObject s2_targetObject_2;
    public GameObject s2_targetObject_3;
    public GameObject s2_targetObject_4;

    public GameObject s2_door1;
    public GameObject s2_door2;
    public GameObject s2_door3;
    public GameObject s2_door4;
    private float s2_targetPos_2 = 3f;

    public GameObject s2_cube3;
    public GameObject s2_cube4;
    #endregion

    #region Section 3
    public GameObject s3_targetObject;

    public GameObject s3_targetObject_2;
    public GameObject s3_targetObject_3;

    public GameObject s3_cube1;
    public GameObject s3_cube2;

    public GameObject s3_door1;
    public GameObject s3_door2;

    public GameObject s3_floor1;
    public GameObject s3_floor2;
    public GameObject s3_floor3;

    private float s3_targetPos_1 = 106f;
    private float s3_targetPos_2 = 7f;

    private float s3_targetPos_3 = -100f;
    private float s3_targetPos_4 = 100f;

    bool rise_platform = false;
    #endregion

    #region Section 4

    #endregion

    #region Section 5

    #endregion

    #region Section 6

    #endregion


    private float moveSpeed = 3f;

    void Start()
    {
        
    }

    void Update()
    {
        #region Section 1
        // Check if targetObject is assigned and has the required script
        if (s1_targetObject != null && s1_targetObject.GetComponent<ManagerRictusempra>() != null && s1_cube1 != null && s1_cube2 != null)
        {
            // Access the isStunned variable from TargetScript on targetObject
            bool isStunned = s1_targetObject.GetComponent<ManagerRictusempra>().isStunned;

            // Move the cube if isStunned is true and the x position is greater than the target
            if (isStunned && s1_cube1.transform.position.x > s1_targetPos && s1_cube2.transform.position.x > s1_targetPos)
            {
                // Move the cube gradually by decreasing the x position
                s1_cube1.transform.position = new Vector3(
                    Mathf.MoveTowards(s1_cube1.transform.position.x, s1_targetPos, moveSpeed * Time.deltaTime),
                    s1_cube1.transform.position.y,
                    s1_cube1.transform.position.z
                );

                // Move the cube gradually by decreasing the x position
                s1_cube2.transform.position = new Vector3(
                    Mathf.MoveTowards(s1_cube2.transform.position.x, s1_targetPos, moveSpeed * Time.deltaTime),
                    s1_cube2.transform.position.y,
                    s1_cube2.transform.position.z
                );
            }
        }
        #endregion

        #region Section 2
        if (s2_targetObject != null && s2_targetObject.transform.position.z > 35 && s2_targetObject.transform.position.x > -10)
        {
            // Move the cube if isStunned is true and the x position is greater than the target
            if (s2_cube1.transform.position.x < s2_targetPos_1 && s2_cube2.transform.position.x < s2_targetPos_1)
            {
                // Move the cube gradually by decreasing the x position
                s2_cube1.transform.position = new Vector3(
                    Mathf.MoveTowards(s2_cube1.transform.position.x, s2_targetPos_1, moveSpeed * Time.deltaTime),
                    s2_cube1.transform.position.y,
                    s2_cube1.transform.position.z
                );

                // Move the cube gradually by decreasing the x position
                s2_cube2.transform.position = new Vector3(
                    Mathf.MoveTowards(s2_cube2.transform.position.x, s2_targetPos_1, moveSpeed * Time.deltaTime),
                    s2_cube2.transform.position.y,
                    s2_cube2.transform.position.z
                );
            }
        }

        if (s2_targetObject_1 != null && s2_targetObject_1.GetComponent<ManagerRictusempra>() != null && s2_door1 != null)
        {
            // Access the isStunned variable from TargetScript on s2_targetObject_1
            bool isStunned = s2_targetObject_1.GetComponent<ManagerRictusempra>().isStunned;

            // Move the door if isStunned is true and the y position is greater than the target
            if (isStunned && s2_door1.transform.position.y > s2_targetPos_2)
            {
                // Move the door gradually by decreasing the y position
                s2_door1.transform.position = new Vector3(
                    s2_door1.transform.position.x,
                    Mathf.MoveTowards(s2_door1.transform.position.y, s2_targetPos_2, moveSpeed * Time.deltaTime),
                    s2_door1.transform.position.z
                );
            }
        }

        if (s2_targetObject_2 != null && s2_targetObject_2.GetComponent<ManagerRictusempra>() != null && s2_door2 != null)
        {
            // Access the isStunned variable from TargetScript on s2_targetObject_1
            bool isStunned = s2_targetObject_2.GetComponent<ManagerRictusempra>().isStunned;

            // Move the door if isStunned is true and the y position is greater than the target
            if (isStunned && s2_door2.transform.position.y > s2_targetPos_2)
            {
                // Move the door gradually by decreasing the y position
                s2_door2.transform.position = new Vector3(
                    s2_door2.transform.position.x,
                    Mathf.MoveTowards(s2_door2.transform.position.y, s2_targetPos_2, moveSpeed * Time.deltaTime),
                    s2_door2.transform.position.z
                );
            }
        }

        if (s2_targetObject_3 != null && s2_targetObject_3.GetComponent<ManagerRictusempra>() != null && s2_door3 != null)
        {
            // Access the isStunned variable from TargetScript on s2_targetObject_1
            bool isStunned = s2_targetObject_3.GetComponent<ManagerRictusempra>().isStunned;

            // Move the door if isStunned is true and the y position is greater than the target
            if (isStunned && s2_door3.transform.position.y > s2_targetPos_2)
            {
                // Move the door gradually by decreasing the y position
                s2_door3.transform.position = new Vector3(
                    s2_door3.transform.position.x,
                    Mathf.MoveTowards(s2_door3.transform.position.y, s2_targetPos_2, moveSpeed * Time.deltaTime),
                    s2_door3.transform.position.z
                );
            }
        }

        if (s2_targetObject_4 != null && s2_targetObject_4.GetComponent<ManagerRictusempra>() != null && s2_door4 != null)
        {
            // Access the isStunned variable from TargetScript on s2_targetObject_1
            bool isStunned = s2_targetObject_4.GetComponent<ManagerRictusempra>().isStunned;

            // Move the door if isStunned is true and the y position is greater than the target
            if (isStunned && s2_door4.transform.position.y > s2_targetPos_2)
            {
                // Move the door gradually by decreasing the y position
                s2_door4.transform.position = new Vector3(
                    s2_door4.transform.position.x,
                    Mathf.MoveTowards(s2_door4.transform.position.y, s2_targetPos_2, moveSpeed * Time.deltaTime),
                    s2_door4.transform.position.z
                );
            }
        }

        if (s2_door1.transform.position.y == s2_targetPos_2 &&
            s2_door2.transform.position.y == s2_targetPos_2 &&
            s2_door3.transform.position.y == s2_targetPos_2 &&
            s2_door4.transform.position.y == s2_targetPos_2)
        {
            // Only move if both cubes have not yet reached -36 on the x-axis
            if (s2_cube3.transform.position.x > -4f || s2_cube4.transform.position.x > -4f)
            {
                // Move s2_cube3 gradually by decreasing the x position
                s2_cube3.transform.position = new Vector3(
                    Mathf.MoveTowards(s2_cube3.transform.position.x, -4f, moveSpeed * Time.deltaTime),
                    s2_cube3.transform.position.y,
                    s2_cube3.transform.position.z
                );

                // Move s2_cube4 gradually by decreasing the x position
                s2_cube4.transform.position = new Vector3(
                    Mathf.MoveTowards(s2_cube4.transform.position.x, -4f, moveSpeed * Time.deltaTime),
                    s2_cube4.transform.position.y,
                    s2_cube4.transform.position.z
                );
            }
        }


        #endregion

        #region Section 3
        if (s3_targetObject != null && s3_targetObject.transform.position.z > 102 && s3_targetObject.transform.position.x < -11)
        {
            if (s3_cube1.transform.position.x < s3_targetPos_1 && s3_cube2.transform.position.x < s3_targetPos_1)
            {
                // Move the cube gradually by decreasing the x position
                s3_cube1.transform.position = new Vector3(
                    s3_cube1.transform.position.x,
                    s3_cube1.transform.position.y,
                    Mathf.MoveTowards(s3_cube1.transform.position.z, s3_targetPos_1, moveSpeed * Time.deltaTime)
                );

                // Move the cube gradually by decreasing the x position
                s3_cube2.transform.position = new Vector3(
                    s3_cube2.transform.position.x,
                    s3_cube2.transform.position.y,
                    Mathf.MoveTowards(s3_cube2.transform.position.z, s3_targetPos_1, moveSpeed * Time.deltaTime)
                );
            }

            if (s3_door1.transform.position.y < s3_targetPos_2 && s3_door2.transform.position.y < s3_targetPos_2)
            {
                // Move the cube gradually by decreasing the x position
                s3_door1.transform.position = new Vector3(
                    s3_door1.transform.position.x,
                    Mathf.MoveTowards(s3_door1.transform.position.y, s3_targetPos_2, moveSpeed * Time.deltaTime),
                    s3_door1.transform.position.z
                );

                // Move the cube gradually by decreasing the x position
                s3_door2.transform.position = new Vector3(
                    s3_door2.transform.position.x,
                    Mathf.MoveTowards(s3_door2.transform.position.y, s3_targetPos_2, moveSpeed * Time.deltaTime),
                    s3_door2.transform.position.z
                );
            }

            if (s3_floor1.transform.position.z > s3_targetPos_4 && s3_floor2.transform.position.z > s3_targetPos_3)
            {
                Debug.Log(s3_floor2.transform.position.z);

                // Move s3_floor1 toward s3_targetPos_4 along the z-axis
                s3_floor1.transform.position = new Vector3(
                    s3_floor1.transform.position.x,
                    s3_floor1.transform.position.y,
                    Mathf.MoveTowards(s3_floor1.transform.position.z, s3_targetPos_4, moveSpeed * Time.deltaTime)
                );

                // Move s3_floor2 in the opposite direction by moving away from s3_targetPos_3
                s3_floor2.transform.position = new Vector3(
                    s3_floor2.transform.position.x,
                    s3_floor2.transform.position.y,
                    Mathf.MoveTowards(s3_floor2.transform.position.z, s3_targetPos_3 - (s3_targetPos_3 - s3_floor2.transform.position.z) * 2, moveSpeed * Time.deltaTime)
                );
            }
        }

        if (s3_targetObject_2 != null && s3_targetObject_3 != null && s3_targetObject_2.GetComponent<ManagerRictusempra>() != null && s3_targetObject_3.GetComponent<ManagerRictusempra>() != null && s3_floor3 != null)
        {
            // Access the isStunned variable from TargetScript on targetObject
            bool isStunned1 = s3_targetObject_2.GetComponent<ManagerRictusempra>().isStunned;
            bool isStunned2 = s3_targetObject_3.GetComponent<ManagerRictusempra>().isStunned;

            if (isStunned1 && isStunned2)
            {
                rise_platform = true;
            }

            // Move the floor3 up to y = 0 if isStunned is true for both target objects
            if (rise_platform && s3_floor3.transform.position.y < 0)
            {
                s3_floor3.transform.position = new Vector3(
                    s3_floor3.transform.position.x,
                    Mathf.MoveTowards(s3_floor3.transform.position.y, 0, moveSpeed * Time.deltaTime),
                    s3_floor3.transform.position.z
                );
            }
        }


        #endregion

        #region Section 4

        #endregion

        #region Section 5

        #endregion

        #region Section 6

        #endregion

    }
}
