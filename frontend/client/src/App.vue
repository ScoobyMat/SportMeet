<script setup>

import Footer from './components/layout/Footer.vue';
import Navbar from './components/layout/Navbar.vue';
import './style.css';
</script>

<template>
    <Navbar />
    <router-view />
    <Footer />
</template>

<style scoped></style>
