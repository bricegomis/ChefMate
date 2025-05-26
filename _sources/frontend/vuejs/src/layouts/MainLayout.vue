<template>
  <q-layout view="lHh Lpr lFf" style="background-color: #2b2a33">
    <!-- Drawer (Menu latéral) -->
    <q-drawer v-model="drawer" bordered>
      <q-list>
        <q-item
          v-for="route in routes"
          :key="route.name"
          clickable
          tag="a"
          :href="route.path"
        >
          <q-item-section>
            <q-icon :name="route.icon || 'menu'" />
          </q-item-section>
          <q-item-section>{{ route.name }}</q-item-section>
        </q-item>
      </q-list>
    </q-drawer>

    <!-- Header -->
    <q-header elevated>
      <q-toolbar>
        <!-- Menu Burger -->
        <q-btn flat round dense icon="menu" @click="drawer = !drawer" />

        <q-toolbar-title class="text-accent">
          Chef Mate
          <q-icon color="red" size="ml" name="wifi" v-if="!isOnline" />
        </q-toolbar-title>

        <div
          class="row text-h6 text-black justify-around items-center"
          v-if="productStore.profile"
        >
          <div>
            <q-avatar size="24px" @click="router.push('/profile')">
              <q-img :src="productStore.profile?.avatarUrl ?? ''" ratio="1" />
            </q-avatar>
          </div>
        </div>
      </q-toolbar>
    </q-header>

    <!-- Page Container -->
    <q-page-container>
      <router-view v-slot="{ Component }">
        <!-- Use a custom transition or fallback to `fade` -->
        <transition name="slide-right">
          <component :is="Component" />
        </transition>
      </router-view>
    </q-page-container>
  </q-layout>
</template>

<script setup lang="ts">
import { useProductStore } from 'src/stores/product-store';
import { useRouter } from 'vue-router';
import { ref, computed, onMounted, onUnmounted } from 'vue';
import { CustomRouteRecordRaw } from '../router/types';

const productStore = useProductStore();
const drawer = ref(false);

// Get routes from the router
const router = useRouter();
const routes = computed(
  () =>
    router.options.routes.filter(
      (route) => route.name && route.path !== '/:catchAll(.*)*'
    ) as CustomRouteRecordRaw[]
);

const isOnline = ref(navigator.onLine);

const updateOnlineStatus = () => {
  isOnline.value = navigator.onLine;
};

onMounted(() => {
  window.addEventListener('online', updateOnlineStatus);
  window.addEventListener('offline', updateOnlineStatus);
});

onUnmounted(() => {
  window.removeEventListener('online', updateOnlineStatus);
  window.removeEventListener('offline', updateOnlineStatus);
});
</script>
