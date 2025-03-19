<template>
  <div>
    <h1>Hand Gesture Recognition</h1>
    <p>Detected Gesture: {{ detectedGesture }}</p>
  </div>
</template>

<script setup>
import { eventBus2 } from "@/js/eventBus.js";
import { ref, onMounted } from "vue";

const detectedGesture = ref("");
const confidence = ref(0);
const handedness = ref("");

onMounted(() => {
  window.addEventListener("gestureDetected", (event) => {
    detectedGesture.value = event.detail.gesture;
    confidence.value = event.detail.confidence;
    handedness.value = event.detail.handedness;
    eventBus2.result = detectedGesture.value;
  });
});
</script>
