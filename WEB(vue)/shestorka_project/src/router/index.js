import { createRouter, createWebHistory } from 'vue-router'
import CatalogView from '../views/CatalogView.vue'
import AuthorizationView from '../views/AuthorizationView.vue'
import AddChangeItemView from '../views/AddChangeItemView.vue'
import OrderView from '../views/OrderView.vue'
import BasketOrderClientView from '../views/BasketOrderClientView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'catalog',
      component: CatalogView,
    },
    {
      path: '/authorization',
      name: 'authorization',
      component: AuthorizationView,
    },
    {
      path: '/add-change-item',
      name: 'addChangeItem',
      component: AddChangeItemView,
    },
    {
      path: '/order',
      name: 'orderWindow',
      component: OrderView,
    },
    {
      path: '/basket-order-client',
      name: 'basketOrderClient',
      component: BasketOrderClientView,
    },
  ],
})

export default router
  