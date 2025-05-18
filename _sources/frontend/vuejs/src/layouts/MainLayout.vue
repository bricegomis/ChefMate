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
          <q-icon
            color="red"
            size="ml"
            name="wifi"
            v-if="!productStore.isOnline"
          />
        </q-toolbar-title>

        <!-- Authentication Button -->
        <q-btn
          flat
          round
          dense
          :icon="authStore.isAuthenticated ? 'logout' : 'login'"
          @click="authStore.setShowAuthPopup(true)"
        />

        <div class="row text-h6 text-black justify-around items-center">
          <div>
            <q-avatar size="24px">
              <q-img src="/img/ChefMate.jpg" ratio="1" />
            </q-avatar>
          </div>
        </div>
      </q-toolbar>
    </q-header>

    <!-- Authentication Popup -->
    <q-dialog v-model="showAuthPopup">
      <q-card>
        <q-card-section>
          <div class="text-h6">Authentication</div>
        </q-card-section>
        <q-card-section v-if="!authStore.isAuthenticated">
          <q-input v-model="username" label="Username" />
          <q-input v-model="password" label="Password" type="password" />
        </q-card-section>
        <q-card-section v-else>
          <div class="text-body1">You are already logged in.</div>
        </q-card-section>
        <q-card-actions align="right">
          <q-btn
            flat
            label="Cancel"
            @click="authStore.setShowAuthPopup(false)"
          />
          <q-btn
            v-if="!authStore.isAuthenticated"
            flat
            label="Login"
            color="primary"
            @click="authenticate"
          />
          <q-btn v-else flat label="Logout" color="negative" @click="logout" />
        </q-card-actions>
      </q-card>
    </q-dialog>

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
import { ref, computed } from 'vue';
import { CustomRouteRecordRaw } from '../router/types';
import { auth } from 'src/boot/axios';
import { useAuthStore } from 'src/stores/auth-store';

const productStore = useProductStore();
const drawer = ref(false);
const username = ref('');
const password = ref('');

const authStore = useAuthStore();
const showAuthPopup = computed(() => authStore.showAuthPopup);

// Get routes from the router
const router = useRouter();
const routes = computed(
  () =>
    router.options.routes.filter(
      (route) => route.name && route.path !== '/:catchAll(.*)*'
    ) as CustomRouteRecordRaw[]
);

function authenticate() {
  auth
    .login(username.value, password.value)
    .then(() => {
      console.log('Authentication successful');
      authStore.setShowAuthPopup(false);
    })
    .catch((error) => {
      console.error('Authentication failed:', error);
    });
}

function logout() {
  auth.logout();
  authStore.setShowAuthPopup(false);
}
</script>
