using UnityEngine;

public class MicrophoneInput : MonoBehaviour
{
    public static float Loudness { get; private set; }

    static AudioClip mic;
    static bool isRecording = false;
    const int sampleWindow = 128;

    public static void StartMic()
    {
        if (isRecording) return;

        if (Microphone.devices.Length > 0)
        {
            mic = Microphone.Start(null, true, 1, 44100);
            isRecording = true;
        }
        else
        {
            Debug.LogError("No microphone detected");
        }
    }

    public static void StopMic()
    {
        if (!isRecording) return;

        Microphone.End(null);
        mic = null;
        isRecording = false;
        Loudness = 0;
    }

    void Update()
    {
        if (!isRecording || mic == null) return;

        int pos = Microphone.GetPosition(null) - sampleWindow;
        if (pos < 0) return;

        float[] samples = new float[sampleWindow];
        mic.GetData(samples, pos);

        float level = 0f;
        for (int i = 0; i < samples.Length; i++)
            level = Mathf.Max(level, Mathf.Abs(samples[i]));

        Loudness = level;
    }
}
