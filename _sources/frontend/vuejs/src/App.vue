<template>
  <router-view />
  <q-ajax-bar />
</template>

<script setup lang="ts">
import { useProfileStore } from 'src/stores/profile-store';

const profileStore = useProfileStore();

defineOptions({
  name: 'App',
});

// Initialize token and fetch profile if valid
profileStore.initialize();
</script>
