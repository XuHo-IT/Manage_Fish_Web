<template>
  <div class="container row">
    <h2 class="col-5" style="padding: 10px 15px; font-size: 16px; margin: 10px">
      🎙 Search by voice
    </h2>
    <div class="col-6">
      <button
        @click="startRecording"
        v-if="!recording"
        style="box-shadow: 0 0 0 0.25rem rgba(5, 222, 103, 0.25)"
      >
        Start Recording
      </button>
      <button
        @click="stopRecording"
        v-else
        style="box-shadow: 0 0 0 0.25rem rgba(5, 222, 103, 0.25)"
      >
        Stop Recording
      </button>
    </div>

    <div v-if="result" class="result">
      <h3>Recognition Result:</h3>
      <p>{{ result }}</p>
    </div>
  </div>
</template>

<script setup>
import { eventBus, eventBus2 } from "@/js/eventBus.js";
import { ref, watch } from "vue";
import api from "@/js/api_auth.js";
import Swal from "sweetalert2";

// Reactive variables
const selectedFile = ref(null);
const result = ref("");

// For MediaRecorder option
const recording = ref(false);
let mediaRecorder = null;
let audioChunks = [];

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
        Swal.fire({
          icon: "error",
          title: "Invalid Quantity",
          text: "No audio was recorded. Please speak something.",
        });

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

watch(
  () => eventBus2.result,
  (newValue) => {
    if (newValue === "Open_Palm" && !recording.value) {
      console.log("Gesture detected: openPlam → Starting recording");
      startRecording();
    } else if (newValue === "Closed_Fist" && recording.value) {
      console.log("Gesture detected: closedFist → Stopping recording");
      stopRecording();
    }
  },
);
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
