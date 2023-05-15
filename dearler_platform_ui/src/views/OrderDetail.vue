<template>
    <div>
       <!-- <PageHead headText="订单详情"/> -->
        <div class="order-info">
            <p>
                <img src="/img/icons/Logo2.png" alt="">
                <b>订单编号：YZ789584858998</b>
            </p>
            <ul>
                <li v-for="orderDetail in order?.orderDetails" :key="orderDetail.id">
                    <img src="" alt="">
                    <p class="p-name">{{ orderDetail.productName }}</p>
                    <p class="p-price">&yen;{{ tranPrice(orderDetail.salePrice) }}</p>
                </li>
            </ul>
            <div :class="['order-progress',{'order-progress-show':isShowProgress}]">
                <div class="progress-node">
                    <el-steps :active="order?.orderProgress.stepSn" align-center v-show="isShowProgress">
                    <el-step title="下单" />
                    <el-step title="审核" />
                    <el-step title="通知发货" />
                    <el-step title="发货出库" />
                    <el-step title="确认收货" />
                    </el-steps>
                </div>
                <div class="show-progress" @click="showProgress()">
                    <img src="/img/dealerImgs/down.png" alt="" :class="{'arrow-up':isShowProgress}">
                </div>
            </div>
        </div>
        <div class="order-set">
            <p class="order-set-item"><span>下单时间</span>{{ tranTime(order?.inputDate)}}</p>
            <p class="order-set-item"><span>交货日期</span>{{ tranTime(order?.deliverDate,false)}}</p>
            <p class="order-submit">
                <button class="order-cancel">
                    取消订单
                </button>
                <button class="buy-again" @click="onBuyAgain()">
                    再次购买
                </button>
            </p>
            <p class="order-noti">
                <span>共83件，合计：<b>&yen;188.00</b></span>
            </p>
            <p class="order-set-item">备注：<span>{{ order?.remark }}</span></p>
            <p class="order-set-item"><span>开票人</span>{{ order?.invoiceNo }}</p>
            <p class="order-set-item"><span>发货仓库</span>成都仓库</p>
            <p class="order-set-item"><span>发货联系人</span>李云龙</p>
            <p class="order-set-item"><span>发货联系手机</span>18989898888</p>
        </div>
    </div>
    </template>
    
<script setup lang="ts">
import {tranPrice,tranTime} from '../utility/common'
import PageHead from '../components/PageHead.vue';
import {getOrderInfo} from '../HttpRequests/OrderDetail'
import {ref,reactive,toRefs, onMounted} from 'vue'
import { computed } from '@vue/reactivity';
            //所有const修饰的外部都能拿到
            const isShowProgress = ref(false);
            const progressStep = ref(3);

            const showProgress = ()=>{
                isShowProgress.value = !isShowProgress.value;
            }



            const orderInfo:any = reactive({
                order:{}
            })

            const onBuyAgain=()=>{
                
            }

            onMounted(async()=>{
                orderInfo.order = await getOrderInfo("");
            })

            const {order} = toRefs(orderInfo);
</script>

    <style lang="scss" scoped>
    .order-info {
        background-color: #fff;
        position: relative;
        padding-bottom: 7px;
        background: #fff url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAIAAAAAKBAMAAACOO0tGAAAABGdBTUEAALGPC/xhBQAAAAFzUkdCAK7OHOkAAAAnUExURf///4u16Oxtbezz/J3B7NDh9vvd3fSmprjS8vGNje57e/jDw/7w8IQ3dnAAAABSSURBVCjPY2CAAG4l7GADVJ4hUBArEIXJJ2HXrw6TZ8auX7ABKs+zCLsBE2AGmGDXLwKT58SuX+sATIEjdgOcYfJF2A1Qg8lz4PCBwWgQUiMIAWCaOFG2MdFRAAAAAElFTkSuQmCC) -7px bottom repeat-x;
    
        p {
            height: 50px;
            line-height: 50px;
            padding: 0 10px;
    
            img {
                width: 26px;
                height: 26px;
                vertical-align: middle;
            }
    
            b {
                font-size: 14px;
                margin-left: 10px;
                color: #333;
            }
        }
    
        ul {
            li:last-child {
                border-bottom: 0 none;
            }
    
            li {
                padding: 10px 16px 10px 100px;
                height: 81px;
                position: relative;
                border-bottom: 1px solid #ddd;
                background-color: #fff;
    
                >img {
                    width: 66px;
                    height: 66px;
                    position: absolute;
                    left: 10px;
                    background-color: #ccc;
                    top: 16px;
                }
    
                p {
                    height: 25px;
                    line-height: 25px;
                }
    
                .p-name {
                    font-size: 13px;
                    font-weight: bolder;
                }
    
                .p-remark {
                    color: #666;
                    font-size: 12px;
    
                    input {
                        color: #666;
                        font-size: 12px;
                        border: none 0;
                        outline: none;
                    }
                }
    
                .p-price {
                    color: crimson;
                    font-size: 14px;
                    font-weight: bolder;
                }
    
            }
        }
    
        .order-progress {
            background-color: #fff;
            padding: 0 10px;
            position: relative;
            height: 16px;
            text-align: center;
            overflow: hidden;
            transition: 0.5s;
    
            .progress-node {
                position: absolute;
                left: 0;
                right: 0;
                margin: auto;
                display: inline-block;
                height: 0px;
                border-top: 1px solid #e5e5e5;
    
                p {
                    display: inline-block;
                    position: relative;
                    padding: 22px 0;
    
                    span {
                        font-size: 12px;
                        position: absolute;
                        top: 0px;
                        left: -16px;
                        color: #999;
                        width: 50px;
                        text-align: center;
                    }
    
                    i {
                        display: inline-block;
                        font-size: 12px;
                        font-style: normal;
                        height: 16px;
                        width: 16px;
                        line-height: 16px;
                        text-align: center;
                        border: 2px solid #dbdbdb;
                        border-radius: 16px;
                        color: #fff;
                        vertical-align: middle;
                        margin-top: 2px;
                    }
    
                    i.node-select {
                        background-color: #3985ff;
                    }
    
                    b {
                        display: inline-block;
                        width: 50px;
                        height: 1px;
                        border-top: 2px solid #dbdbdb;
                        background-color: #e5e5e5;
                        border-bottom: 2px solid #dbdbdb;
                        margin: 0;
                    }
    
                    b.line-select {
                        background-color: #3985ff;
                    }
                }
            }
    
            .show-progress {
                position: absolute;
                height: 16px;
                width: 100%;
                left: 0;
                bottom: 0px;
                text-align: center;
                background-color: #fff;
    
                img {
                    background-color: transparent;
                    width: 20px;
                    height: 6px;
                    margin-bottom: 6px;
                    transition: 0.5s;
                    transform: rotate(0deg);
                }
    
                img.arrow-up {
                    transform: rotate(180deg);
                }
            }
        }
    
        .order-progress-show {
            height: 88px;
            .progress-node{
                height: 90px;
                .el-steps{
                    margin-top: 20px;
                }
            }
    
            .progress-node {
                height: 90px;
            }
        }
    }
    
    .order-set {
        background-color: #fff;
    
        p {
            padding: 6px 0;
            margin: 0 10px;
            border-bottom: 1px solid #ddd;
        }
    
        p.order-set-item {
            height: 26px;
            font-size: 13px;
            line-height: 26px;
    
            span {
                display: inline-block;
                width: 98px;
                color: #666;
            }
    
            input,
            select {
                padding: 0 3px;
                box-sizing: border-box;
                border: 0 none;
                background-color: #ddd;
                border-radius: 3px;
                color: #666;
                width: 136px;
                outline: none;
                height: 26px;
            }
        }
    
        p.order-noti {
            line-height: 2;
    
            span {
                font-size: 13px;
                font-weight: bolder;
                display: block;
            }
    
            span:nth-child(1) {
    
                color: #666;
    
                b {
                    color: crimson;
                    font-size: 14px;
                }
            }
    
            span:nth-child(2) {
                color: crimson;
            }
        }
    
        p.order-submit {
            display: flex;
            justify-content: space-between;
    
            button {
                flex-basis: 49%;
                box-sizing: border-box;
                margin: 10px 0;
                height: 36px;
                border: 0 none;
                background-color: #e93b3d;
                border-radius: 5px;
                color: #fff;
                font-size: 15px;
            }
    
            button.order-cancel {
                background-color: #f6f6f6;
                border: 1px solid #ddd;
                color: #333;
            }
    
            button.buy-again {
                background-color: #f19325;
                border: 1px solid #f19325;
                color: #fff;
            }
        }
    
        p:last-child {
            border-bottom: 0 none;
        }
    }
    </style>