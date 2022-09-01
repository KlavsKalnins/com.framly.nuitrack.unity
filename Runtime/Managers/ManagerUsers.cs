using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NuitrackSDK;
using UnityAtoms.BaseAtoms;

namespace Framly.Nuitrack
{
    public class ManagerUsers : MonoBehaviour
    {

        [System.Serializable]
        public struct User
        {
            public Vector3 position;
            public bool hasUserEntered;
            public bool isInUserSpace;
            public float distanceToSensor;
            public GameObject debugObject;
        }
        [System.Serializable]
        public struct UserSpaceBox
        {
            public float killboxX; // x = -+metres ; y 
            public Vector2 killboxZ; // x min ;y max
            public UserSpaceBox(float killboxX, Vector2 killboxZ)
            {
                this.killboxX = killboxX;
                this.killboxZ = killboxZ;
            }
        }
        [Header("Configuration")]
        [Range(0, 6)]
        [SerializeField] int trackingUserCount = 6;
        [SerializeField] Transform _sensorPoint;
        [SerializeField] nuitrack.JointType _jointToEvaluateBy = nuitrack.JointType.Waist;
        [SerializeField] float _userDebugGameObjectScale = 0.5f;
        [Header("Variables and Events")]
        [SerializeField] IntVariable _userID;

        [Header("Runtime")]
        [SerializeField] UserSpaceBox userSpaceBox = new UserSpaceBox(0.2f, new Vector2(1.5f, 2.5f));
        [SerializeField] User[] users;

        [Header("Debug")]
        [SerializeField] KeyCode debugKey = KeyCode.Semicolon;
        [SerializeField] bool isDebugging;

        void OnEnable()
        {
            NuitrackManager.Users.OnUserEnter += OnUserEnter;
            NuitrackManager.Users.OnUserExit += OnUserExit;
        }
        void Awake()
        {
            InitNuitrack();
            users = new User[trackingUserCount];
            InitUsersDebugObjects();
        }

        void InitUsersDebugObjects()
        {
            for (int i = 0; i < users.Length; i++)
            {
                var inst = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                users[i].debugObject = inst;
                users[i].debugObject.SetActive(false);
                inst.transform.SetParent(transform);
                inst.transform.localScale = new Vector3(_userDebugGameObjectScale, _userDebugGameObjectScale, _userDebugGameObjectScale);
            }
        }
        #region Nuitrack
        void InitNuitrack()
        {
            if (NuitrackManager.Instance.nuitrackInitialized)
                NuitrackManager.SkeletonTracker.SetNumActiveUsers(trackingUserCount);

        }
        void OnUserExit(UserData user)
        {
            users[user.ID - 1].hasUserEntered = false;
        }
        void OnUserEnter(UserData user)
        {
            users[user.ID - 1].hasUserEntered = true;
        }
        #endregion Nuitrack


        void Update()
        {
            if (Input.GetKeyDown(debugKey))
            {
                isDebugging = !isDebugging;
                DebugLogic();
            }

            if (NuitrackManager.Users.Count <= 0)
            {
                if (_userID != null)
                {
                    if (_userID.Value != 0)
                    {
                        _userID.Value = 0;
                    }

                }
                for (int i = 0; i < users.Length; i++)
                {
                    users[i].debugObject.transform.position = users[i].position;
                }
            }
            else
            {
                for (int i = 0; i < users.Length; i++)
                {
                    var nuitrackUser = NuitrackManager.Users.GetUser(i + 1);
                    if (nuitrackUser != null && nuitrackUser.Skeleton != null)
                    {
                        users[i].position = nuitrackUser.Skeleton.GetJoint(_jointToEvaluateBy).Position;
                        //Debug.Log($"user {i} pos: {users[i].position} hasUserEntered: {users[i].hasUserEntered} isInUserSpace: {users[i].isInUserSpace}");
                        if (isDebugging)
                            users[i].debugObject.transform.position = users[i].position;
                    }

                }
            }
            CalculateUserDistanceToSensor();
            ValidateUserSpace();
            GetLowestUserDistance();
            var currentValidUserIndex = GetLowestUserDistance();
            if (currentValidUserIndex < 0) // maybe dont need
                return;
            if (currentValidUserIndex + 1 == _userID.Value)
                return;
            _userID.Value = currentValidUserIndex + 1;
        }
        void CalculateUserDistanceToSensor()
        {
            for (int i = 0; i < users.Length; i++)
            {
                users[i].distanceToSensor = Vector3.Distance(users[i].position, _sensorPoint != null ? _sensorPoint.position : new Vector3(0, 0, 0));
            }
        }

        void ValidateUserSpace()
        {
            for (int i = 0; i < users.Length; i++)
            {
                if (!users[i].hasUserEntered)
                {
                    users[i].isInUserSpace = false;
                    // return;
                }
                else
                {
                    bool isValid = true;
                    if (users[i].position.x < -userSpaceBox.killboxX || users[i].position.x > userSpaceBox.killboxX)
                        isValid = false;
                    if (users[i].position.z < userSpaceBox.killboxZ.x || users[i].position.z > userSpaceBox.killboxZ.y)
                    {
                        isValid = false;
                    }

                    users[i].isInUserSpace = isValid;
                    if (isValid)
                    {
                        if (_userID.Value != i + 1)
                        {
                            _userID.Value = i + 1;
                        }

                    }
                }
            }
            bool isAnyUser = false;
            for (int i = 0; i < users.Length; i++)
            {
                if (users[i].isInUserSpace)
                {
                    isAnyUser = true;
                }
            }
            if (!isAnyUser)
            {
                if (_userID.Value != 0)
                {
                    _userID.Value = 0;
                }

            }
        }

        int GetLowestUserDistance()
        {
            float value = float.PositiveInfinity;
            int index = -1;
            for (int i = 0; i < users.Length; i++)
            {
                if (users[i].isInUserSpace && users[i].distanceToSensor < value)
                {
                    index = i;
                    value = users[i].distanceToSensor;
                }
            }
            return index;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0.0f, 1.0f, 0.0f);
            Gizmos.DrawWireCube(new Vector3(0, 0, userSpaceBox.killboxZ.x), new Vector3(userSpaceBox.killboxX * 2, 0.1f, Mathf.Abs(userSpaceBox.killboxZ.y)));
        }

        void DebugLogic()
        {
            for (int i = 0; i < users.Length; i++)
            {
                users[i].debugObject.SetActive(isDebugging);
            }
        }
    }
}