import { useAuthStore } from '@/stores/auth';
import { createRouter, createWebHistory } from 'vue-router';

import CreateEvent from '@/pages/CreateEvent.vue';
import EventGroup from '@/pages/EventGroup.vue';
import Home from '@/pages/Home.vue';
import Login from '@/pages/Login.vue';
import Register from '@/pages/Register.vue';
import SearchEvent from '@/pages/SearchEvent.vue';
import UserEdit from '@/pages/UserEdit.vue';
import UserProfile from '@/pages/UserProfile.vue';
import UsersList from '@/pages/UsersList.vue';

const routes = [
  { path: '/login', name: 'Login', component: Login },
  { path: '/register', name: 'Register', component: Register },
  { path: '/', name: 'Home', component: Home },
  { path: '/profile', name: 'Profile', component: UserProfile, meta: { requiresAuth: true } },
  { path: '/profile/edit', name: 'UserProfileEdit', component: UserEdit, meta: { requiresAuth: true } },
  { path: '/my-events', name: 'MyEvents', component: Home, meta: { requiresAuth: true } },
  { path: '/events', name: 'Events', component: SearchEvent },
  { path: '/event/create', name: 'EventCreate', component: CreateEvent, meta: { requiresAuth: true } },
  { path: '/event/:eventId', name: 'EventGroup', component: EventGroup, props: true },
  { path: '/users', name: 'UsersList', component: UsersList, meta: { requiresAuth: true } },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

router.beforeEach((to, from, next) => {
  const authStore = useAuthStore();

  if (to.meta.requiresAuth && !authStore.isAuthenticated) {
    next({ name: 'Login' });
  } else {
    next();
  }
});

export default router;
