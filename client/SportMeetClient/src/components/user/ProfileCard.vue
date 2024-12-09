<template>
    <img
  :src="photoUrl || userImage"
  alt="userPhoto"
  class="card-img-top"
/>
    <div class="card-body">
      <strong v-if="user.gender === 'Mężczyzna'">
        Pan:
      </strong>
      <strong v-else="user.gender === 'Kobieta'">
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
    </div>
</template>
  
<script setup>
import userImage from '@/assets/image/user.png';

const photoUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTj-mz--RrBmFB_O1s36EQYyQuc2IunZgZhBg&s";

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
    max-width: 80%;
    max-height: 450px;
  }

  .card-body {
    text-align: center;
    font-size: larger;
  }
  </style>
  