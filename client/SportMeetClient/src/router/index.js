import { useAuthStore } from '@/stores/auth';
import { createRouter, createWebHistory } from 'vue-router';

import EventGroup from '@/pages/EventGroup.vue';
import Home from '@/pages/Home.vue';
import Login from '@/pages/Login.vue';
import Register from '@/pages/Register.vue';
import UserProfile from '@/pages/UserProfile.vue';
import UsersList from '@/pages/UsersList.vue';
import EventCreate from '../pages/EventCreate.vue';
import EventEdit from '../pages/EventEdit.vue';
import EventSearch from '../pages/EventSearch.vue';
import EventUserManaged from '../pages/EventUserManaged.vue';
import FriendsList from '../pages/FriendsList.vue';
import MyProfile from '../pages/MyProfile.vue';
import MyProfileEdit from '../pages/MyProfileEdit.vue';
import PrivateChat from '../pages/PrivateChat.vue';

const routes = [
  { path: '/login', name: 'Login', component: Login },
  { path: '/register', name: 'Register', component: Register },
  { path: '/', name: 'Home', component: Home },
  { path: '/user/:username', name: 'UserProfile', component: UserProfile, props: true, meta: { requiresAuth: true }},
  { path: '/events', name: 'Events', component: EventSearch, meta: { requiresAuth: true }},
  { path: '/event/group/:eventId', name: 'EventGroup', component: EventGroup, props: true, meta: { requiresAuth: true } },
  { path: '/profile', name: 'MyProfile', component: MyProfile, meta: { requiresAuth: true } },
  { path: '/profile/edit', name: 'UserProfileEdit', component: MyProfileEdit, meta: { requiresAuth: true } },
  { path: '/event/create', name: 'EventCreate', component: EventCreate, meta: { requiresAuth: true } },
  { path: '/myEvents', name: 'MyEvents', component: EventUserManaged, meta: { requiresAuth: true } },
  { path: '/myEvents/update/:eventId', name: 'EventEdit', component: EventEdit, props: true, meta: { requiresAuth: true } },
  { path: '/users', name: 'UsersList', component: UsersList, meta: { requiresAuth: true } },
  { path: '/friends', name: 'Friends', component: FriendsList, meta: { requiresAuth: true }},
  { path: "/private-chat/:friendId", name: "PrivateChat", component: PrivateChat, meta: { requiresAuth: true }},
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
