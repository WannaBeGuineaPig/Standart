<script setup>
import axios from "axios";
import { inject } from 'vue';
import OrderItem from "../components/OrderItem.vue"

const $cookies = inject('$cookies');

if (!$cookies.isKey('user') || $cookies.get('user').role != 'Администратор'){
    window.location = 'http://localhost:5173/';
}

document.title = "ООО'Шестёрочка' - Закезы"

</script>

<script>
export default {
    data() {
        return {
            orders: []
        }
    },
    async created(){
        this.orders = await axios.get("http://localhost:5231/order")
        .then(response => { return response.data })
        .catch(error => {
            console.log(error);
            return {};
        });
    }
}
</script>

<template>
    <div class="box-orders">
        <OrderItem :order="order" v-for="order in orders"/>
    </div>
</template>

<style>

.box-orders{
    display: flex;
    flex-direction: column;
    border: 2px solid black;
}
</style>