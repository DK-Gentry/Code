import { createRouter, createWebHashHistory, RouteRecordRaw } from 'vue-router'
import Login from '../views/Login.vue'


const routes: Array<RouteRecordRaw> = [
  {
    path: '/',
    name: 'Login',
    component: Login
  },
  {
    path: '/layoutMain',
    name: 'LayoutMain',
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: () => import(/* webpackChunkName: "about" */ '../views/LayoutMain.vue'),
    // redirect表示重定向，这里就是如果进入layoutMain界面就重定向到main
    // 重定向的界面必须要是下方已经定义过的，可以是在子路由中定义的，也可以是平级的正常路由
    redirect: '/main',
    // 子路由
    children: [
      {
        path: '/main',
        name: 'Main',
        component: () => import(/* webpackChunkName: "about" */ '../views/Main.vue'),
      },
      {
        path: '/productList',
        name: 'ProductList',
        component: () => import(/* webpackChunkName: "about" */ '../views/ProductList.vue'),
      },
      {
        path: '/shoppingCart',
        name: 'shoppingCart',
        component: () => import(/* webpackChunkName: "about" */ '../views/ShoppingCart.vue'),
      },
      {
        path: '/orderConfirm',
        name: 'orderConfirm',
        component: () => import(/* webpackChunkName: "about" */ '../views/OrderConfrim.vue'),
      },
      {
        path: '/orderDetail',
        name: 'orderDetail',
        component: () => import(/* webpackChunkName: "about" */ '../views/OrderDetail.vue'),
      }
    ]
  },
  {
    path: '/about',
    name: 'About',
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: () => import(/* webpackChunkName: "about" */ '../views/AboutView.vue')
  }
]

const router = createRouter({
  history: createWebHashHistory(),
  routes
})

export default router
