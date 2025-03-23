import { RouteRecordRaw } from 'vue-router';

const routes: RouteRecordRaw[] = [
  {
    path: '/',
    name: 'Products',
    component: () => import('layouts/MainLayout.vue'),
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
    component: () => import('layouts/MainLayout.vue'),
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
    path: '/:catchAll(.*)*',
    component: () => import('pages/ErrorNotFound.vue'),
  },
];

export default routes;
