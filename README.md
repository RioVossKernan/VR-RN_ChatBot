# VR-RN_ChatBot

In this VR Experience designed for the Oculus Quest, the user plays the role of a nurse. They are able to verbally converse with a digital patient who has articulate responses, facial expressions, and body gestures as they speak. Alongside this discussion is an assessment of the user's professionalism and tips on how to better word things to sound more empathetic and professional.

The experience takes place in a photorealistic operating room I designed using UnityStore Assets. The patient is a 3D model pulled off of Ready Player Me, a software designed for making VR avatars for metaverse games. As you speak to the patient, your voice is recorded and sent to OpenAI's Whisper Engine to convert into text. This text is then forwarded to the API for ChatGPT-3.5-Turbo, along with a personally designed prompt to return the optimal response from the "patient"/ChatGPT. This text response is then forwarded to Amazon Web Service's Polly API to convert the text into speech. This audio file is brought back to Unity and spoken by the patient. As it speaks, Oculus LipSync software is connected and rigged to the face and mouth of the avatar, allowing the lips to move in accordance with the spoken words. The assessment board, a wall to the right of the patient, is also managed by the ChatGPT AI Engine. As you speak with the patient, both sides of the conversation are sent to a new thread OpenAI's API, with another designed prompt for the AI to assess and provide advice to you based on how you have been speaking to the patient.

This was an amazing experience, and I am thankful for all I've learned. I am now able to interface with Unity for VR development, build apps that send and receive data to and from AWS and OpenAI APIs and have the two interact. In addition, there are countless more skills, small and large, that this project has trained me in. To experience my work for yourself, download the code and upload it to an Oculus Quest or a Meta Quest 2.

EDIT: due to security risks, I have removed my API keys from the scripts. This means to run this program on your own device, you will need to add your own API Keys into their respective slots in the "Text-To-Speech" and "OpenAiAPI" scripts in the asset folder.


Credits to:
VR-RN for inspiration, Oculus LipSync for speech rigging technology, Amazon Polly for text-to-speech, OpenAI for chat generation and speech-to-text, ReadyPlayerMe for a Human 3D Model, Unity for the technology that makes it all work
