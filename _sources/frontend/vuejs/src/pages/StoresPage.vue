<template>
  <q-page title="Stores">
    <q-card class="q-pa-md">
      <q-btn
        color="primary"
        icon-right="add"
        label="Create new store"
        no-caps
        @click="handleCreateStore"
      />
    </q-card>
    <q-list v-if="stores?.length > 0" bordered>
      <q-item
        v-for="store in stores"
        :key="store.id"
        clickable
        @click="handleOpenStore(store)"
      >
        <q-item-section>
          <q-item-label>{{ store.name }}</q-item-label>
        </q-item-section>
      </q-item>
    </q-list>
    <q-dialog v-model="popupDetailOpen">
      <q-card>
        <q-card-section>
          <q-input v-model="editingStore.name" label="Store Name" />
        </q-card-section>
        <q-card-actions align="right">
          <q-btn
            flat
            label="Cancel"
            color="primary"
            @click="popupDetailOpen = false"
          />
          <q-btn
            flat
            label="Save"
            color="primary"
            @click="saveStore(editingStore)"
          />
        </q-card-actions>
      </q-card>
    </q-dialog>
  </q-page>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue';
import { useQuasar } from 'quasar';
import { useStoreStore } from 'stores/store-store'; // Assume a store-store.ts exists
import { Store } from 'src/models/Store';

const storeStore = useStoreStore();
const $q = useQuasar();

const stores = computed(() => storeStore.stores);

const popupDetailOpen = ref(false);
const editingStore = ref<Store>({ id: '', name: '', image: null } as Store);

onMounted(() => {
  storeStore.fetchStores();
});

function handleCreateStore() {
  editingStore.value = { id: '', name: '', image: null };
  popupDetailOpen.value = true;
}

function handleOpenStore(store: Store) {
  editingStore.value = { ...store };
  popupDetailOpen.value = true;
}

async function saveStore(store: Store) {
  const success = await storeStore.saveStore(store);
  if (success) {
    $q.notify({ message: 'Store saved successfully', color: 'green' });
    popupDetailOpen.value = false;
    storeStore.fetchStores();
  } else {
    $q.notify({ message: 'Failed to save store', color: 'red' });
  }
}
</script>
