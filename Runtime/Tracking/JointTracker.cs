using NuitrackSDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityAtoms.BaseAtoms;

namespace Framly.Nuitrack
{
    [RequireComponent(typeof(Image))]
    public class JointTracker : MonoBehaviour
    {
        [Header("Configure")]
        public IntVariable userID;
        [SerializeField] protected nuitrack.JointType _joint;
        [SerializeField] protected float _positionLerpSpeed = 20f;

        protected bool _isVisible = true;
        protected bool _isInteractable = true;

        protected Image _thisImage;

        protected RectTransform _thisRectTransform;
        protected RectTransform _parentRectTransform;

        protected UserData _userData;


        protected virtual void Awake()
        {
            _thisImage = GetComponent<Image>();
            _parentRectTransform = transform.parent.gameObject.GetComponent<RectTransform>();
            _thisRectTransform = _thisImage.rectTransform;
        }

        protected virtual void Update()
        {
            _userData = userID == null ? NuitrackManager.Users.Current : NuitrackManager.Users.GetUser(userID.Value);

            if (_userData == null)
                return;

            try
            {
                UserData.SkeletonData.Joint joint = _userData.Skeleton.GetJoint(_joint);

                if (joint.Confidence <= 0.1f)
                    return;

                _thisRectTransform.anchoredPosition = Vector2.Lerp(_thisRectTransform.anchoredPosition, joint.AnchoredPosition(_parentRectTransform.rect, _thisRectTransform), Time.deltaTime * _positionLerpSpeed);
            }
            catch
            {

            }
        }

        public virtual void ToggleInteractive(bool state)
        {
            try
            {
                _isVisible = state;
                _thisImage.enabled = _isVisible;
            }
            catch
            {
                _isVisible = state;
                _thisImage = GetComponent<Image>();
                _thisImage.enabled = _isVisible;
            }
        }
    }
}
