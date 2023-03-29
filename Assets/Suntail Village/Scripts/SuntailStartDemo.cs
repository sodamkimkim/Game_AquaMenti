using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

//This script is only used to start the Suntail demo scene
namespace Suntail
{
    public class SuntailStartDemo : MonoBehaviour
    {
        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private Image blackScreenImage;
        [SerializeField] private Text blackScreenText1;
        [SerializeField] private Text blackScreenText2;
        [SerializeField] private Text hintText;
        [SerializeField] private float blackScreenDuration = 4f;
        [SerializeField] private float hintDuration = 14f;
        [SerializeField] private float fadingDuration = 3f;
        
        //Private variables
        private bool screenTimerIsActive = true;
        private bool hintTimerIsActive = true;

        private void Start()
        {
            if(blackScreenImage != null)
                blackScreenImage.gameObject.SetActive(true);
            if (blackScreenText1 != null)
                blackScreenText1.gameObject.SetActive(true);
            if (blackScreenText2 != null)
                blackScreenText2.gameObject.SetActive(true);
            if (hintText != null)
                hintText.gameObject.SetActive(true);
            _audioMixer.SetFloat("soundsVolume", -80f);
        }

        private void Update()
        {
            //Black screen timer
            if (screenTimerIsActive)
            {
                blackScreenDuration -= Time.deltaTime;
                if (blackScreenDuration < 0)
                {
                    screenTimerIsActive = false;
                    if (blackScreenImage != null)
                        blackScreenImage.CrossFadeAlpha(0, fadingDuration, false);
                    if (blackScreenText1 != null)
                        blackScreenText1.CrossFadeAlpha(0, fadingDuration, false);
                    if (blackScreenText2 != null)
                        blackScreenText2.CrossFadeAlpha(0, fadingDuration, false);
                    StartCoroutine(StartAudioFade(_audioMixer, "soundsVolume", fadingDuration, 1f));
                }
            }

            //Hint text timer
            if (hintTimerIsActive)
            {
                hintDuration -= Time.deltaTime;
                if (hintDuration < 0)
                {
                    hintTimerIsActive = false;
                    if (hintText != null)
                        hintText.CrossFadeAlpha(0, fadingDuration, false);
                }
            }
        }

        //Sound fading
        public static IEnumerator StartAudioFade(AudioMixer audioMixer, string exposedParam, float duration, float targetVolume)
        {
            float currentTime = 0;
            float currentVol;
            audioMixer.GetFloat(exposedParam, out currentVol);
            currentVol = Mathf.Pow(10, currentVol / 20);
            float targetValue = Mathf.Clamp(targetVolume, 0.0001f, 1);

            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                float newVol = Mathf.Lerp(currentVol, targetValue, currentTime / duration);
                audioMixer.SetFloat(exposedParam, Mathf.Log10(newVol) * 20);
                yield return null;
            }
            yield break;
        }
    }
}