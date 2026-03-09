<script setup>
import axios from "axios";
import { inject } from 'vue';

const $cookies = inject('$cookies');

if (!$cookies.isKey('user') || $cookies.get('user').role != 'Администратор'){
    window.location = 'http://localhost:5173/';
}

document.title = "ООО'Шестёрочка' - окно добавления/редактирования"

</script>

<script>
export default {
    data() {
        return {
            item: {},
            listType: {},
            listSupplier: {},
            listManafacturer: {},
            listCategory: {},
            newImage: null
        }
    },
    methods: {
        CheckNumber(event){
            if(event.key === 'e' || event.key === 'E' || event.key === '-' || 
            (event.target.value.length > 8 && event.key in ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9']))
            {   
                event.preventDefault();
            }
        },
        CheckDiscount(event){
            if(event.key === 'e' || event.key === 'E' || event.key === '-' || event.key === ',' || event.key === '.'|| (event.key in ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9'] && parseFloat(event.target.value + event.key) > 99)|| (event.target.value.length > 5 && event.key in ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9']))
            {   
                event.preventDefault();
            }
        },
        async ChangeImage(event){
            document.getElementById("imageItem").src = URL.createObjectURL(event.target.files[0]);
            this.newImage = event.target.files[0];
        },
        CheckError(){
            let lstEnterField = {
                'priceEat' : 'Цена', 
                'discountEat' : 'Скидка', 
                'amountInStorageEat' : 'Количества на складе', 
                'descriptionEat' : 'Описание' 
            };
            for (let enter in lstEnterField){
                let enterField = document.getElementById(enter);
                if(enterField.value.length == 0){
                    alert(`Бротан, ты поле '${lstEnterField[enter]}' забыл!`);
                    enterField.focus();
                    return false;
                }
            }
            return true;
        },
        async CreateDataDict(){
            if (this.newImage != null){
                this.newImage = await new Promise((resolve, reject) => {
                    const reader = new FileReader();
                    reader.readAsDataURL(this.newImage);
                    reader.onload = () => resolve(reader.result.replace(/^data:.+;base64,/, ''));
                    reader.onerror = error => reject(error);
                });
            }
            let data = {
                "type": document.getElementById('typeEat').value,
                "unitOfMeasurement": document.getElementById('unitOfMeasureEat').value,
                "price": document.getElementById('priceEat').value,
                "supplier": document.getElementById('supplierEat').value,
                "manafacturer": document.getElementById('manafacturerEat').value,
                "category": document.getElementById('categoryEat').value,
                "discountPercent": document.getElementById('discountEat').value,
                "amountInStorage": document.getElementById('amountInStorageEat').value,
                "description": document.getElementById('descriptionEat').value,
                "image": this.newImage
            };
            
            return data
        },
        async AddItem(event){
            if (!this.CheckError()) 
                return
            let data = await this.CreateDataDict();
            await axios.post(`http://localhost:5231/add-eat`, data)
            .then(response => { 
                console.log(response);
                alert(response.data);
                window.location.href = window.location.href.split('add-change-item')[0];
            })
            .catch(error => {
                console.log(error);
                alert(error.response.data);
            });
        },
        async ChangeItem(event){
            if (!this.CheckError()) 
                return
            let data = await this.CreateDataDict();
            data['articul'] = this.$route.query.articul;
            await axios.put(`http://localhost:5231/change-eat`, data)
            .then(response => { 
                console.log(response);
                alert(response.data);
                window.location.href = window.location.href.split('add-change-item')[0];
            })
            .catch(error => {
                console.log(error);
                alert(error.response.data);
            });

        },
        async DeleteItem(){
            await axios.delete(`http://localhost:5231/delete-eat?articul=${this.$route.query.articul}`)
            .then(response => { 
                alert(response.data);
                window.location.href = window.location.href.split('add-change-item')[0];
             })
            .catch(error => {
                alert(error.response.data);
            });
        },
    },
    async created() {
        if (this.$route.query.articul){
            this.item = await axios.get(`http://localhost:5231/item-on-articul?articul=${this.$route.query.articul}`)
            .then(response => { return response.data })
            .catch(error => {
                alert(error);
                window.location.href = window.location.href.split('add-change-item')[0]
                return {};
            });
        }
        document.title = "ООО'Шестёрочка' - " + (this.articul == null ? "добавить товар" : "редактировать товар");

        this.listType = await axios.get("http://localhost:5231/type")
        .then(response => { return response.data })
        .catch(error => {
            console.log(error);
            return {};
        });
        this.listSupplier = await axios.get("http://localhost:5231/supplier")
        .then(response => { return response.data })
        .catch(error => {
            console.log(error);
            return {};
        });
        this.listManafacturer = await axios.get("http://localhost:5231/manafacturer")
        .then(response => { return response.data })
        .catch(error => {
            console.log(error);
            return {};
        });
        this.listCategory = await axios.get("http://localhost:5231/category")
        .then(response => { return response.data })
        .catch(error => {
            console.log(error);
            return {};
        });

    },
}

</script>

<template>
    <div class="add-change-item-box">
        <div class="box-image">
            <img id="imageItem" v-if="item.image" :src="'data:image/png;base64,' + item.image" alt="" class="item-image">
            <img id="imageItem" v-else src="@/assets/images/picture.png" alt="" class="item-image">
        </div>

        <div class="box-change">
            <label for="changeImageItem">Ну ка давай фото: </label>
            <input type="file" name="" id="changeImageItem" accept="image/*" v-on:change="ChangeImage">
        </div>

        <div class="box-change">
            <label for="" v-if="$route.query.articul != null">Кодяра: {{ $route.query.articul }}</label>
        </div>

        <div class="box-change">
            <label for="typeEat">Тупе: </label>
            <select name="" id="typeEat">
                <option :value="item" v-for="(item, index) in listType" :key="index" :selected="item.type == item">{{ item }}</option>
            </select>
        </div>

        <div class="box-change">
            <label for="unitOfMeasureEat">Единица изм.. чего-то там: </label>
            <select name="" id="unitOfMeasureEat">
                <option value="шт.">шт.</option>
            </select>
        </div>

        <div class="box-change">
            <label for="priceEat">Это тебе станет в такую копеечку: </label>
            <input type="number" name="" id="priceEat" v-on:keydown="CheckNumber" min="0" max="100000000" :value="item.price">
        </div> 

        <div class="box-change">
            <label for="supplierEat">Привизи брад по-братски: </label>
            <select name="" id="supplierEat">
                <option :value="item" v-for="(item, index) in listSupplier" :key="index" :selected="item.supplier1 == item">{{ item }}</option>
            </select>
        </div>

        <div class="box-change">
            <label for="manafacturerEat">Кто накалдует хавчик: </label>
            <select name="" id="manafacturerEat">
                <option :value="item" v-for="(item, index) in listManafacturer" :key="index" :selected="item.manafacturer1 == item">{{ item }}</option>
            </select>
        </div>

        <div class="box-change">
            <label for="categoryEat">Котигария: </label>
            <select name="" id="categoryEat">
                <option :value="item" v-for="(item, index) in listCategory" :key="index" :selected="item.category == item">{{ item }}</option>
            </select>
        </div>

        <div class="box-change">
            <label for="discountEat">Уступи пж: </label>
            <input type="number" name="" id="discountEat" v-on:keydown="CheckDiscount" min="0" max="99" :value="item.discountPercent">
        </div>

        <div class="box-change">
            <label for="amountInStorageEat">Сколько надо: </label>
            <input type="number" name="" id="amountInStorageEat" v-on:keydown="CheckNumber" min="0" max="100000000" :value="item.amountInStorage">
        </div>
        
        <div class="box-change">
            <label for="descriptionEat">Опиши этого фунтика: </label>
            <textarea type="text" name="" id="descriptionEat" maxlength="1500">{{ item.description }}</textarea>
        </div>

        <div class="box-button-action">
            <button class="add-change-btn" v-on:click="$route.query.articul == null ? AddItem() : ChangeItem()">{{ $route.query.articul == null ? "Добавить товар(зачем...)" : "Редактировать товар(для чего...)" }}</button>
            
            <button class="delete-btn" v-on:click="DeleteItem()" v-if="$route.query.articul">Снести это хочу</button>
        </div>
            
    </div>
</template>

<style scoped>

.add-change-item-box{
    margin: 40px 0;
    display: flex;
    flex-direction: column;
    align-items: center;
    gap: 2vh 0;
}


.box-image{
    max-width: 250px;
    max-height: 250px;
    display: flex;
    align-items: center;
    justify-content: center;
}

.box-image img{
    width: 100%;
    height: 100%;
}

.box-change{
    display: flex;
    align-items: center;
    justify-content: center;
    gap: 0 1vw;
    font-size: 18px;
}

.box-change select, .box-change input{
    font-size: 16px;
    min-width: 200px;
}

#descriptionEat{
    resize: none;
    width: 400px;
    height: 200px;
}

.add-change-btn{
    width: 300px;
    height: 50px;
    font-size: 16px;
    background-color: white;
    border: 2px dashed black;
}

.add-change-btn:hover, .delete-btn:hover{
    opacity: 0.6;
    cursor: pointer;
}

.add-change-btn:active, .delete-btn:active{
    opacity: 0.4;
}

.delete-btn{
    border: 2px solid red;
    width: 250px;
    height: 50px;
    font-size: 16px;
    background-color: white;
    margin-left: 50px;
}

</style>