using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityAtoms.BaseAtoms;

namespace Framly.Nuitrack
{
    public class UserValidator : MonoBehaviour
    {
        [SerializeField] IntVariable _userIndex;
        public int _cachedInt;

        [SerializeField] UnityEvent _onTrue;
        [SerializeField] UnityEvent _onFalse;

        private void Awake()
        {
            _userIndex.Changed.Register(ValidateEvents);

        }
        private void OnDestroy()
        {
            _userIndex.Changed.Unregister(ValidateEvents);
        }
        private void Start()
        {
            ValidateEvents(_userIndex.Value);
        }

        public void ValidateEvents(int value)
        {
            Debug.Log(value);
            if (_cachedInt == value)
                return;
            _cachedInt = value;
            if (_cachedInt <= 0 || _cachedInt > 6)
            {
                _onFalse.Invoke();
                return;
            }
            _onTrue.Invoke();
        }
    }
}

