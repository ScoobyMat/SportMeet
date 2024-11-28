import { createRouter, createWebHistory } from 'vue-router';

import Home from '@/pages/Home.vue';
import Login from '@/pages/Login.vue';
import Register from '@/pages/Register.vue';
import SearchEvent from '@/pages/SearchEvent.vue';
import UserProfile from '@/pages/UserProfile.vue';

const routes = [
    {
      path: '/login',
      name: 'Logowanie',
      component: Login,
    },
    {
        path: '/register',
        name: 'Rejestracja',
        component: Register,
      },
    {
      path: '/',
      name: 'Home',
      component: Home,
    },
    {
      path: '/profile',
      name: 'Profil',
      component: UserProfile,
      meta: { requiresAuth: true },
    },
    {
      path: '/events',
      name: 'wydarzenia',
      component: SearchEvent,
    },
  ];

  const router = createRouter({
    history: createWebHistory(),
    routes,
  });

  router.beforeEach((to, from, next) => {
    const isLoggedIn = !!localStorage.getItem('user');
  
    if (to.matched.some((record) => record.meta.requiresAuth)) {
      if (!isLoggedIn) {
        next('/login');
      } else {
        next();
      }
    } else {
      next();
    }
  });
  
  export default router;