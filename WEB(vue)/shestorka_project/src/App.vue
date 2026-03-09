<script setup>

import { inject } from 'vue';
const $cookies = inject('$cookies');
document.title = "ООО'Шестёрочка' - None"


function closeDescriptionData(){
    let descriptionDataModal = document.getElementById('descriton-data-modal');
    descriptionDataModal.style.visibility = "collapse";
    document.getElementsByClassName('header-box')[0].style.opacity = '1';
    document.getElementsByClassName('main-box')[0].style.opacity = '1';
    document.body.style.overflowY = 'visible';
}

function logOutClick(){
  if (!confirm("Че, всё?!"))
    return

  $cookies.remove('user');
  window.location = 'http://localhost:5173/';
}

</script>


<template>

  <div class="header-box">
    <router-link to="/" class="logo-box">
        <img src="@/assets/images/shestorka_logo2.png" alt="">
    </router-link>
    <div class="data_user">
      <p id="fio-user" v-if="$cookies.isKey('user')">{{ $cookies.get('user').lastName }} {{ $cookies.get('user').firstName }} {{ $cookies.get('user').midleName }}</p>
      <p id="role-user" v-if="$cookies.isKey('user')">{{ $cookies.get('user').role }}</p>
      <p id="role-user" v-else>Гость</p>
    </div>
    <div class="box-buttons">
      <router-link to="/order" class="login-button" v-if="$cookies.isKey('user') && $cookies.get('user').role == 'Администратор'">Заказы</router-link>
      <router-link to="/basket-order-client" class="login-button" style="width: 150px;" v-if="$cookies.isKey('user') && $cookies.get('user').role == 'Авторизированный клиент'">Кәрзин и заказ</router-link>
      <button class="logout-button" v-if="$cookies.isKey('user')" v-on:click="logOutClick">Выйти</button>
      <router-link to="/authorization" class="login-button" v-else>Войти</router-link>
    </div>
  </div>

  <div class="main-box">
    <router-view></router-view>
  </div>

  <div id="descriton-data-modal">
    <div>
        <h2 id="name-eat"></h2>
        <img class="close-description" src="@/assets/images/close_icon.png" alt="" v-on:click="closeDescriptionData">
    </div>
    <textarea id="description-textarea" readonly="true"></textarea>
  </div>

</template>
<style>
*{
  margin: 0;
  padding: 0;
  box-sizing: border-box;
  font-family: cursive;
}

body{
  overflow-x: hidden;

}

</style>
<style scoped>

.header-box{
  background-color: gray;
  height: 80px;
  width: 100vw;
  display: flex;
  justify-content: space-around;
  align-items: center;
}

.main-box{
  margin-top: 20px;
  max-width: 1366px;
  display: flex;
  justify-self: center;
}

.logo-box{
  width: 60px;
  height: 60px;
}

.logo-box:hover{
  opacity: 0.6;
}

.logo-box:active{
  opacity: 0.4;
}

.logo-box img{
  width: 100%;
  height: 100%;
}

#role-user, #fio-user{
  font-size: 20px;
}

.logout-button{
  border: 2px solid red;
  font-size: 16px;
  height: 40px;
  width: 120px;
}

.login-button{
  background-color: white;
  text-decoration: none;
  color: black;
  border: 2px dashed black;
  font-size: 16px;
  height: 40px;
  width: 120px;
  display: flex;
  align-items: center;
  justify-content: center;
}

.data_user{
  width: 60%;
  display: flex;
  justify-content: center;
  gap: 0 10%;
}



#descriton-data-modal{
    visibility: collapse;
    position: fixed;
    left: 20vw;
    top: 20vh;
    width: 60vw;
    height: 60vh;
    background-color: gray;
    padding: 10px;
    border: 4px dashed blue;
    display: flex;
    flex-direction: column;
    gap: 20px 0;
}

#descriton-data-modal div{
    margin: 0 20px;
    display: flex;
    align-items: center;
    justify-content: space-between;
}

#descriton-data-modal div h1{
  text-align: center;
}

#descriton-data-modal textarea{
    flex: 1;
    resize: none;
    border: 4px solid white;
    font-size: 18px;
}

.close-description{
    width: 24px;
    height: 24px;
    cursor: pointer;
}

.close-description:hover, .login-button:hover, .logout-button:hover{
    opacity: 0.6;
}

.close-description:active, .login-button:active, .logout-button:active{
    opacity: 0.4;
}

.box-buttons{
  display: flex;
  gap: 0 2vw;
  align-items: center;
}

</style>
