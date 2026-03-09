<script setup>
import axios from "axios";
import Item from "../components/Item.vue"
import { inject } from 'vue';
import { ref } from 'vue';

const $cookies = inject('$cookies');
document.title = "ООО'Шестёрочка' - Товаралог"

</script>

<script>

export default{
    methods: {
        async changeListItem(){
            this.listItem = await axios.get(`http://localhost:5231/eats?search=${this.searchText}&sorting=${this.sortingText}&filtering=${this.filteringText}`)
            .then(response => { return response.data })
            .catch(error => {
                console.log(error); 
                return {};
            });
            if (this.listItem.length == 0){
                document.getElementById("not-found-items").style.visibility = "visible";
            }
            else{
                document.getElementById("not-found-items").style.visibility = "collapse";
            }
        }
    },
    data(){
        return{
            listItem: {},
            listSupplier: {},
            searchText: ref(''),
            sortingText: ref('по возрастанию'),
            filteringText: ref('Все поставщики'),
        }
    },
    async created(){ 
        this.listItem = await axios.get("http://localhost:5231/eats")
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
    }
}

</script>

<template>
    <div class="content-box">

        <div class="work-item-box" v-if="$cookies.isKey('user')">
            <div class="search-box">
                <label for="search-input">Найдёшь?: </label>
                <input type="text" id="search-input" v-model="searchText" v-on:input="changeListItem">
            </div>
            <div class="search-box">
                <label for="sorting">Чё на складе творится: </label>
                <select name="" id="sorting" v-on:change="changeListItem" v-model="sortingText">
                    <option value="по возрастанию">по возрастанию</option>
                    <option value="по убыванию">по убыванию</option>
                </select>
            </div>
            <div class="search-box">
                <label for="filtering">Кто это привозит: </label>
                <select name="" id="filtering" v-on:change="changeListItem" v-model="filteringText">
                    <option value="Все поставщики">Все поставщики</option>
                    <option :value="item" v-for="(item, index) in listSupplier" :key="index">{{ item }}</option>
                </select>
            </div>
        </div>
        <p id="not-found-items">Не найдено брад!</p>
        <div class="list-items">
            <Item
            v-for="(item, index) in listItem" 
            :item="item"
            :index
            :roleUser="$cookies.isKey('user') ? $cookies.get('user').role : ''"/>
        </div>

        <div class="add-box" v-if="$cookies.isKey('user') && $cookies.get('user').role == 'Администратор'">
            <router-link to="/add-change-item" class="add-button">Добавить нннадо?</router-link>
        </div>
    </div>


</template>

<style scoped>

#not-found-items{
    font-size: 20px;
    text-align: center;
    visibility: collapse;
}

.list-items{
    border: 2px solid black;
    padding: 0 10px;
    display: flex;
    flex-direction: column;
    gap: 20px;
}

.work-item-box{
    display: flex;
    flex-direction: column;
    align-items: center;
    margin: 20px 0;
}

.search-box label{
    font-size: 18px;
}

#search-input, .search-box select{
    font-size: 14px;
}

.add-box{
    display: flex;
    align-items: center;
    justify-content: center;
    margin: 20px 0;
}

.add-button{
    width: 200px;
    height: 50px;
    font-size: 16px;
    border: 2px dashed black;
    background-color: white;
    display: flex;
    align-items: center;
    justify-content: center;
    color: black;
    text-decoration: none;
}

.add-button:hover{
    opacity: 0.6;
    cursor: pointer;
}

.add-button:active{
    opacity: 0.4;
}

</style>