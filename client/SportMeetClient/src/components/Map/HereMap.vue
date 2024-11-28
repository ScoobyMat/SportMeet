<template>
    <div id="map">
      <div class="map-card" style="height:700px;width:100%;" ref="hereMap"></div>
    </div>
  </template>

<script setup>
import { onMounted, ref } from 'vue';

const props = defineProps({
    center: {
        type: Object,
        required: true,
    },
});

const hereMap = ref(null);

const apikey = "LlRhGrawpujpHpgpxdaTjmwDqM5LRxmhJqIruMeZJNk";

onMounted(() => {
    const platform = new window.H.service.Platform({
        apikey: apikey,
    });

    initializeHereMap(platform);
});

function initializeHereMap(platform) {
    const mapContainer = hereMap.value;
    const H = window.H;

    const maptypes = platform.createDefaultLayers();

    const map = new H.Map(mapContainer, maptypes.vector.normal.map, {
        zoom: 12,
        center: props.center,
    });

    window.addEventListener("resize", () => map.getViewPort().resize());

    new H.mapevents.Behavior(new H.mapevents.MapEvents(map));
    H.ui.UI.createDefault(map, maptypes);
}
</script>

<style scoped>
#map {
  width: 100%;
  height: 100%;
}

.map-card {
  height: 700px;
  width: 100%;
  border-radius: 15px;
  border: double black;
  align-items: center;
  min-height: 40vh;
  min-width: 320px;
  padding: 0.5vh;
}
</style>