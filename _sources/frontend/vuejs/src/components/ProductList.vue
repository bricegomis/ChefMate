<template>
  <div class="q-pa-md">
    <q-table
      class="my-sticky-header-column-table"
      :columns="allColumns"
      :rows="filteredProducts"
      row-key="id"
      separator="cell"
      flat
      bordered
      dense
      title="Produits"
      :rows-per-page-options="[20, 50, 100]"
    >
      <!-- TOP SLOT -->
      <template v-slot:top>
        <div class="row">
          <div class="col-2">
            <q-input borderless dense debounce="300" v-model="filter" filled>
              <template v-slot:append>
                <q-icon name="search" color="primary" />
              </template>
            </q-input>
          </div>
          <div class="col-10">
            <q-btn
              v-for="type in types"
              :key="type.name"
              :outline="!type.isSelected"
              square
              size="sm"
              class="q-ma-xs"
              color="green"
              @click="toggleType(type, $event)"
              :text-color="type.isSelected ? 'white' : 'black'"
            >
              <span class="">{{ type.name }}</span>
              <span class="text-italic text-weight-thin"
                >&nbsp; ({{ type.nbOccurrence }})</span
              >
            </q-btn>
          </div>
        </div>
      </template>
      <!-- BODY SLOT -->
      <template v-slot:body="props">
        <q-tr :props="props">
          <!-- NAME -->
          <q-td
            :props="props"
            key="name"
            @click="props.expand = !props.expand"
            class="cursor-pointer"
          >
            <span class="">{{ props.row.name }}</span
            ><br />
            <span class="text-italic text-caption">{{
              props.row.comments
            }}</span>
          </q-td>
          <q-td :props="props" key="actions">
            <q-btn
              flat
              dense
              size="sm"
              icon="delete"
              color="negative"
              @click="confirmDelete(props.row)"
            >
              <q-dialog v-model="confirmDeleteDialog" persistent>
                <q-card>
                  <q-card-section class="row items-center">
                    <q-avatar icon="alert" color="primary" text-color="white" />
                    <span class="q-ml-sm">Are you sure ?</span>
                  </q-card-section>

                  <q-card-actions align="right">
                    <q-btn
                      flat
                      label="Ok"
                      @click="deleteProduct"
                      color="primary"
                      v-close-popup
                    />
                    <q-btn
                      flat
                      label="Cancel"
                      @click="deletingProduct = null"
                      color="primary"
                      v-close-popup
                    />
                  </q-card-actions>
                </q-card>
              </q-dialog>
            </q-btn>
          </q-td>
          <q-td :props="props" key="labels">
            <q-chip
              v-for="(label, index) in props.row.labels"
              :key="index"
              color="primary"
              text-color="white"
              class="q-mr-sm"
            >
              {{ label }}
            </q-chip>
            <q-popup-edit v-model="props.row.labels" v-slot="scope">
              <div class="q-pa-sm">
                <q-chip
                  v-for="(label, index) in scope.value"
                  :key="index"
                  removable
                  @remove="removeLabel(props.row, label)"
                  class="q-mr-sm"
                >
                  {{ label }}
                </q-chip>
                <q-input
                  v-model="newLabel"
                  dense
                  placeholder="Add label"
                  @keyup.enter="addLabel(scope)"
                />
              </div>
            </q-popup-edit>
          </q-td>
          <q-td :props="props" key="type">
            <q-select
              filled
              v-model="props.row.type"
              :options="types"
              option-label="name"
              option-value="name"
              emit-value
            />
          </q-td>
          <q-td
            v-for="col in props.cols.slice(4)"
            :key="col.name"
            :props="props"
          >
            {{ col.value }}
          </q-td>
        </q-tr>
        <q-tr v-show="props.expand" :props="props">
          <q-td colspan="100%">
            <div class="text-left">
              <q-table
                :rows="props.row.prices"
                row-key="storeName"
                :columns="subColumns"
              />
            </div>
          </q-td>
        </q-tr>
      </template>
    </q-table>
  </div>
</template>

<script setup lang="ts">
import { Product } from 'src/models/Product';
import { useProductStore } from 'src/stores/product-store';
import { computed, reactive, ref } from 'vue';

const props = defineProps<{
  products: Product[];
}>();

const filter = ref<string>('');

const confirmDeleteDialog = ref(false);
const deletingProduct = ref<Product | null>(null);

function confirmDelete(product: Product) {
  deletingProduct.value = product;
  confirmDeleteDialog.value = true;
}

function deleteProduct() {
  if (!deletingProduct.value) return;
  const productStore = useProductStore();
  productStore.deleteProduct(deletingProduct.value);
}

const columns = [
  {
    name: 'name',
    label: 'Name',
    field: 'name',
    sortable: true,
    style: 'width: 250px',
  },
  {
    name: 'actions',
    label: '',
    field: '',
    sortable: false,
  },
  {
    name: 'labels',
    label: 'Labels',
    field: 'labels',
  },
  // { name: 'comments', label: 'Comments', field: 'comments', sortable: true },
  { name: 'type', label: 'Type', field: 'type', sortable: true },
  { name: 'tags', label: 'Tags', field: 'tags', sortable: true },
  {
    name: 'lowestPrice',
    label: ' BestPrice',
    field: 'lowestPrice',
    sortable: true,
    format: (val: unknown) => {
      return val ? `${val}€` : 'N/A';
    },
  },
  { name: 'unit', label: 'Unit', field: 'unit', sortable: true },
];
const subColumns = [
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
];

// Create the list of stores
const stores = Array.from(
  new Set(
    props.products.flatMap(
      (product) => product.prices?.map((price) => price.storeName) || []
    )
  )
);

// Création des colonnes dynamiquement pour chaque magasin
const storeColumns = stores.map((store) => ({
  name: store,
  label: store,
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  field: (row: any) => row.storePrices?.[store] ?? null,
  format: (val: unknown) => {
    return val ? `${val}€` : '';
  },
  sortable: true,
}));

// Ajout des colonnes des magasins aux colonnes existantes
const allColumns = [...columns, ...storeColumns];

const products = computed(() => {
  return props.products.map((product) => {
    const lowestPrice = product.prices
      ? Math.min(...product.prices.map((priceItem) => priceItem.price))
      : null;
    const storePrices = stores.reduce(
      (acc: { [key: string]: number | null }, store: string) => {
        const priceItem = product.prices?.find(
          (price) => price.storeName === store
        );
        acc[store] = priceItem ? priceItem.price : null;
        return acc;
      },
      {}
    );
    return {
      ...product,
      lowestPrice,
      storePrices,
    };
  });
});

const types = ref(
  Object.entries(
    products.value.reduce((acc: Record<string, number>, product) => {
      if (product.type) {
        acc[product.type] = (acc[product.type] || 0) + 1; // Count occurrences
      }
      return acc;
    }, {})
  )
    .sort((a, b) => b[1] - a[1]) // Sort by occurrences in descending order
    .map(([name, nbOccurrence]) =>
      reactive({
        name,
        isSelected: ref<boolean>(true),
        nbOccurrence,
      })
    )
);

// Function to toggle the selection of a type
function toggleType(
  selectedType: { name: string; isSelected: boolean },
  event: Event
) {
  const mouseEvent = event as MouseEvent;
  if (mouseEvent.ctrlKey || mouseEvent.metaKey) {
    selectedType.isSelected = !selectedType.isSelected;
  } else {
    types.value.forEach((type) => {
      type.isSelected = type == selectedType;
    });
  }
}

// Filter products based on selected types
const filteredProducts = computed(() => {
  const selectedTypes = types.value
    .filter((type) => type.isSelected)
    .map((type) => type.name);
  return selectedTypes.length
    ? products.value.filter(
        (product) => product.type && selectedTypes.includes(product.type)
      )
    : [];
});

const newLabel = ref<string>('');

function removeLabel(product: Product, label: string) {
  if (product.labels) {
    product.labels = product.labels.filter((l) => l !== label);
    useProductStore().updateProduct(product);
  }
}

// Fonction pour ajouter un label
function addLabel(scope: { value: string[] }) {
  if (newLabel.value.trim() && !scope.value.includes(newLabel.value.trim())) {
    scope.value.push(newLabel.value.trim());
    newLabel.value = '';
  }
}
</script>
<style lang="sass">
.my-sticky-header-column-table
  td:first-child
    background-color: $accent
    z-index: 1
    position: sticky
    left: 0
  tr th
    position: sticky
    /* higher than z-index for td below */
    z-index: 2
    /* bg color is important; just specify one */
    background: $primary
  thead tr:last-child th
    /* height of all previous header rows */
    top: 48px
    /* highest z-index */
    z-index: 3
  thead tr:first-child th
    top: 0
    z-index: 1
  tr:first-child th:first-child
    /* highest z-index */
    z-index: 3
  td:first-child, th:first-child
    position: sticky
    left: 0

  /* prevent scrolling behind sticky top row on focus */
  tbody
    /* height of all previous header rows */
    scroll-margin-top: 48px
.q-chip
  width: 100px
</style>
