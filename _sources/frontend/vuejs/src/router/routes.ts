import { CustomRouteRecordRaw } from './types';

const routes: CustomRouteRecordRaw[] = [
  {
    path: '/profile',
    name: 'Profile',
    icon: 'person',
    component: () => import('layouts/MainLayout.vue'),
    children: [
      {
        path: '',
        name: 'auth',
        component: () => import('pages/ProfilePage.vue'),
        meta: { transition: 'slide-left' },
      },
    ],
  },
  {
    path: '/',
    name: 'Products',
    icon: 'restaurant_menu',
    component: () => import('layouts/MainLayout.vue'),
    meta: { requiresAuth: true },
    children: [
      {
        path: '',
        name: 'products',
        component: () => import('pages/ProductsPage.vue'),
        meta: { transition: 'slide-left' },
      },
    ],
  },
  {
    path: '/chatgpt',
    name: 'Chatgpt',
    icon: 'chat',
    component: () => import('layouts/MainLayout.vue'),
    meta: { requiresAuth: true },
    children: [
      {
        path: '/chatgpt',
        name: 'chatgpt',
        component: () => import('pages/ChatGptPage.vue'),
        meta: { transition: 'slide-left' },
      },
    ],
  },
  {
    path: '/receipes',
    name: 'Receipes',
    icon: 'restaurant',
    component: () => import('layouts/MainLayout.vue'),
    meta: { requiresAuth: true },
    children: [
      {
        path: '',
        name: 'receipes',
        component: () => import('pages/ReceipesPage.vue'),
        meta: { transition: 'slide-left' },
      },
    ],
  },
  {
    path: '/:catchAll(.*)*',
    component: () => import('pages/ErrorNotFound.vue'),
  },
];

export default routes;
