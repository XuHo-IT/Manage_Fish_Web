<template>
    <div v-if="alertStore.isVisible" class="overlay">
      <div class="popup" :class="alertStore.type">
        <p>{{ alertStore.message }}</p>
        <button @click="alertStore.hideAlert">OK</button>
      </div>
    </div>
  </template>
  
  <script>
  import { useAlertStore } from "@/js/useAlertStore";
  
  export default {
    setup() {
      const alertStore = useAlertStore;
      return { alertStore };
    }
  };
  </script>
  
  <style scoped>
  /* Popup Styles */
  .overlay {
    position: fixed;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background: rgba(0, 0, 0, 0.5);
    display: flex;
    align-items: center;
    justify-content: center;
    z-index: 999;
  }
  
  .popup {
    background: white;
    padding: 20px;
    border-radius: 10px;
    text-align: center;
    box-shadow: 0px 4px 6px rgba(0, 0, 0, 0.1);
  }
  
  .success {
    border: 2px solid green;
    color: green;
  }
  
  .error {
    border: 2px solid red;
    color: red;
  }
  
  button {
    background: #007bff;
    color: white;
    border: none;
    padding: 10px 20px;
    cursor: pointer;
    border-radius: 5px;
  }
  
  button:hover {
    background: #0056b3;
  }
  </style>
  