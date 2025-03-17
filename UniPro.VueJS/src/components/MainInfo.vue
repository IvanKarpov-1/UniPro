<script lang="ts">
import { defineComponent } from 'vue';

export default defineComponent({
  name: 'MainInfo',
  props: {
    userData: {
      type: Object,
      required: true,
    },
  },
});
</script>

<template>
    <div class="container main-info">
      <h2>Welcome! Your data: </h2>
      <p><strong>User ID:</strong> {{ userData.userId }}</p>
      <p><strong>First Name:</strong> {{ userData.firstName }}</p>
      <p><strong>Last Name:</strong> {{ userData.lastName }}</p>
      <p><strong>Patronymic:</strong> {{ userData.patronymic }}</p>
      <p><strong>Role:</strong> {{ userData.userRole }}</p>
    </div>
</template>
  
  <style scoped>
  .main-info {
    margin: 20px;
  }
  </style>
  