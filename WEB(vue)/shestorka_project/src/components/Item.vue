<script setup>
import { inject } from 'vue';

</script>
<script>
export default{
    props: ['item', 'index', 'roleUser'],
    data() {
        return {
            $cookies: ''
        }
    },
    methods: {
        openDescriptionData() {
            let descriptionDataModal = document.getElementById('descriton-data-modal');
            document.getElementById('name-eat').textContent = `Описание '${this.item.type}' от ${this.item.manafacturer1}`;
            document.getElementById('description-textarea').value = this.item.description.replaceAll('\\n', '\n');
            descriptionDataModal.style.visibility = "visible";
            document.getElementsByClassName('header-box')[0].style.opacity = '0.6';
            document.getElementsByClassName('main-box')[0].style.opacity = '0.6';
            document.body.style.overflowY = 'hidden';
        },
        
        changeDescriptionText(text){
            if (text.length < 150)
                return text
            return text.replaceAll('\\n\\n', ' ').slice(0, 150) + '...';
        },

        ChangeItemWindow(){
            if (document.getElementById('role-user').textContent != 'Администратор')
                return
            window.location.href += `add-change-item?articul=${this.item.articul}`;
        },
        AddItemInBasket(){
            let user = this.$cookies.get('user');
            if (!('basket' in user)){
                user['basket'] = {};
            }
            let basket = user['basket'];
            let articul = this.item.articul;
            if (articul in basket){
                basket[articul] += 1;
            }
            else{
                basket[articul] = 1;
            }
            this.$cookies.set('user', user);
            if(this.item.amountInStorage == basket[articul]){
                document.getElementsByClassName('add-item-basket')[this.index].disabled = 'true';
            }
            alert("Еда добавлена(скоро пир на весь мир!)!");
        }
    },
    setup() {
        $cookies = inject('$cookies');
    },
    mounted(){
        let textdesc = document.getElementById("description-item").textContent;
        if (textdesc.length > 190){
            document.getElementById("description-item").textContent = textdesc.replaceAll('\\n\\n', ' ').slice(0, 190) + '...';
        }
        if (parseInt(this.item.discountPercent) > 14){
            document.getElementsByClassName('item-card')[this.index].style.borderColor = 'green';
        }
        if(parseInt(this.item.amountInStorage) == 0){
            document.getElementsByClassName('amountInStorage')[this.index].style.color = 'aqua';
            if(document.getElementsByClassName('add-item-basket').count > 0)
            document.getElementsByClassName('add-item-basket')[this.index].disabled = 'true';
    }
    let user = this.$cookies.get('user');
    if('basket' in user && this.item.articul in user['basket'] && user['basket'][this.item.articul] == this.item.amountInStorage){
        if(document.getElementsByClassName('add-item-basket').count > 0)
            document.getElementsByClassName('add-item-basket')[this.index].disabled = 'true';
        }
    },
    
}

</script>

<template>
    <div class="item-card" id="itemCard">
        <div class="item-image-box" v-on:click="ChangeItemWindow">
            <img v-if="item.image" :src="'data:image/png;base64,' + item.image" alt="" class="item-image">
            <img v-else src="@/assets/images/picture.png" alt="" class="item-image">
        </div>
        <div class="item-data">
            <p id="category" v-on:click="ChangeItemWindow">{{ item.category }} | {{ item.type }}</p>
            <p id="description-item" v-on:click="openDescriptionData">Описание полуфабриката: {{  changeDescriptionText(item.description) }}</p>
            <p>Производитель: {{ item.manafacturer1 }}</p>
            <p>Поставщик: {{ item.supplier1 }}</p>
            <div class="box">
                <div>
                    <p v-if="item.discountPercent == 0">Цена: {{ item.price }} руб.</p>
                    <div class="price-discount-box" v-else>
                        <p>Цена: </p>
                        <p>{{ item.price }} руб.</p>
                        <p>{{ item.price - item.price / 100 * item.discountPercent }} руб.</p>
                    </div>
                    <p class="amountInStorage">Количество на складе: {{ item.amountInStorage }} шт.</p>
                </div>
                <button class="add-item-basket" v-if="roleUser == 'Авторизированный клиент'" v-on:click="AddItemInBasket">Добавить в корзину</button>
            </div>
        </div>
        <div class="discount-box">
            <p>{{ item.discountPercent }}%</p>
        </div>
    </div>



</template>

<style scoped>

.item-card{
    border: 2px dashed black;
    padding: 10px;
    display: flex;
    align-items: center;
    justify-content: space-between;
}

.item-image-box:hover, #category:hover, .add-item-basket:not(:disabled):hover{
    opacity: 0.6;
    cursor: pointer;
}

.item-image-box:active, #category:active, .add-item-basket:not(:disabled):active{
    opacity: 0.4;
}

.item-image-box, .discount-box{
    width: 18%;
}
.item-image-box, .item-data, .discount-box{
    height: 200px;
    padding: 5px;
    border: 2px solid black;
    border-radius: 5px;
    display: flex;
    align-items: center;
    justify-content: center;
}

.item-data{
    flex-direction: column;
    width: 60%;
    gap: 5px 0;
}

.item-data p{
    text-indent: 10px;
    width: 100%;
    font-size: 14px;
}

.item-image{
    max-width: 100%;
    max-height: 100%;
}

.discount-box p{
    font-size: 24px;
}


#description-item{
    cursor: pointer;
    user-select: none;
}

#description-item:hover, .close-description:hover{
    opacity: 0.6;
}

#description-item:active, .close-description:active{
    opacity: 0.4;
}



.price-discount-box{
    width: 100%;
    display: flex;
    align-items: center;
}

.price-discount-box p{
    flex: 0;
    text-wrap: nowrap;
}

.price-discount-box p:nth-child(2){
    color: red;
    text-decoration: line-through;
    font-style: italic;
}
.price-discount-box p:nth-child(3){
    color: green;
}

.box{
    width: 100%;
    display: flex;
    justify-content: space-between;
}

.add-item-basket{
    width: 200px;
    height: 40px;
    border: 2px dashed green;
    background-color: whitesmoke;
}

</style>