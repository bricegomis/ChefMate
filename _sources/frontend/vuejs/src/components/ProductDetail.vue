<template>
  <q-dialog v-model="isOpen">
    <q-card class="my-card">
      <q-img src="https://cdn.quasar.dev/img/chicken-salad.jpg" />

      <q-card-section>
        <q-btn
          fab
          color="primary"
          icon="place"
          class="absolute"
          style="top: 0; right: 12px; transform: translateY(-50%)"
        />

        <div class="row no-wrap items-center">
          <div class="col text-h6 ellipsis">{{ product.name }}</div>
          <div
            class="col-auto text-grey text-caption q-pt-md row no-wrap items-center"
          >
            <q-icon name="euro" />
            {{ lowestPrice }}
          </div>
        </div>

        <q-rating v-model="stars" :max="5" size="32px" />
      </q-card-section>

      <q-card-section class="q-pt-none">
        <div class="text-subtitle1">{{ product.comments }}</div>
        <div class="text-caption text-grey">
          Small plates, salads & sandwiches in an intimate setting.
        </div>
      </q-card-section>

      <q-separator />

      <q-card-actions align="right">
        <q-btn v-close-popup flat color="primary" label="Reserve" />
        <q-btn v-close-popup flat color="primary" round icon="event" />
      </q-card-actions>
    </q-card>
  </q-dialog>
  <!-- <q-table
                :rows="props.row.prices"
                row-key="storeName"
                :columns="subColumns"
              /> const subColumns = [
  {
    name: 'store',
    label: 'Store',
    field: 'storeName',
    sortable: true,
    style: 'width: 80px',
  },
  {
    name: 'price',
    label: 'Price',
    field: 'price',
    sortable: true,
    style: 'width: 50px',
  },
  {
    name: 'date',
    label: 'Date',
    field: 'dateBuying',
    sortable: true,
    style: 'width: 100px',
  },
];-->
</template>

<script setup lang="ts">
import { Product } from 'src/models/Product';
import { computed, ref } from 'vue';

const props = defineProps<{
  isOpen: boolean;
  product: Product;
}>();

const isOpen = computed(() => {
  return props.isOpen;
});

const stars = ref(3);

// Calculate this on back
const lowestPrice = computed(() => {
  return props.product.prices
    ? Math.min(...props.product.prices.map((priceItem) => priceItem.price))
    : null;
});
</script>
