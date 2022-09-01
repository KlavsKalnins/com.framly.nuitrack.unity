using UnityEngine.UI;

namespace Framly.Nuitrack
{
    public class Hand : JointTracker
    {
        public static Hand[] instance = new Hand[2];
        public static bool isHandTracking = false;

        protected override void Awake()
        {
            base.Awake();
            SetupInstanceHands();
        }

        void SetupInstanceHands()
        {
            if (_joint == nuitrack.JointType.LeftHand)
            {
                instance[0] = this;
            }
            if (_joint == nuitrack.JointType.RightHand)
            {
                instance[1] = this;
            }
        }
        public Image GetImageComponent()
        {
            return _thisImage;
        }
        public override void ToggleInteractive(bool state)
        {
            //base.ToggleInteractive(state);
            isHandTracking = state;
            foreach (var hand in Hand.instance)
            {
                hand._isVisible = state;
                hand._thisImage.enabled = state;
            }

        }
    }
}