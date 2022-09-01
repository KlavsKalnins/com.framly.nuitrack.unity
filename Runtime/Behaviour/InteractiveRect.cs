using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityAtoms.BaseAtoms;

namespace Framly.Nuitrack
{
    public class InteractiveRect : MonoBehaviour, IHoverable
    {

        [Header("Configuration")]
        public Image buttonFill;
        [SerializeField] protected VoidEvent onButtonPress;

        [Header("Optional Configuration")]
        public Image buttonImage;
        protected float secondCounter;
        protected float secondsTillFull = 1.5f;
        protected private float interractionScale = 2f;

        private Image interractorRightHand;
        private Image interractorLeftHand;

        [SerializeField] protected bool _isUsable = true;

        private void Awake()
        {
            if (buttonImage == null)
                buttonImage = GetComponent<Image>();

        }
        private void Start()
        {
            interractorLeftHand = Hand.instance[0].GetImageComponent();
            interractorRightHand = Hand.instance[1].GetImageComponent();
        }

        void Update()
        {

            if (!Hand.isHandTracking)
            {
                ResetButton();
                return;
            }

            if (interractorRightHand.transform.localPosition.x + (interractorRightHand.rectTransform.rect.width / interractionScale) > buttonImage.transform.localPosition.x - (buttonImage.rectTransform.rect.width / interractionScale)
                && interractorRightHand.transform.localPosition.x - (interractorRightHand.rectTransform.rect.width / interractionScale) < buttonImage.transform.localPosition.x + (buttonImage.rectTransform.rect.width / interractionScale)
                && interractorRightHand.transform.localPosition.y - (interractorRightHand.rectTransform.rect.height / interractionScale) < buttonImage.transform.localPosition.y + (buttonImage.rectTransform.rect.height / interractionScale)
                && interractorRightHand.transform.localPosition.y + (interractorRightHand.rectTransform.rect.height / interractionScale) > buttonImage.transform.localPosition.y - (buttonImage.rectTransform.rect.height / interractionScale)
                || interractorLeftHand.transform.localPosition.x + (interractorLeftHand.rectTransform.rect.width / interractionScale) > buttonImage.transform.localPosition.x - (buttonImage.rectTransform.rect.width / interractionScale)
                && interractorLeftHand.transform.localPosition.x - (interractorLeftHand.rectTransform.rect.width / interractionScale) < buttonImage.transform.localPosition.x + (buttonImage.rectTransform.rect.width / interractionScale)
                && interractorLeftHand.transform.localPosition.y - (interractorLeftHand.rectTransform.rect.height / interractionScale) < buttonImage.transform.localPosition.y + (buttonImage.rectTransform.rect.height / interractionScale)
                && interractorLeftHand.transform.localPosition.y + (interractorLeftHand.rectTransform.rect.height / interractionScale) > buttonImage.transform.localPosition.y - (buttonImage.rectTransform.rect.height / interractionScale))
            {

                secondCounter += Time.unscaledDeltaTime;
                Hover(true);

                if (secondCounter / secondsTillFull > 1)
                {
                    if (buttonFill != null)
                        buttonFill.fillAmount = 1;
                    Press();
                    secondCounter = -0.5f;
                }
                else
                {
                    if (buttonFill != null)
                        buttonFill.fillAmount = secondCounter / secondsTillFull;
                }

            }
            else
            {
                ResetButton();
            }
        }
        private void ResetButton()
        {
            Hover(false);
            secondCounter = 0;
            if (buttonFill != null)
                buttonFill.fillAmount = 0;
        }

        public virtual void Press()
        {
            if (onButtonPress != null)
                onButtonPress.Raise();

        }
        public virtual void Hover(bool state)
        {

        }

        public virtual void IsUsable()
        {
            _isUsable = true;
        }
    }
}