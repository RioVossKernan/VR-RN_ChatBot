using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Amazon.Polly;
using Amazon.Runtime;
using Amazon;
using Amazon.Polly.Model;
using System.IO;
using UnityEngine.Networking;
using System.Threading.Tasks;

public class TextToSpeech : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    BasicAWSCredentials credentials;
    AmazonPollyClient client;
    [SerializeField] private string speakOnStart;
    

    // Start is called before the first frame update
    public void Start()
    {
        credentials = new BasicAWSCredentials("AKIAWLBKCKCADNOQXJZV", "4BC4tFw2ilpCEXsNVZ8BehmD8ETKt3GcOFea4fuF");
        client = new AmazonPollyClient(credentials, RegionEndpoint.USEast1);
        speakText(speakOnStart);
    }

    public async void speakText(string text)
    {
        var request = new SynthesizeSpeechRequest()
        {
            Text = text,
            Engine = Engine.Neural,
            VoiceId = VoiceId.Salli,
            OutputFormat = OutputFormat.Mp3
        };

        var response = await client.SynthesizeSpeechAsync(request);
        Debug.Log("RESPONSE RECIEVED");
        Debug.Log($"file://{Application.persistentDataPath}/audio.mp3");


        WriteIntoFile(response.AudioStream);

        string audioPath;

            #if UNITY_ANDRIOD && !UNITY_EDITOR
                            audioPath = $"jar:file://{Application.persistentDataPath}/clip.mp3";
            #else
                    audioPath = $"file://{Application.persistentDataPath}/clip.mp3";
            #endif

        using (var www = UnityWebRequestMultimedia.GetAudioClip(audioPath, AudioType.MPEG))
        {
            var op = www.SendWebRequest();

            while (!op.isDone) await Task.Yield();

            var clip = DownloadHandlerAudioClip.GetContent(www);

            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    private void WriteIntoFile(Stream stream)
    {
        using (var fileStream = new FileStream($"{Application.persistentDataPath}/clip.mp3", FileMode.Create))
        {
            byte[] buffer = new byte[8 * 1024];
            int bytesRead;

            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                fileStream.Write(buffer, 0, bytesRead);
            }
          }
    }
}
