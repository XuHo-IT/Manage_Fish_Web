import axios from 'axios'

const api = axios.create({
  baseURL: 'https://localhost:7229/api/UserAuth/', // Update based on your API
  headers: {
    'Content-Type': 'application/json',
  },
})

// Attach token automatically
api.interceptors.request.use((config) => {
  const token = localStorage.getItem('token')
  if (token) {
    config.headers.Authorization = `Bearer ${token}`
  }
  return config
})

export default api
