import { createRouter, createWebHistory } from 'vue-router';

import EventDetails from '@/components/EventDetails.vue';
import Home from '@/pages/Home.vue';
import Login from '@/pages/Login.vue';
import Register from '@/pages/Register.vue';
import SearchEvent from '@/pages/SearchEvent.vue';
import UserProfile from '@/pages/UserProfile.vue';

const routes = [
    {
      path: '/login', name: 'Login', component: Login,
    },
    {
        path: '/register', name: 'Register', component: Register,
    },
    {
      path: '/', name: 'Home', component: Home,
    },
    {
      path: '/profile', name: 'Profile', component: UserProfile, meta: { requiresAuth: true },
    },
    {
      path: '/events', name: 'Events', component: SearchEvent,
    },
    {
      path: '/events/:id', name: 'EventDetails', component: EventDetails, props: true,
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