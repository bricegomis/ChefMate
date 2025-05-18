<template>
  <div class="row">
    <div class="col-2">
      <q-input borderless dense debounce="300" v-model="searchFilter" filled>
        <template v-slot:append>
          <q-icon name="search" color="primary" />
        </template>
      </q-input>
    </div>
    <div class="col-10">
      <q-btn
        v-for="tag in props.tags"
        :key="tag.name"
        :outline="!tag.isSelected"
        square
        size="sm"
        class="q-ma-xs"
        color="green"
        @click="toggleType(tag, $event)"
        :text-color="tag.isSelected ? 'white' : 'black'"
      >
        <span class="">{{ tag.name }}</span>
        <span class="text-italic text-weight-thin"
          >&nbsp; ({{ tag.nbOccurrence }})</span
        >
      </q-btn>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue';

const props = defineProps<{
  search: string;
  tags: {
    name: string;
    isSelected: boolean;
    nbOccurrence: number;
  }[];
}>();

const searchFilter = computed({
  get: () => props.search,
  set: (value: string) => {
    emit('search', value);
  },
});

const emit = defineEmits<{
  (event: 'search', search: string): void;
}>();

function toggleType(
  selectedTag: { name: string; isSelected: boolean },
  event: Event
) {
  const mouseEvent = event as MouseEvent;
  if (mouseEvent.ctrlKey || mouseEvent.metaKey) {
    selectedTag.isSelected = !selectedTag.isSelected;
  } else {
    props.tags.forEach((tag) => {
      tag.isSelected = tag == selectedTag;
    });
  }
}
</script>
