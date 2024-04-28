using OpenAI;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Import the TextMesh Pro namespace

namespace Samples.Whisper
{
    public class Whisper : MonoBehaviour
    {
        [SerializeField] private Image progressBar;
        [SerializeField] private TextMeshProUGUI message; // Change this from Text to TextMeshProUGUI
        public SceneTransitionManager sceneTransitionManager;
        private readonly string fileName = "output.wav";
        private readonly int duration = 5;
        private readonly int totalRecordings = 3;
        private int currentRecording = 0;

        private AudioClip clip;
        private bool isRecording;
        private float time;
        private OpenAIApi openai = new OpenAIApi("sk-proj-oqD4RCPUAFtXJCx8ye2QT3BlbkFJwS6GMDk7btVDHg1tVGzZ");

        private void Start()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            message.text = "Microphone not supported on WebGL"; // TextMeshProUGUI supports the .text property
#else
            if (Microphone.devices.Length > 0)
            {
                PlayerPrefs.SetInt("user-mic-device-index", 0);  // Always use the first microphone
                StartRecording();
            }
            else
            {
                message.text = "No microphones available";
            }
#endif
        }

        private void StartRecording()
        {
            if (currentRecording < totalRecordings)
            {
                isRecording = true;

                var index = PlayerPrefs.GetInt("user-mic-device-index", 0);

#if !UNITY_WEBGL
                clip = Microphone.Start(Microphone.devices[index], false, duration, 44100);
#endif
            }
        }

        private async void EndRecording()
        {
            message.text = "Transcribing...";

#if !UNITY_WEBGL
            Microphone.End(null);
#endif

            byte[] data = SaveWav.Save(fileName, clip);
            var req = new CreateAudioTranscriptionsRequest
            {
                FileData = new FileData() { Data = data, Name = "audio.wav" },
                Model = "whisper-1",
                Language = "en"
            };
            var res = await openai.CreateAudioTranscription(req);

            progressBar.fillAmount = 0;
            if (currentRecording != 2)
            {
                message.text = "Listening...";
            }
            else
            {
                message.text = "";
            }

            if (res.Text.Contains("M") && res.Text.Contains("R") && res.Text.Contains("I"))
            {
                message.text = "Going directly to MRI Room";
                sceneTransitionManager.GoToSceneAsync(1);
            }

            currentRecording++;
            if (currentRecording < totalRecordings)
            {
                StartRecording();
            }
        }

        private void Update()
        {
            if (isRecording)
            {
                time += Time.deltaTime;
                progressBar.fillAmount = time / duration;

                if (time >= duration)
                {
                    time = 0;
                    isRecording = false;
                    EndRecording();
                }
            }
        }
    }
}
