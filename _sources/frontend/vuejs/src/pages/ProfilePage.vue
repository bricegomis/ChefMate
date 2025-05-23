<template>
  <q-card class="auth-page" v-if="!profileStore.isAuthenticated()">
    <h1>Authentication</h1>
    <form @submit.prevent="login">
      <div>
        <label for="email">Email:</label>
        <input v-model="email" type="email" id="email" required />
      </div>
      <div>
        <label for="password">Password:</label>
        <input v-model="password" type="password" id="password" required />
      </div>
      <button type="submit">Login</button>
    </form>
    <p v-if="error" class="error">{{ error }}</p>
  </q-card>
  <q-card v-if="profileStore.isAuthenticated()">
    <q-btn flat round dense icon="logout" @click="profileStore.logout()" />
  </q-card>
</template>

<script setup>
import { useProfileStore } from 'src/stores/profile-store';
import { ref } from 'vue';
import { useRouter } from 'vue-router';

const email = ref('');
const password = ref('');
const error = ref('');
const router = useRouter();
const profileStore = useProfileStore();

async function login() {
  try {
    await profileStore.login(email.value, password.value);
    router.push('/');
  } catch (err) {
    error.value = 'Invalid credentials. Please try again.';
  }
}
</script>
