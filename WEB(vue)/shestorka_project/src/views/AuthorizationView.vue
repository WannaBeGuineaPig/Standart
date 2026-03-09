<script setup>
import axios from "axios";
import { ref } from 'vue'
import { inject } from 'vue';

const $cookies = inject('$cookies');

if ($cookies.isKey('user')){
    window.location = 'http://localhost:5173/';
}

const email = ref('')
const password = ref('')

document.title = "ООО'Шестёрочка' - Заходи в акк";

async function Authorization(){
    let data = {
        "mail": email.value,
        "password": password.value 
    };
    await axios.post("http://localhost:5231/authorization", data)
    .then(response => { 
        alert("Нармас, вспомнил!)");
        $cookies.set("user", response.data);
        window.location = 'http://localhost:5173/'; 
    })
    .catch(error => {
        let errorBox = document.getElementById('errorBox');
        errorBox.style.display = 'flex';
        errorBox.textContent = `Да ну ёмаё: ${error.response.data} \n Даааа, ну ты даёшь:(`;
    });
}
</script>

<template>
    <form action="" method="post" class="auth-box" v-on:submit.prevent="Authorization">
        <textarea id="errorBox" readonly=""></textarea>
        <div class="input-box">
            <label for="mailInput">Накликайте почту: </label>
            <input class="mailInputClass" type="email" name="" id="mailInput" v-model="email" maxlength="300" required>
        </div>
        <div class="input-box">
            <label for="mailInput">Нагадайте пароль: </label>
            <input class="mailInputClass" type="password" name="" id="mailInput" v-model="password" maxlength="80" required>
        </div>
        <button type="submit" class="authBTN">Ну, ищи родимая</button>
    </form>
</template>

<style>
    
.auth-box{
    margin-top: 50px;
    display: flex;
    align-items: center;
    justify-content: space-evenly;
    background-color: gray;
    height: 75vh;
    width: 60vw;
    border: 4px dashed black;
    flex-direction: column;
}

.input-box label{
    font-size: 22px;
    color: white;
}
.mailInputClass{
    font-size: 18px;
    min-width: 350px;
}

.authBTN{
    font-size: 20px;
    width: 300px;
    height: 50px;
    border: 2px dashed black;
}

.authBTN:hover{
    opacity: 0.6;
}

.authBTN:active{
    opacity: 0.4;
}

#errorBox{
    display: none;
    text-align: center;
    resize: none;
    background-color: transparent;
    width: 100%;
    height: 50px;
    font-size: 18px;
    color: red;
    border: none;
    user-select: none;
}

#errorBox:hover{
    cursor: default;
}

#errorBox:focus{
    outline: none;
}

</style>