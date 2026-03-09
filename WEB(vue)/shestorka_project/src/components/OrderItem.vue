<script>
export default {
    props: ['order']
}
</script>

<template>
    <div class="box-order">
        <div class="box-main-data">
            <h3>Номер заказа: {{ this.order.idOrder }}</h3>
            <p>Статус заказа: {{ this.order.statusOrder }}</p>
            <p>Адрес пункта выдачи заказа: {{ this.order.addressPickUpPoint }}</p>
            <p>Дата заказа: {{ this.order.dateOrdering }}</p>
            <p>Код получения заказа: {{ this.order.codePickup }}</p>
        </div>
        <div class="box-date-delivery">
            <p>Дата доставки заказа:</p>
            <p>{{ this.order.dateDelivery }}</p>
        </div>
    </div>
</template>

<style scoped>
    
.box-order{
    display: flex;
    justify-content: space-between;
    width: 60vw;
    user-select: none;
    padding: 1%;
}

.box-main-data{
    width: 80%;
    border: 1px solid black;
    padding: 1%;
}

.box-date-delivery{
    width: 18%;
    border: 2px solid black;
    padding: 1%;
}

</style>