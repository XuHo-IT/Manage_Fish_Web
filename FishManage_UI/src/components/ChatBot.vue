<template>
  <!-- Chat Support Button -->
  <button class="btn-open-chat" @click="toggleChat">
    <svg class="svg-info" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 320 512">
      <path
        d="M70 160c0-35.3 28.7-64 64-64l32 0c35.3 0 64 28.7 64 64l0 3.6c0 21.8-11.1 42.1-29.4 53.8l-42.2 27.1c-25.2 16.2-40.4 44.1-40.4 74l0 1.4c0 17.7 14.3 32 32 32s32-14.3 32-32l0-1.4c0-8.2 4.2-15.8 11-20.2l42.2-27.1c36.6-23.6 58.8-64.1 58.8-107.7l0-3.6c0-70.7-57.3-128-128-128l-32 0C73.3 32 16 89.3 16 160c0 17.7 14.3 32 32 32s32-14.3 32-32zm80 320a40 40 0 1 0 0-80 40 40 0 1 0 0 80z"
      />
    </svg>
  </button>

  <!-- Chat Container -->
  <div v-if="isChatOpen" class="chat-container">
    <h1>ChatBot Support</h1>
    <button class="btn-close-chat" @click="toggleChatClose">X</button>

    <div class="chat-box">
      <div v-for="(msg, index) in messages" :key="index" class="chat-message">
        <div :class="msg.type">{{ msg.text }}</div>
      </div>
    </div>

    <div class="controls">
      <select v-model="selectedQuestion" class="question-dropdown">
        <option value="" disabled>Select a question...</option>
        <option v-for="(question, index) in questions" :key="index" :value="question">
          {{ question }}
        </option>
      </select>
      <button @click="askQuestion">Ask</button>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from "vue";
import api from "@/js/api_auth.js";
import Swal from "sweetalert2";

const isChatOpen = ref(false);
const questions = ref([]);
const selectedQuestion = ref("");
const messages = ref([]);


const toggleChat = () => {
  isChatOpen.value = true;
};

const toggleChatClose = () => {
  isChatOpen.value = false;
  messages.value = [];
};

const fetchQuestions = async () => {
  try {
    const response = await api.get("chatbot/questions");
    questions.value = response.data.questions;
  } catch (error) {
    console.error("Error", error);

  }
};
const askQuestion = async () => {
  if (!selectedQuestion.value) return;

  messages.value.push({ text: selectedQuestion.value, type: "user" });

  try {
    const response = await api.post("chatbot/ask", {
      message: selectedQuestion.value,
    });
    messages.value.push({ text: response.data.response, type: "bot" });
    localStorage.setItem("chatMessages", JSON.stringify(messages.value)); // Save messages
  } catch (error) {
    console.error("Error", error);
    messages.value.push({ text: "Error connecting to chatbot.", type: "bot" });
    localStorage.setItem("chatMessages", JSON.stringify(messages.value)); // Save messages
  }

  selectedQuestion.value = "";
};

onMounted(fetchQuestions);
</script>

<style>
.chat-container {
  position: fixed;
  bottom: 20px;
  right: 20px;
  width: 350px;
  background: white;
  box-shadow: 0 0 10px rgba(0, 0, 0, 0.3);
  border-radius: 10px;
  padding: 15px;
  z-index: 1000;
}
.btn-open-chat {
  position: fixed; 
  bottom: 20px; 
  right: 20px; 
  background-color: #0de329; 
  color: white;
  width: 60px; 
  height: 60px;
  border: none;
  cursor: pointer;
  font-size: 14px;
  border-radius: 50%; 
  display: flex;
  align-items: center;
  justify-content: center;
  box-shadow: 2px 2px 5px rgba(0, 0, 0, 0.2);
  margin-left: auto;

}

.btn-open-chat:hover {
  background-color: #0fbbde; 
  transform: scale(1.1); 
}

.btn-close-chat {
  background: red;
  color: white;
  border: none;
  padding: 5px 10px;
  cursor: pointer;
  float: right;
  border-radius: 5px;
}

.chat-box {
  border: 1px solid #ccc;
  padding: 10px;
  height: 300px;
  overflow-y: auto;
  background: #f9f9f9;
}

.chat-message {
  margin: 5px 0;
}
.user {
  background: #d1e7ff;
  padding: 5px;
  border-radius: 5px;
  text-align: right;
}
.bot {
  background: #e1ffd1;
  padding: 5px;
  border-radius: 5px;
  text-align: left;
}

/* Controls */
.controls {
  margin-top: 10px;
}
.question-dropdown {
  padding: 5px;
  width: 80%;
}
svg.svg-info {
    width: 17px;
}
</style>
