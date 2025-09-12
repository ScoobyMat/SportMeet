<template>
  <div>
    <div v-if="!authStore.isAuthenticated" class="card">
      <img src="@/assets/image/logo.png" alt="Logo" class="navbar-logo" />
      <div class="app-description">
        <h2>Witaj na platformie SportMeet.</h2>
        <h4>Tutaj w łatwy sposób możesz znaleźć partnerów do wspólnych aktywności.</h4>
        <h4>Szukaj i zapisuj się na wydarzenia sportowe w twojej okolicy.</h4>
      </div>
    </div>

    <div v-else>
      <div class="card">
        <h2>Twoje nadchodzące wydarzenia</h2>
        <div v-if="events.length" class="carousel-wrapper">

          <button class="arrow left" @click="prevSlide" :disabled="isFirstSlide">
            <i class="bi bi-arrow-left-circle"></i>
          </button>

          <Carousel
            ref="carousel"
            :items-to-show="3"
            :wrap-around="false"
            :mouse-drag="true"
            :autoplay="false"
            :breakpoints="carouselBreakpoints"
            class="custom-carousel"
          >
            <Slide v-for="event in events" :key="event.id">
              <div class="eventBox" @click="goToEvent(event.id)">
                <img
                  :src="event.photoUrl || sportImages[event.sportType]"
                  alt="Event Sport Image"
                  class="card-img-top"
                />
                <h5>{{ event.eventName }}</h5>
                <p>Data: {{ event.date }}</p>
                <p>Godzina: {{ event.time }}</p>
                <p>Adres: {{ event.address }}</p>
                <p>Miasto: {{ event.city }}</p>
              </div>
            </Slide>
          </Carousel>

          <button class="arrow right" @click="nextSlide" :disabled="isLastSlide">
            <i class="bi bi-arrow-right-circle"></i>
          </button>
        </div>

        <div v-else>Nie masz nadchodzących wydarzeń</div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { computed, onMounted, ref, watch } from "vue";
import { useRouter } from "vue-router";
import { Carousel, Slide } from "vue3-carousel";
import "vue3-carousel/dist/carousel.css";

import { sportImages } from "@/constants/sports";
import EventService from "@/services/EventService";
import { useAuthStore } from "@/stores/auth";

const authStore = useAuthStore();
const router = useRouter();

const events = ref([]);

const carouselBreakpoints = {
  1200: { itemsToShow: 3 },
  768: { itemsToShow: 2 },
  0: { itemsToShow: 1 },
};

const carousel = ref(null);

const isFirstSlide = computed(() => {
  return carousel.value ? carousel.value.currentPage === 0 : true;
});
const isLastSlide = computed(() => {
  return carousel.value ? carousel.value.currentPage === carousel.value.pageCount - 1 : true;
});

const prevSlide = () => {
  if (carousel.value) carousel.value.prev();
};
const nextSlide = () => {
  if (carousel.value) carousel.value.next();
};

const goToEvent = (eventId) => {
  router.push(`/event/group/${eventId}`);
};

const fetchUpcomingEventsForUser = async () => {
  try {
    const currentUser = authStore.getUserId;
    if (currentUser) {
      const fetchedEvents = await EventService.getUpcomingEventsForUser(currentUser);
      events.value = fetchedEvents.filter(
        (ev) => ev.sportType && ev.eventName && ev.date
      );
    }
  } catch (error) {
    console.error("Failed to fetch events:", error);
  }
};

onMounted(() => {
  if (authStore.isAuthenticated) {
    fetchUpcomingEventsForUser();
  }
});

watch(
  () => authStore.isAuthenticated,
  (newVal, oldVal) => {
    if (newVal && !oldVal) {
      fetchUpcomingEvents();
    } else if (!newVal && oldVal) {
      events.value = [];
    }
  }
);
</script>

<style scoped>
.carousel-wrapper {
  position: relative;
  width: 100%;
  margin: 0 auto;
  display: flex;
  align-items: center;
  justify-content: center;
  overflow: hidden;
  padding: 1rem 0;
}

.arrow {
  position: absolute;
  top: 50%;
  transform: translateY(-50%);
  background: none;
  border: none;
  font-size: 2rem;
  cursor: pointer;
  z-index: 2;
  color: #333;
  transition: color 0.2s;
}

.arrow:hover {
  color: #555;
}

.arrow.left {
  left: 0.5rem;
}

.arrow.right {
  right: 0.5rem;
}

.arrow:disabled {
  opacity: 0.3;
  cursor: default;
}

.custom-carousel {
  width: 100%;
  padding: 0 3rem;
}

.eventBox {
  background: rgba(255, 255, 255, 0.25);
  border-radius: 15px;
  padding: 2rem;
  cursor: pointer;
  transition: background 0.2s;
  text-align: center;
  min-height: 35vh;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  margin: 0 auto;
}

.eventBox:hover {
  background: rgba(255, 255, 255, 0.35);
}

h2 {
  margin-top: 100px;
  text-align: center;
}

.navbar-logo {
  width: 200px;
  margin: 0 auto;
  display: block;
}

.app-description {
  text-align: center;
  margin: 20px 0;
}


@media (min-width: 1600px) {
  .eventBox {
    min-height: 45vh;
    min-width: 80%;
    padding: 3rem;
  }
  .card-img-top {
    width: 250px;
    height: 250px;
    margin-bottom: 20px;
    object-fit: cover;
  }
}

@media (max-width: 1199px) and (min-width: 768px) {
  .eventBox {
    min-height: 35vh;
    padding: 1.5rem;
  }
  .card-img-top {
    width: 180px;
    height: 180px;
    margin-bottom: 20px;
    object-fit: cover;
  }
}

@media (max-width: 767px) {
  .eventBox {
    min-height: 30vh;
    padding: 1rem;
  }
  .card-img-top {
    width: 150px;
    height: 150px;
    margin-bottom: 20px;
    object-fit: cover;
  }
}
</style>
