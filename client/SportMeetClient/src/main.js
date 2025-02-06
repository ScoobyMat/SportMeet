import 'bootstrap-icons/font/bootstrap-icons.css';
import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap/dist/js/bootstrap.bundle.min.js';
import Toast from 'vue-toastification';
import 'vue-toastification/dist/index.css';

import { useUserStore } from "@/stores/user";
import { createPinia } from 'pinia';
import { createApp } from 'vue';
import App from './App.vue';
import router from './router';
import { useAuthStore } from './stores/auth';
import './style.css';

const options = {
  position: 'bottom-right',
  timeout: 3000,
  closeOnClick: true,
  pauseOnHover: true,
};

const pinia = createPinia();

createApp(App)
  .use(pinia)
  .use(router)
  .use(Toast, options)
  .mount('#app');

  const authStore = useAuthStore();
  const userStore = useUserStore();
  
  authStore.loadToken();

  if (authStore.isAuthenticated) {
    userStore.fetchUser(authStore.getUserId);
  }