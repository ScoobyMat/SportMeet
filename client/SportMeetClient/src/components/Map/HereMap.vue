<template>
  <div ref="hereMap" :style="{ height: mapHeight, width: mapWidth }"></div>
</template>

<script setup>
import { defineEmits, defineProps, onMounted, ref, watch } from 'vue';

const props = defineProps({
  center: {
    type: Object,
    required: true,
  },
  markers: {
    type: Array,
    default: () => [],
  },
  mapHeight: {
    type: String,
    default: '100%',
  },
  mapWidth: {
    type: String,
    default: '100%',
  },
});

const emit = defineEmits();
const hereMap = ref(null);
let map, markerGroup;
const apikey = 'LlRhGrawpujpHpgpxdaTjmwDqM5LRxmhJqIruMeZJNk';

onMounted(() => {
  const platform = new window.H.service.Platform({
    apikey: apikey,
  });
  initializeHereMap(platform);
});

function initializeHereMap(platform) {
  const H = window.H;
  const mapContainer = hereMap.value;
  const maptypes = platform.createDefaultLayers();

  map = new H.Map(mapContainer, maptypes.vector.normal.map, {
    zoom: 12,
    center: props.center,
  });

  markerGroup = new H.map.Group();
  map.addObject(markerGroup);

  window.addEventListener('resize', () => map.getViewPort().resize());
  new H.mapevents.Behavior(new H.mapevents.MapEvents(map));
  H.ui.UI.createDefault(map, maptypes);

  updateMarkers();
}

function updateMarkers() {
  if (!map || !markerGroup) return;

  markerGroup.removeAll();
  props.markers.forEach(({ lat, lng, id }) => {
    const H = window.H;
    const marker = new H.map.Marker({ lat, lng });

    marker.addEventListener('tap', () => {
      emit('marker-click', id);
    });

    markerGroup.addObject(marker);
  });
}

watch(() => props.markers, updateMarkers, { deep: true });
watch(() => props.center, (newCenter) => {
  if (map) map.setCenter(newCenter);
});
</script>
