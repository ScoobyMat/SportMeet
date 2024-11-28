<template>
        <div class="row">
            <div class="col-md-8 card">
                <h2 class="text-center p-2">Szukaj wydarzeń:</h2>
                <hr>
                <form class="d-flex gap-2">
                    <div class="form-floating">
                        <input id="city" class="form-control form-control-sm" placeholder="City" type="text" />
                        <label for="city">Podaj lokalizacje</label>
                    </div>
                    <div class="form-floating">
                        <input id="startDate" class="form-control form-control-sm" placeholder="StartDate"
                            type="date" />
                        <label for="startDate">Data od:</label>
                    </div>
                    <div class="form-floating">
                        <input id="endDate" class="form-control form-control-sm" placeholder="EndDate" type="date" />
                        <label for="endDate">Data do:</label>
                    </div>
                    <button type="submit" class="btn btn-primary">Szukaj</button>
                </form>
                <div class="text-box mt-5">
                Tu pojawią się kafelki dla wydarzeń zwracanych z API
            </div>
            </div>

            <div class="col-md-4 card">
                <h2 class="text-center p-2">Mapa wydarzeń:</h2>
                <HereMap :center="center" />
            </div>
        </div>
</template>

<script>
import HereMap from '../components/Map/HereMap.vue';

export default {
    name: 'app',
    components: {
        HereMap
    },
    data() {
        return {
            center: {
                lat: 52.2296756,
                lng: 21.0122287
            }
        }
    }
}
</script>

<style scoped>
form {
    flex-wrap: wrap;
}

.card {
justify-content:flex-start;
}

.btn {
    width: 10vh;
}

</style>