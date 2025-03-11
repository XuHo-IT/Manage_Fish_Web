<template>
  <div class="container">
    <h2>🎙 Search by voice</h2>

    <!-- Option 1: File Input -->
    <!-- <div>
      <input type="file" @change="onFileChange" accept="audio/*" />
      <button @click="uploadAudio" :disabled="!selectedFile">Upload Audio</button>
    </div> -->

    <!-- Option 2: Recording (uncomment if you prefer using MediaRecorder) -->

    <div>
      <button @click="startRecording" v-if="!recording">Start Recording</button>
      <button @click="stopRecording" v-else>Stop Recording</button>
    </div>

    <div v-if="result" class="result">
      <h3>Recognition Result:</h3>
      <p>{{ result }}</p>
    </div>
  </div>
</template>

<script setup>
import { ref } from "vue";
import axios from "axios";
import api from "@/js/api_auth.js";
import {eventBus} from "@/js/eventBus.js"


// Reactive variables
const selectedFile = ref(null);
const result = ref("");

// For MediaRecorder option
const recording = ref(false);
let mediaRecorder = null;
let audioChunks = [];

// File input handler
const onFileChange = (event) => {
  if (event.target.files && event.target.files.length > 0) {
    selectedFile.value = event.target.files[0];
    console.log("Selected file:", selectedFile.value);
  }
};

// Function to upload a selected file to the API endpoint
const uploadAudio = async () => {
  if (!selectedFile.value) {
    alert("Please select an audio file first.");
    return;
  }
  const formData = new FormData();
  formData.append("file", selectedFile.value, selectedFile.value.name);

  try {
    const response = await axios.post(
      "https://localhost:7229/api/voice/recognize/bytes",
      formData,
      {
        headers: { "Content-Type": "multipart/form-data" },
      },
    );
    console.log("API Response:", response.data);
    const parsedResult = JSON.parse(response.data.result);
    result.value = parsedResult.text;
  } catch (error) {
    console.error("Error uploading audio:", error);
    result.value = "Error processing audio.";
  }
};

// // Function to start recording audio from the user's microphone
const startRecording = async () => {
  try {
    const stream = await navigator.mediaDevices.getUserMedia({ audio: true });
    mediaRecorder = new MediaRecorder(stream);
    audioChunks = [];
    mediaRecorder.ondataavailable = (event) => {
      if (event.data.size > 0) {
        audioChunks.push(event.data);
      }
    };
    mediaRecorder.onstop = async () => {
      const audioBlob = new Blob(audioChunks, { type: "audio/webm" });
      console.log("Recorded Audio Blob:", audioBlob);
      if (audioBlob.size === 0) {
        alert("No audio was recorded. Please speak something.");
        return;
      }
      const formData = new FormData();
      formData.append("file", audioBlob, "recorded_audio.webm");
      try {
        const response = await api.post("voice/recognize/bytes", formData, {
          headers: {
            "Content-Type": "multipart/form-data",
          },
        });
        console.log("API Response:", response.data);
        const parsedResult = JSON.parse(response.data.result);
        result.value = parsedResult.text;
        eventBus.result = result.value;
      } catch (error) {
        console.error("Error uploading recorded audio:", error);
        result.value = "Error processing audio.";
      }
    };
    mediaRecorder.start();
    recording.value = true;
  } catch (error) {
    console.error("Error accessing microphone:", error);
  }
};

const stopRecording = () => {
  if (mediaRecorder) {
    mediaRecorder.stop();
    recording.value = false;
  }
};
</script>

<style scoped>
.container {
  text-align: center;
  margin-top: 20px;
}
button {
  padding: 10px 15px;
  font-size: 16px;
  cursor: pointer;
  margin: 10px;
}
.result {
  margin-top: 20px;
  padding: 10px;
  background: #f4f4f4;
  border-radius: 5px;
}
</style>
