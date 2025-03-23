<template>
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
        v-for="type in props.types"
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

<script setup lang="ts">
import { ref } from 'vue';

const filter = ref<string>('');
const props = defineProps<{
  types: {
    name: string;
    isSelected: boolean;
    nbOccurrence: number;
  }[];
}>();

function toggleType(
  selectedType: { name: string; isSelected: boolean },
  event: Event
) {
  const mouseEvent = event as MouseEvent;
  if (mouseEvent.ctrlKey || mouseEvent.metaKey) {
    selectedType.isSelected = !selectedType.isSelected;
  } else {
    props.types.forEach((type) => {
      type.isSelected = type == selectedType;
    });
  }
}
</script>
