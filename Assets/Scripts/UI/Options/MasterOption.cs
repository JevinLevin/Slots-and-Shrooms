using UnityEngine;
using UnityEngine.Audio;

public class MasterOption : MonoBehaviour
{
      public Vector2 range = new Vector2(-20, 0);
      
      public AudioMixer mixer;
      public void SetValue(float value)
      {
            mixer.SetFloat("MasterVolume", Mathf.Lerp(range.x, range.y, value));
      }
}
