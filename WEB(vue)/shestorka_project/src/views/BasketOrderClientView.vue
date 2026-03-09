<script setup>
import axios from "axios";
import { inject } from 'vue';
import OrderItem from "../components/OrderItem.vue"

const $cookies = inject('$cookies');

if (!$cookies.isKey('user') || $cookies.get('user').role != 'Авторизированный клиент'){
    window.location = 'http://localhost:5173/';
}

document.title = "ООО'Шестёрочка' - закезы"

</script>

<script>
export default {
    data() {
        return {
            listItems: [],
            listOrder: [],
            listAddress: [],
        }
    },
    methods: {
        CheckNumber(event){
            if(['e', 'E', '-', '.', ','].includes(event.key) ||
            (event.target.value.length > 8 && event.key in ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9']))
            {   
                event.preventDefault();
            }
        },
        CheckChange(item){
            let inputBox = document.getElementById(item.articulItem);
            if(item["amountInStorageItem"] < inputBox.value){
                inputBox.value = item["amountInStorageItem"];
            }
            let user = $cookies.get('user');
            if(inputBox.value.length == 0){
                inputBox.value = 0;
                delete user.basket[item.articulItem];
                $cookies.set('user', user);
                return;
            }
            
            user.basket[item.articulItem] = inputBox.value;
            $cookies.set('user', user);
        },
        DeleteItemOrdering(event){
            let user = $cookies.get('user');
            delete user.basket[event.target.id];
            $cookies.set('user', user);
            document.getElementById(event.target.id).value = 0;
            alert("Еда удалена(но есть шанс вернуть)!");
        },
        async Ordering(event){
            if(!confirm("Решился?")){
                return;
            }

            let data = {
                idUser: $cookies.get('user').id,
                dateOrdering: new Date().toISOString().split('T')[0],
                dateDelivery: document.getElementById('DateDelivery').value,
                status: 'Новый',
                addressPickUpPoint: document.getElementById('selectAddress').value,
                itemOrder: $cookies.get('user').basket
            };
            
            await axios.post('http://localhost:5231/new-order', data)
            .then(response => {
                let user = $cookies.get('user');
                delete user.basket;
                $cookies.set('user', user);
                alert("Заказ оформлен(сенкью вери матч)!");
                window.location = window.location;
            })
            .catch(error => {
                console.log(error)
                alert(error.response.data);
                return;
            });

        }
    },
    async created(){
        this.listOrder = await axios.get(`http://localhost:5231/order?idUser=${$cookies.get('user').id}`)
        .then(response => { return response.data })
        .catch(error => {
            console.log(error);
            return {};
        });

        this.listAddress = await axios.get('http://localhost:5231/address-pick-up-point')
        .then(response => { return response.data })
        .catch(error => {
            console.log(error);
            return {};
        });

        if (!("basket" in $cookies.get('user')) || Object.keys($cookies.get('user').basket).length <= 0){
            return;
        }
        this.listItems = await axios.post('http://localhost:5231/eat-ordering', Object.keys($cookies.get('user').basket))
        .then(response => { return response.data })
        .catch(error => {
            console.log(error);
            return {};
        });
    },
    mounted() {
        if(document.getElementById('DateDelivery') == undefined){
            return
        }
        document.getElementById('DateDelivery').valueAsDate = new Date();
        let today = new Date();
        document.getElementById('DateDelivery').setAttribute('min', today.toISOString().split('T')[0]);
        today.setFullYear(today.getFullYear() + 1);
        document.getElementById('DateDelivery').setAttribute('max', today.toISOString().split('T')[0]);
    },
}
</script>

<template>
    <div>
        <h2 class="titleText">Ваш неоформленный заказ</h2>
        <div v-if="'basket' in $cookies.get('user') && Object.keys($cookies.get('user').basket).length > 0">
            <p>Дата заказа: {{ new Date().toLocaleDateString() }}</p>
            <span>Дата доставки: <input type="date" name="" id="DateDelivery"></span>
            <p>Статус заказа: Новый</p>
            <span>Адрес пункта выдачи: 
                <select name="" id="selectAddress">
                    <option :value="item" v-for="item in listAddress">{{ item }}</option>
                </select>
            </span>
            <h3 class="titleText">Еда в корзине</h3>
            <div class="all-order-item">
                <div class="order-item" v-for="item in listItems">
                    <div class="item-image-box" v-on:click="ChangeItemWindow">
                        <img v-if="item.imageItem" :src="'data:image/png;base64,' + item.imageItem" alt="" class="item-image">
                        <img v-else src="@/assets/images/picture.png" alt="" class="item-image">
                    </div>
                    <div class="box-data">    
                        <p>Артикул еды: {{ item.articulItem }}</p>
                        <p>Тип еды: {{ item.typeItem }}</p>
                        <p>Цена еды(рублей/шт): {{ item.priceItem }}</p>
                        <p>Скидка еды(%/шт): {{ item.discountItem }}</p>
                        <p>Количество еды на просторах(шт): {{ item.amountInStorageItem }}</p>
                    </div>
                    <div class="box-action-item">
                        <span>Количество в заказе(бери, бери) <input :id="item.articulItem" type="number" min="1" max="99999999" v-on:keydown="CheckNumber" :value="$cookies.get('user').basket[item.articulItem]" v-on:input="CheckChange(item)"></span>
                        <button :id="item.articulItem" class="delete-item" v-on:click="DeleteItemOrdering">Снести еду(ну хоть не систему...)</button>
                    </div>
                </div>
            </div>
            <button class="complete-order" v-on:click="Ordering">Оформить заказ(а монет хватит?)</button>
        </div>
        <div v-else>
            <p style="font-size: 16px; text-align: center;">Ваша корзина пуста(грустно...)</p>
        </div>

        <h2 class="titleText">Ваши заказы(рэхмет за деньги)</h2>
        <div class="box-orders">
            <OrderItem :order="order" v-for="order in listOrder"/>
        </div>
    </div>
</template>

<style scoped>

.titleText{
    text-align: center;
    margin-bottom: 25px;
}

.all-order-item{
    border: 2px dashed black;
    padding: 1%;
    max-height: 600px;
    overflow: scroll;
}

.order-item{
    height: 100%;
    display: flex;
    justify-content: space-evenly;
    border: 1px solid black;
    margin-top: 10px;
}

.order-item *{
    font-size: 16px;
}

.item-image-box{
    width: 15%;
    height: 200px;
    padding: 5px;
    border-radius: 5px;
    display: flex;
    align-items: center;
    justify-content: center;
}

.item-image{
    max-width: 100%;
    max-height: 100%;
}

.box-data{
    display: flex;
    flex-direction: column;
    justify-content: center;
    width: 40%;
}

.box-action-item{
    display: flex;
    flex-direction: column;
    justify-content: center;
    width: 40%;
    gap: 20px 0;
}

.delete-item{
    height: 40px;
    width: 300px;
    border: 2px solid red;
    background-color: transparent;
}

.delete-item:hover, .complete-order:hover{
    opacity: 0.6;
}

.delete-item:active, .complete-order:active{
    opacity: 0.4;
}

.complete-order{
    margin-top: 20px;
    height: 50px;
    width: 300px;
    background-color: transparent;
    border: 3px solid green;
}

</style>