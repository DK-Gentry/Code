<template>
    <div>
        <div class="back-headed">
            <button></button>
            <h3>订单确认</h3>
        </div>
        <div class="order-info">
            <p>
                <!-- <img src="/img/icons/Logo2.png" alt=""> -->
                <b>订单信息</b>
            </p>
            <ul>
                <li v-for="cart in carts" key="cart.id">
                    <img src="" alt="">
                    <p class="p-name">{{ cart.productDto?.productName }}</p>
                    <p class="p-price">&yen;{{ cart.productDto.productSale?.salePrice }}</p>
                </li>
            </ul>
        </div>
        <div class="order-set">
            <p class="order-set-item">
                <span>交货日期</span> 
                <input type="date" name="" id="" v-model="deliveryDate">
            </p>
            <p class="order-set-item">
                <span>开票人</span>
                <select name="" id="" v-model="invoiceSelected">
                    <option :value="invoice.invoiceNo" v-for="invoice in invoices" :key="invoice.id">{{ invoice.invoiceNo }}</option>
                </select>
            </p>
            <p class="order-noti">
                <span>{{ getTotalNum() }}<b>&yen;{{ getTotalPrice() }}</b></span>
                <span>注：显示金额为成本金额，不含运费，实际结算价格，以审单后为准。</span>
            </p>
            <p class="order-submit">
                <button @click="onSubmitOrdder()">
                    提交订单
                </button>
            </p>
    
        </div>
    </div>
    </template>
    
    <script>

    import {reactive,toRefs,onMounted} from 'vue'
    import {getInvoice,getOrderConfrmCarts,addOrder} from '../HttpRequests/OrderConfimRequest'

    export default 
    {
        setup()
        {
            const orderConfrimInfo = reactive(
            {
                carts:[],
                invoices:[],
                remark:'',
                invoiceSelected:'',
                deliveryDate:"2020-01-23",
                //设置交货日期:当前时间往后推一天系统默认交期
                setdeliveryDate:()=>{
                    //创建时间对象
                    var date = new Date();
                    //获取当前年份
                    var year=date.getFullYear();
                    //获取月份，js中获取的月份从0开始，0月是1月
                    var orgMonth =  date.getMonth();
                    var month = orgMonth + 1 < 10
                                ?"0"+ (orgMonth+1):orgMonth+1;
                    //获取日期
                    //与月份信息一样，当前日期也是从0开始，使用时候需要+1
                    var orgDay = date.getDate();
                    var day = orgDay+1<10?"0"+ (orgDay+1) : orgDay+1;
                    //返回标准的适配input date的标签日期格式
                    orderConfrimInfo.deliveryDate = `${year}-${month}-${day}`;
                },

                //获取总价
                getTotalPrice:()=>{
                    var totalPrice = 0;
                    //循环购物车，获取总价
                    orderConfrimInfo.carts.forEach(c => {
                        totalPrice+=c.productNum + (c.productDto.productSale?.salePrice ?? 0);
                    });
                    return totalPrice;
                },

                getTotalNum:()=>{
                    var totalNum = 0;
                    orderConfrimInfo.carts.forEach(c => {
                        totalNum+=c.productNum;
                    });
                    return totalNum;
                },

                onSubmitOrdder:()=>{
                    addOrder({
                        remark:orderConfrimInfo.remark,
                        invoice:orderConfrimInfo.invoiceSelected,
                        deliveryDate:orderConfrimInfo.deliveryDate
                    });
                }
            })

        onMounted(async()=>{
            orderConfrimInfo.invoices = await getInvoice();
            orderConfrimInfo.carts = await getOrderConfrmCarts();
            //orderConfrimInfo.setdeliveryDate();
        })

        return {...toRefs(orderConfrimInfo)}
        }
    }
    </script>
    
    <style lang="scss" scoped>
    .back-headed {
        height: 46px;
        width: 100%;
        box-sizing: border-box;
        padding: 0 40px;
        position: relative;
        background-color: #f8f8f8;
    
        button {
            position: absolute;
            left: 12px;
            top: 12px;
            width: 22px;
            height: 22px;
            background-color: #aaa;
            border-radius: 22px;
            // background-image: url("/img/icons-png/back-white.png");
            border: 0 none;
            background-position: center;
            background-repeat: no-repeat;
            outline: none;
        }
    
        h3 {
            width: 100%;
            text-align: center;
            height: 45px;
            line-height: 45px;
            font-size: 15px;
            color: #333;
        }
    }
    
    .order-info {
        background-color: #fff;
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
            padding-bottom: 7px;
            // background: #fff url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAIAAAAAKBAMAAACOO0tGAAAABGdBTUEAALGPC/xhBQAAAAFzUkdCAK7OHOkAAAAnUExURf///4u16Oxtbezz/J3B7NDh9vvd3fSmprjS8vGNje57e/jDw/7w8IQ3dnAAAABSSURBVCjPY2CAAG4l7GADVJ4hUBArEIXJJ2HXrw6TZ8auX7ABKs+zCLsBE2AGmGDXLwKT58SuX+sATIEjdgOcYfJF2A1Qg8lz4PCBwWgQUiMIAWCaOFG2MdFRAAAAAElFTkSuQmCC) -7px bottom repeat-x;
    
            li {
                padding: 10px 16px 10px 100px;
                height: 91 px;
                position: relative;
                border-bottom: 1px solid #ddd;
                background-color: #fff;
    
                img {
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
    }
    
    .order-set {
        background-color: #fff;
    
        p {
            padding: 6px 0;
            margin: 0 10px;
            border-bottom: 1px solid #ddd;
        }
    
        p.order-set-item {
            height: 40px;
            font-size: 13px;
            line-height: 40px;
    
            span {
                display: inline-block;
                width: 80px;
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
            button {
                width: 100%;
                box-sizing: border-box;
                margin: 10px 0;
                height: 36px;
                border: 0 none;
                background-color: #e93b3d;
                border-radius: 5px;
                color: #fff;
                font-weight: bolder;
                font-size: 15px;
            }
        }
    
        p:last-child {
            border-bottom: 0 none;
        }
    }
    </style>