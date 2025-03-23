<template>
  <q-layout view="lHh Lpr lFf" style="background-color: #2b2a33">
    <q-header elevated>
      <q-toolbar class="">
        <q-toolbar-title class="text-accent">
          Chef Mate
          <q-icon
            color="red"
            size="ml"
            name="wifi"
            v-if="!productStore.isOnline"
        /></q-toolbar-title>

        <div class="row text-h6 text-black justify-around items-center">
          <div>
            <q-avatar size="24px">
              <q-img src="/img/ChefMate.jpg" ratio="1" />
            </q-avatar>
          </div>
        </div>
      </q-toolbar>
    </q-header>

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

const productStore = useProductStore();
</script>
