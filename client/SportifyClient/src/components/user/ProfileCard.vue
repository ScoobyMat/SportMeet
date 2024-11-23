<template>
    <img
  :src="user.photoUrl || '/assets/user.png'"
  alt="userPhoto"
  class="card-img-top"
/>
    <div class="card-body">
      <strong v-if="user.gender === 'Male'">
        Pan:
      </strong>
      <strong v-else="user.gender === 'Female'">
        Pani:
      </strong>
      <p>{{ user.firstName }} {{ user.lastName }}</p>

      <strong>Email:</strong>
      <p>{{ user.email }}</p>
      
      <strong>Wiek:</strong>
      <p>{{ (user.age) }}</p>

      <strong>Płeć:</strong>
      <p>{{ user.gender }}</p>

      <strong>Mieszka w:</strong>
      <p>{{ user.city }}, {{ user.country }}</p>

      <button class="btn btn-primary">
        Edytuj profil
      </button>
    </div>
</template>
  
<script setup>
const props = defineProps({
  user: {
    type: Object,
    required: true,
  },
});
</script>
  
  <style scoped>
  .card-img-top{
    padding: 25px;
    border-radius: 50%;
    max-width: 350px;
    max-height: 350px;
  }

  .card-body {
    text-align: center;
    font-size: larger;
  }
  </style>
  