<template>
  <div class="private-chat-view">
    <ChatComponent chatType="private" :chatId="Number(friendId)" />
  </div>
</template>

<script setup>
import ChatComponent from "@/components/Chat/ChatComponent.vue";
import { useRoute } from "vue-router";

const route = useRoute();
const friendId = route.params.friendId;
</script>

<style scoped>
.private-chat-view {
  width: 100%;
  height: 100%;
}
</style>