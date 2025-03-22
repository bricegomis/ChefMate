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
      {
        path: 'product/:id',
        name: 'product',
        component: () => import('pages/ProductPage.vue'),
        meta: { transition: 'slide-right' },
      },
    ],
  },
  {
    path: '/scanner',
    name: 'Scanner',
    component: () => import('layouts/MainLayout.vue'),
    children: [
      {
        path: '/scanner',
        name: 'scanner',
        component: () => import('pages/ScannerPage.vue'),
        meta: { transition: 'slide-left' },
      },
    ],
  },
  {
    path: '/stats',
    name: 'Stats',
    component: () => import('layouts/MainLayout.vue'),
    children: [
      {
        path: '/stats',
        name: 'stats',
        component: () => import('pages/StatsPage.vue'),
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
