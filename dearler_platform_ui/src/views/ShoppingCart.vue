<template>
    <div>
        <div class="cart-list">
            <ul>
                <li v-for="type in types" :key="type.typeNo">
                    <p>
                        <i 
                        :class="{'cart-select':type.typeSelected}" 
                        @click="onSelectType(type)"
                        >√</i>
                        <span>{{ transTypeWhenNull(type.typeName) }}</span>
                    </p>
                    <template v-for="cart in carts">
                        <div v-if="cart.productDto?.typeNo==type.typeNo" :key="cart.id">
                        <i 
                        :class="{'cart-select':cart.cartSelected}" 
                        @click="onSelectCart(cart)"
                        >√</i>
                        <img src="" alt="">
                        <p class="p-name">{{ cart.productDto?.productName }}</p>
                        <p class="p-price">{{ tranPrice(transPriceWhenNull(cart.productDto?.productSale?.salePrice)) }}</p>
                        <p class="p-num">
                            <span class="sub-num" @click="onSubNum(cart)">-</span>
                            <input 
                            type="text" 
                            name="" 
                            id="" 
                            :value="cart.productNum" 
                            @change="onChangeNum(cart)">
                            <span class="add0num" @click="onAddNum(cart)">+</span>
                            <b>块</b>
                        </p>
                    </div> 
                    </template>
                </li>
            </ul>
        </div>
<div class="total-pad">
    <i :class="{'cart-select': isAllSelected}">√</i>
    <span>全选</span>
    <span>
                合计：&yen; <b>{{totalPrice|price}}</b>
            </span>
    <button>确定下单</button>
</div>
</div>
</template>

<script>
    import {
        onMounted,
        reactive,
        toRefs,
        watch,
        findIndex,
        splice
    } from 'vue'

import{tranPrice} from '../utility/common'

import {shoppingCartNum} from '../store'

    import {
        getCarts,
        updateCartSelect
    } from '../HttpRequests/ShoppingCartRequest'

import {useStore} from 'vuex'

    export default {
        // mounted() {
        //     this.$store.comm
        //     this.$store.dispatch('setFootMenuIndexAsync', 2);
        // },    

        setup() {
            const store = useStore();
            const shoppingCartInfo = reactive({
                carts: [],
                types: [],
                totalPrice:0,
                isAllSelected:false,
                onAddNum(cart) {
                    cart.productNum++;
                    updateCartSelect([cart.cartGuid],
                    cart.cartSelected,
                    cart.productNum);
                },

                onSubNum(cart){
                    if (cart.productNum > 0) {
                        cart.productNum--;

                    updateCartSelect([cart.cartGuid],
                    cart.cartSelected,
                    cart.productNum);
                    }

                    if(res=="Remove"){
                        // 删除元素
                        var index = shoppingCartInfo.carts.findIndex(
                            m=>m.cartNo==cart.cartNo
                            );
                        shoppingCartInfo.carts.splice(index,1);
                    }
                },

                onChangeNum(cart) {
                    //通过点击事件获取当前控件的值
                    var currNum = event.target.value;
                    //判断当前值是个数字且大于0
                    if (!isNaN(currNum) && currNum > 0) {
                        //是就赋值给动态响应变量
                        cart.productNum = currNum;
                    } else {
                        //否则就用cart.productNum还原
                        event.target.value = cart.productNum;
                    }
                },
                //获取购物车信息
                onGetShoppingCarts: async() => {
                    var res = await getCarts();
                    shoppingCartInfo.carts = res.carts;
                    shoppingCartInfo.types = res.types;
                },
                //全选功能
                onSelectCart: cart => {
                    cart.cartSelected = !cart.cartSelected;
                    updateCartSelect([cart.cartGuid],cart.cartSelected,cart.productNum);
                     //shoppingCartInfo.checkTypeSelected();
                    },

                    //选择类型时候触发
                    onSelectType:(type)=>{
                        var cartGuids = [];
                        type.typeSelected = !type.typeSelected;
                        shoppingCartInfo.carts
                        .filter(
                            m=>m.productDto?.typeNo==type?.typeNo)
                            .forEach(m=>{
                                cartGuids.push(m.cartGuid);
                                m.cartSelected = type?.typeSelected;
                             })
                            updateCartSelect(cartGuids,type.typeSelected);
                            //shoppingCartInfo.checkAllSelected();
                    },

                    //检测选择类型时候触发
                    checkTypeSelected:()=>{
                    shoppingCartInfo.types.forEach(type=>{
                        var cartsOfType = shoppingCartInfo.carts.filter(
                            m=>m.productDto?.typeNo==type?.typeNo
                            );
                     if (cartsOfType.every(m=>m.cartSelected))
                     {
                        type.typeSelected=true
                     }
                    else
                     {
                        type.typeSelected = false
                      }
                    })
                    shoppingCartInfo.checkAllSelected();
                    },

                    //检测是否选中所有物品
                    checkAllSelected:()=>{
                        if (shoppingCartInfo.carts.every(m=>m.cartSelected)){
                            shoppingCartInfo.isAllSelected =true;
                        }else{
                            shoppingCartInfo.isAllSelected=false;
                        }
                    },

                    //计算总价
                    calctotalPrice:(carts)=>{
                        shoppingCartInfo.totalPrice=0
                        let currentCartNum = 0
                        carts[0]
                        .filter(c=>c.cartSelected==true)
                        .forEach(c=>{
                            var singlePrice=c.productDto?.productSale?.salePrice??0;
                               shoppingCartInfo.totalPrice += singlePrice * c.productNum;
                               currentCartNum += c.productNum;
                            })
                        //shoppingCartNum.value = currentCartNum;
                        store.dispatch("setCartNum",currentCartNum);
                    },

                transTypeWhenNull: (type) => type??"未分类产品",
                transPriceWhenNull: (price) => price ?? 0
            })

     /**
     * 对carts进行深度监听
     */
    watch(
      [() => shoppingCartInfo.carts],
      (newValue, oldValue) => {
        shoppingCartInfo.checkTypeSelected()
        shoppingCartInfo.calctotalPrice(newValue)
      },
      {
        deep: true // 是否深度监听，一般监听对象或者是数组，咱都需要深度监听
      }
    )

            //页面加载时候触发
            onMounted(async() => {
                await shoppingCartInfo.onGetShoppingCarts();
                shoppingCartInfo.checkAllSelected();
                shoppingCartInfo.checkTypeSelected();
            })

            return {
                ...toRefs(shoppingCartInfo),
                tranPrice
            }
        }
    }
</script>

<style lang="scss" scoped>
    .cart-list {
        text-align: left;
        ul {
            margin-bottom: 108px;
            li {
                background-color: #fff;
                margin-bottom: 12px;
                >p {
                    padding-left: 46px;
                    position: relative;
                    height: 46px;
                    border-bottom: 1px solid #ddd;
                    i {
                        border: 1px solid #a9a9a9;
                        width: 18px;
                        height: 18px;
                        line-height: 18px;
                        border-radius: 18px;
                        position: absolute;
                        left: 13px;
                        top: 13px;
                        text-align: center;
                        font-size: 12px;
                        color: #fff;
                        font-style: normal;
                    }
                    i.cart-select {
                        background-color: crimson;
                        border: 1px solid crimson;
                    }
                    span {
                        display: inline-block;
                        border-left: 3px solid crimson;
                        height: 28px;
                        margin: 9px 0;
                        padding-left: 8px;
                        line-height: 30px;
                    }
                }
                div {
                    padding-left: 46px;
                    position: relative;
                    height: 98px;
                    padding: 8px 14px 8px 148px;
                    i {
                        border: 1px solid #a9a9a9;
                        width: 18px;
                        height: 18px;
                        line-height: 18px;
                        border-radius: 18px;
                        position: absolute;
                        left: 13px;
                        top: 28px;
                        text-align: center;
                        font-size: 12px;
                        color: #fff;
                        font-style: normal;
                    }
                    i.cart-select {
                        background-color: crimson;
                        border: 1px solid crimson;
                    }
                    img {
                        width: 68px;
                        height: 68px;
                        background-color: #ccc;
                        position: absolute;
                        left: 58px;
                        ;
                        top: 20px;
                    }
                    p.p-name {
                        font-size: 13px;
                        margin-top: 10px;
                        height: 30px;
                        ;
                    }
                    p.p-price {
                        font-size: 13px;
                        height: 20px;
                        color: crimson;
                    }
                    p.p-num {
                        text-align: right;
                        padding-right: 20px;
                        span {
                            display: inline-block;
                            width: 18px;
                            height: 18px;
                            border: 1px solid crimson;
                            color: crimson;
                            border-radius: 9px;
                            text-align: center;
                            line-height: 18px;
                        }
                        input {
                            width: 28px;
                            border: none 0px;
                            outline: none;
                            text-align: center;
                        }
                        b {
                            font-weight: normal;
                            margin-left: 10px;
                            font-size: 13px;
                        }
                    }
                }
            }
        }
    }
    
    .total-pad {
        height: 58px;
        width: 100%;
        background-color: #383838;
        position: fixed;
        left: 0;
        bottom: 40px;
        i {
            display: inline-block;
            border: 1px solid #a9a9a9;
            width: 18px;
            height: 18px;
            line-height: 18px;
            border-radius: 18px;
            background-color: #fff;
            margin-left: 13px;
            margin-top: 20px;
            vertical-align: bottom;
            height: 18px;
            text-align: center;
            font-size: 12px;
            color: #fff;
            font-style: italic;
        }
        i.cart-select {
            background-color: crimson;
            border: 1px solid crimson;
        }
        span {
            color: #fff;
            margin-left: 6px;
            font-size: 13px;
            b {
                font-size: 15px;
            }
        }
        button {
            float: right;
            height: 58px;
            width: 120px;
            border: 0 none;
            background-color: #ddd;
            color: #aaa;
            font-size: 15px;
            font-weight: bold;
        }
    }
</style>