<template>
  <q-dialog v-model="isOpen">
    <q-card class="my-card">
      <div class="relative">
        <q-img :src="product.image || defaultImage" :ratio="16 / 9" width="500px">
          <div class="absolute-bottom text-subtitle2 text-center" v-if="lowestPrice">
            {{ lowestPrice }} € /
          </div>
          <q-btn
            @click="isOpen = false"
            color="primary"
            rounded
            class="absolute-top-right"
            size="sm"
            icon="close"
          />
        </q-img>
      </div>
      <q-tab-panels v-model="tab" animated swipeable>
        <q-tab-panel name="infos">
          <q-card-section>
            <div class="row no-wrap items-center">
              <div class="col">
                <q-rating v-model="stars" :max="5" size="16px" />
                <q-input color="purple-12" v-model="editingProduct.name" label="Name" />
                <q-select
                  v-model="editingProduct.usages"
                  :options="usages"
                  :multiple="true"
                  use-chips
                  label="Usage"
                />
                <q-select
                  v-model="editingProduct.tags"
                  :options="tags"
                  :multiple="true"
                  input-debounce="0"
                  use-input
                  use-chips
                  new-value-mode="add-unique"
                  label="Tags"
                />
              </div>
              <div
                class="col-auto text-grey text-caption q-pt-md row no-wrap items-center"
              ></div>
            </div>
          </q-card-section>

          <q-card-section class="q-pt-none">
            <div class="text-subtitle1"></div>
            <div class="text-caption text-grey">
              {{ product.description }}
            </div>
          </q-card-section>
          <q-card-section>
            <!-- <q-select
              filled
              v-model="editingProduct.type"
              dense
              use-input
              input-debounce="0"
              new-value-mode="add-unique"
              :options="types"
              label="Type"
              options-dense
            /> -->
          </q-card-section>
        </q-tab-panel>
        <q-tab-panel name="prices">
          <q-table
            :rows="product.prices || []"
            row-key="storeId"
            :columns="priceColumns"
          />
        </q-tab-panel>
      </q-tab-panels>

      <q-separator />

      <q-card-actions align="stretch">
        <!-- <q-tabs v-model="tab">
          <q-tab name="infos" icon="infos" label="Infos" />
          <q-tab name="prices" icon="history" label="Prices" />
        </q-tabs> -->
        <q-btn flat @click="tab = 'infos'" color="primary" label="Infos" />
        <q-btn flat @click="tab = 'prices'" color="primary" round label="prices" />
        <q-space />
        <q-btn flat icon="save" @click="SaveProduct" />
        <q-btn flat icon="delete" @click="DeleteProduct" />
      </q-card-actions>
    </q-card>
  </q-dialog>
</template>

<script setup lang="ts">
import { Product } from 'src/models/Product';
import { computed, ref } from 'vue';

const props = defineProps<{
  isOpen: boolean;
  product: Product;
  tags: string[];
  usages: string[];
}>();

const editingProduct = computed(() => props.product);

const emit = defineEmits<{
  (e: 'close', value: boolean): void;
  (e: 'save', value: Product): void;
  (e: 'delete', value: Product): void;
}>();

const isOpen = computed({
  get: () => props.isOpen,
  set: (value: boolean) => {
    emit('close', value);
  },
});

const stars = ref(3);
const tab = ref('infos');

// Calculate this on back
const lowestPrice = computed(() => {
  return props.product.prices && props.product.prices.length > 0
    ? Math.min(...props.product.prices.map((priceItem) => priceItem.price))
    : null;
});

function SaveProduct() {
  emit('save', editingProduct.value);
}

function DeleteProduct() {
  emit('delete', editingProduct.value);
}

const priceColumns = [
  {
    name: 'store',
    label: 'Store',
    field: 'storeId',
    sortable: true,
    style: 'width: 80px',
  },
  {
    name: 'price',
    label: 'Price',
    field: 'price',
    sortable: true,
    style: 'width: 50px',
    format: (val: unknown) => {
      return val ? `${val}€` : '';
    },
  },
  {
    name: 'date',
    label: 'Date',
    field: 'dateBuying',
    sortable: true,
    style: 'width: 100px',
  },
];

const defaultImage =
  'https://cdn.arihantbooks.com/assets/ProductImage/product-not-found.png';
</script>
