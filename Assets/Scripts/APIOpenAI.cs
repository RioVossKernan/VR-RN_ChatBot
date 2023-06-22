using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

namespace OpenAI
{
    public class APIOpenAI : MonoBehaviour
    {
        public GameObject assessmentTextObj;
        public GameObject sallyVoice;
        private TextToSpeech textToSpeech;
        private TextMeshPro chatText;
        private TextMeshPro assessmentText;

        private readonly string fileName = "output.wav";
        
        private AudioClip clip;
        private bool isRecording;
        private int duration = 10;

        private OpenAIApi openai = new OpenAIApi("PUT YOUR KEY HERE");

        private List<ChatMessage> chats = new List<ChatMessage>();
        private List<ChatMessage> assessments = new List<ChatMessage>();
        private string chatPrompt =
            "Pretend to be my female patient for this conversation. I am your nurse.  Start by introducing yourself to me so I can begin my evaluation";
        private string assessmentPrompt =
            "Please, to the best of your ability, provide concise advice for how this nurse can better converse with her patient based on the provided conversation. Are you ready?";
        private string currentConvo = "";


        private void Start()
        {
            chatText = GetComponent<TextMeshPro>();
            assessmentText = assessmentTextObj.GetComponent<TextMeshPro>();
            textToSpeech = sallyVoice.GetComponent<TextToSpeech>();

            ChatCall(chatPrompt, true);
            AssessmentCall(assessmentPrompt,true);
        }

        public void StartRecording()
        {
            if (!isRecording)
            {
                isRecording = true;

                clip = Microphone.Start(Microphone.devices[0], false, duration, 44100);
                //message.text = "Recording...";
                //foreach(ChatMessage chat in assessments)
                //{
                //    assessmentText.text += chat.Content;
                //}
            }
        }

        public async void EndRecording()
        {
            //message.text = "Storing .WAV";
            isRecording = false;
            Microphone.End(null);
            byte[] data = SaveWav.Save(fileName, clip);
            
            var req = new CreateAudioTranscriptionsRequest
            {
                FileData = new FileData() {Data = data, Name = "audio.wav"},
                // File = Application.persistentDataPath + "/" + fileName,
                Model = "whisper-1",
                Language = "en"
            };

            chatText.text = "Calling API...";
            var res = await openai.CreateAudioTranscription(req);

            //message.text = res.Text;
            ChatCall(res.Text,false);
        }

        public async void ChatCall(String speech, bool introCall)
        {
            var chat = new ChatMessage()
            {
                Role = "user",
                Content = speech
            };
            chats.Add(chat);

            chatText.text = "Loading...";
            var completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
            {
                Model = "gpt-3.5-turbo-0301",
                Messages = chats
            });

            if (completionResponse.Choices != null && completionResponse.Choices.Count > 0)
            {
                var chatRes = completionResponse.Choices[0].Message;
                chatRes.Content = chatRes.Content.Trim();
                chats.Add(chatRes);

                chatText.text = "CHAT: \n" + chatRes.Content;
                textToSpeech.speakText(chatRes.Content);
             

                if (!introCall)
                {
                    currentConvo += "Nurse:" + speech;
                    AssessmentCall(currentConvo, false);
                }

                currentConvo += "Patient:" + chatRes.Content;

            }
        }

        public async void AssessmentCall(String speech, bool introCall)
        {
            if (introCall)
            {
                var setUpChat = new ChatMessage()
                {
                    Role = "system",
                    Content = "Your job is to provide advice to nurses to help them better converse with patients"
                };
                assessments.Add(setUpChat);
            }

            var chat = new ChatMessage()
            {
                Role = "user",
                Content = speech
            };
            assessments.Add(chat);


            assessmentText.text = "Loading...";
            var completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
            {
                Model = "gpt-3.5-turbo-0301",
                Messages = assessments
            });

            if (completionResponse.Choices != null && completionResponse.Choices.Count > 0)
            {
                var chatRes = completionResponse.Choices[0].Message;
                chatRes.Content = chatRes.Content.Trim();
                assessmentText.text = "ASSESSMENT: \n" + chatRes.Content;
                currentConvo = "";
            }
        }

        private void Update()
        {
        }
    }
}
