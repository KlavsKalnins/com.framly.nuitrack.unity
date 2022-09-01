using UnityEngine;
using NuitrackSDK.Avatar;
using UnityAtoms.BaseAtoms;

namespace Framly.Nuitrack
{
    public class SetNuitrackAvatarUser : MonoBehaviour
    {
        [SerializeField] IntVariable _avatarID;
        [SerializeField] NuitrackAvatar _nuitrackAvatar;
        private void Awake()
        {
            _avatarID.Changed.Register(SetAvatarID);
            if (_nuitrackAvatar == null)
                _nuitrackAvatar = GetComponent<NuitrackAvatar>();

            _nuitrackAvatar.UseCurrentUserTracker = false;
        }
        private void OnEnable()
        {
            if (_avatarID.Value != 0)
                SetAvatarID(_avatarID.Value);
        }

        private void OnDestroy()
        {
            _avatarID.Changed.Unregister(SetAvatarID);
        }

        public void SetAvatarID(int value)
        {
            if (_avatarID.Value > 0)
                _nuitrackAvatar.UserID = _avatarID.Value;
        }
    }
}
