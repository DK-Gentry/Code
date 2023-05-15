<template>
  <div>
    <!-- 搜索面板 -->
    <div class="search-pad">
      <input
        type="text"
        name=""
        id=""
        v-model="searchText"
        @focus="searchFocus()"
        @blur="searchBlur()"
        @input="search"
      />
      <button v-show="isShowSearchBtn">搜索</button>
      <button v-show="!isShowSearchBtn" @click="showRight()">筛选</button>
    </div>
    <!-- 物品大类面板 -->
    <div class="system-pad">
      <div
        v-for="belogType in belogTypes"
        :key="belogType.belongTypeNanme"
        :class="[
          'system-item',
          { 'system-select': systemIndex == belogType.belongTypeNanme }
        ]"
        @click="selectSystemProduct(belogType.belongTypeNanme)"
      >
        <span>{{ belogType.belongTypeNanme }}</span>
      </div>
    </div>
    <!-- 物品展示列表 -->
    <div class="product-list">
      <ul>
        <li v-for="product in products" :key="product.id">
          <img :src="product.productPhoto?.productPhotoUrl" alt="" />
          <div>
            <p class="p-name">{{ product.productName }}</p>
            <p class="p-type">类别：{{ product.typeName }}</p>
            <p class="p-price">
              &yen;{{ tranPrice(product.productSale?.salePrice) }}/张
            </p>
            <p class="p-cart" @click="onAddCart(product.productNo,1)">
               <em></em>
               <i>x1</i>
            </p>
          </div>
        </li>
      </ul>
<!-- 左边物品类型面板 -->
      <div :class="['left-menu', { 'left-menu-show': isShowLeft }]">
        <div class="left-switch" @click="showLeft()">
          <img src="/img/R-C.jpg" alt="" />
        </div>
        <ul>
          <li
            v-for="productType in productTypes"
            :key="productType.id"
            :class="{ 'left-item-select': typeSelected == productType.typeNo }"
            @click="selectType(productType.typeNo)"
          >
            {{ productType.typeName }}
          </li>
        </ul>
      </div>
    </div>
    <!-- 右边物品属性面板 -->
    <div class="right-pad">
     <div class="list-pad">
        <ul class="f-type-list">
        <template v-for="(values,key) in productProps" :key="key">
            <li v-if="values.length>0 ">
          <p>{{ GetPropKey(key,1) }}</p>
          <ul class="f-item-list">
            <li  
            v-for="value in values" 
            :key="value" 
            @click="selectProp(GetPropKey(key,0),value)"
            >
            <span>{{ value }}</span></li>
            <!-- <li><span class="prop-select">胡桃色</span></li> -->
          </ul>
          <div class="clear-tag"></div>
        </li>
        </template>
</ul>
</div>
<div class="right-edit">
    <button @click="confirmFilter()" style="background-color:rgb(188,0,0); color:#fff">
          确定
        </button>
    <button @click="hideRight()">取消</button>
</div>
</div>
<div class="cover" v-show="isShowCover" @click="hideRight()"></div>
</div>
</template>

<script lang="ts">
    import {
        onMounted,
        reactive,
        toRefs
    } from 'vue'
    import {
        useRoute,
        useRouter
    } from 'vue-router'
    import {
        getProduct,
        getBelogType,
        getType,
        getProp,
        addCart
    } from '../HttpRequests/ProductListRequest'

    import {tranPrice} from '../utility/common'

    import {
        IProductInfo,
        IProduct,
        IBlongType,
        IProductType
    } from '../Interfaces/ProductList'
    export default {
        // 这个页面的运转原理:
        //1.首先数据获取是在一开始setup会执行初始化所有内容
        //2.onMounted在调用渲染在页面上，这时候我们在onMounted中固定会调用函数得到所有需要再页面上显示的所有函数值
        //3.当我们选择商品数据之类的操作时候我们会把该操作保存到url上然后刷新网页
        setup() {
            //router中包含所有的对象
            var router = useRouter();
            //route是一个跳转的路由对象，每一个路由都会有一个route对象，
            //是一个局部的对象，
            //可以获取对应的name,path,params,query等
            var route = useRoute();

            console.log('setUp');

            const pageController = reactive({
                isShowLeft: false,
                isShowCover: false,
                isShowSearchBtn: false
            })

            const productInfo: IProductInfo = reactive({
                systemIndex: '1',
                searchText: '',
                propSelect: {},
                products: [],
                belogTypes: [],
                productTypes: [],
                productProps: {},
                typeSelected: '',
                timer: 0,
                pageIndex: 1,
                /**
                 * 获取物品
                 */
                getPruducts: async(
                    belongTypeName: string,
                    productType: string | null,
                    searchText: string | null,
                    productProps: string | null,
                ) => {
                    console.log('getpruducts');
                    var products = (await getProduct({
                        searchText: searchText,
                        productType: productType,
                        belongTypeName: belongTypeName,
                        productProps: productProps,
                        sort: 'ProductName',
                        pageIndex: productInfo.pageIndex
                    })) as IProduct[]

                    products.forEach(
                        p => {
                            productInfo.products.push(p);
                        }
                    );
                },
                getBelongTypes: async() => {
                    productInfo.belogTypes = (await getBelogType()) as IBlongType[]
                },
                selectSystemProduct: async(belongTypeName: string) => {
                    productInfo.propSelect = {};
                    productInfo.typeSelected = ' ';
                    productInfo.searchText = '';
                    router.push(`/productList?belongType=${belongTypeName}`);
                },
                getType: async(belongTypeNanme: string) => {
                    productInfo.productTypes = (await getType(
                        belongTypeNanme
                    )) as IProductType[]
                },
                // 选择物品类型，选择物品类型时候可以清空搜索栏
                selectType: async(typeNo: string) => {
                    productInfo.propSelect = {};
                    productInfo.searchText = '';
                    if (productInfo.typeSelected == typeNo) {
                        productInfo.typeSelected = ''
                    } else {
                        productInfo.typeSelected = typeNo
                    }
                    setRouter();
                },
                //从后端获取物品属性
                getProps: async(belongTypeName: string, typeNo: string | null) => {
                    var res = await getProp({
                        belongTypeName: belongTypeName,
                        typeNo: typeNo
                    });
                    productInfo.productProps = res;
                },
                // // 保留两位小数的价格
                // tranPrice: (price: number) => {
                //     if (price == null) return "0.00"
                //     return price.toFixed(2).toString()
                // },
                // 获取物品属性种类的名称
                GetPropKey: (key: string, index: number) => {
                    return key.split("|")[index];
                },
                // 这里search没有调用getProducts如何得到搜索物品的？
                // 通过改变searchText的值然后刷新界面，onMounted会去调用
                search: () => {
                    clearTimeout(productInfo.timer);
                    productInfo.timer = setTimeout(async() => {
                        setRouter();
                    }, 1000);
                },
                // 选择属性
                selectProp: (propKey: string, propValue: string) => {
                    if (productInfo.propSelect[propKey] == propValue) {
                        productInfo.propSelect[propKey] = ''
                    } else {
                        productInfo.propSelect[propKey] = propValue
                    }
                },
                confirmFilter: () => {
                    setRouter();
                },
                onAddCart: async(productNo: string, productNum: number) => {
                    await addCart({
                        CustomerNo: localStorage["cno"],
                        productNo: productNo,
                        productNum: productNum
                    }
                    )
                }
            })

            // 将选中的物品属性转换成字符串
            const productPropToString = () => {
                productProps = '';
                for (const key in productInfo.propSelect) {
                    const value = productInfo.propSelect[key];
                    if (value != '') {
                        productProps += `${key}_${value}^`
                    }
                }
                // -3是减去我们最后的转移字符^
                productProps = productProps.substring(0, productProps.length - 1);
            }

            // 定义url的地址
            const setRouter = () => {
                var url = `/productList?belongType=${productInfo.systemIndex}`;
                // 根地址.包含根路由以及物品大类信息
                if (productInfo.systemIndex.trim() != '') {
                    // 拼接物品搜索信息
                    url += `&keywords=${productInfo.searchText}`;
                }
                // 拼接物品类型
                if (productInfo.typeSelected ?.trim() != '') {
                    url += `&type=${productInfo.typeSelected}`
                }
                //拼接属性
                productPropToString();
                if (productInfo.productProps != '') {
                    url += `&prop=${productProps}`
                }
                // router.push用于跳转页面,这里需要筛选物品时候保存我们的筛选选项
                //于是我们定义一个keyword来保存我们输入的选项，这里keyword是我们自定义的
                router.push(url);
            }

            const showLeft = () =>{
                pageController.isShowLeft = !pageController.isShowLeft
            }

            const searchFocus = () => {
                pageController.isShowSearchBtn = true
            }

            const searchBlur = () => {
                pageController.isShowSearchBtn = false
            }

            const showRight = () => {
                pageController.isShowCover = true
                var dom = document.querySelector('.right-pad') as HTMLElement
                dom.style.right = '0'
            }

            const hideRight = () => {
                pageController.isShowCover = false
                var dom = document.querySelector('.right-pad') as HTMLElement
                dom.style.right = '-85%'
            }

            let keywords: string | string[] = ''
            let systemIndex: string = ''
            let productType: string | null = ''
            let productProps: string = ''
                // 解析地址
            const resolutionAddress = () => {
                // 获得页面的路由
                productInfo.searchText = keywords = (route.query.keywords) as string;
                productInfo.systemIndex = systemIndex = (route.query.belongType as string)?? '1'
                productInfo.typeSelected = productType = (route.query.type as string)??''

                productProps = (route.query.prop as string)?? ''
                if (productProps != '') {
                    var arrayProducProps = productProps.split('^')
                    for (let i = 0; i < arrayProducProps.length; i++) {
                        const element = arrayProducProps[i]
                        productInfo.propSelect[element.split("_")[0]] = element.split("_")[1];
                    }
                }
            }

            // 监听页面的滚动事件
            const handleScroll = () => {
                // 获取当前页面
                var htmlDom = document.querySelector("html") as HTMLElement;
                //-获得当前整个页面的长度
                var htmlHeight = htmlDom.offsetHeight;
                // 获取滚动条距离顶部的距离
                var scrollTop = htmlDom.scrollTop;
                // 获得屏幕可视区域的高度
                var sreenHeight = document.documentElement.clientHeight;
                // 获取可视区域顶部到整个页面底部的距离
                var diffHeight = htmlHeight - scrollTop - scrollTop - sreenHeight;
                if (diffHeight <= 2 && scrollTop > 0) {
                    // 页数加1
                    onPageChange();
                }
            }

            const onPageChange = async() => {
                productInfo.pageIndex++;
                await productInfo.getPruducts(systemIndex,
                    productType,
                    keywords as string, productProps)
            }

            onMounted(async() => {
                // scroll是固定的表示监听鼠标的滚轮，滚动条
                // 当鼠标滚动时候触发handleScroll方法
                window.addEventListener('scroll', handleScroll)
                resolutionAddress()
                await productInfo.getType(systemIndex)
                await productInfo.getPruducts(systemIndex,
                    productType,
                    keywords as string,
                    productProps)
                await productInfo.getBelongTypes()
                await productInfo.getProps(systemIndex, productType)
            })

            return {
                ...toRefs(pageController),
                ...toRefs(productInfo),
                tranPrice,
                showLeft,
                searchFocus,
                searchBlur,
                showRight,
                hideRight
            }
        }
    }
</script>

<style lang="scss" scoped>
    .i-search:after {
        background-color: #b70101 !important;
    }
    
    .search-pad {
        z-index: 10;
        position: fixed;
        width: 100%;
        padding: 1px 20px;
        background-color: #f0f0f0;
        display: flex;
        input {
            height: 20px;
            box-sizing: border-box;
            border: 1px solid #ddd;
            border-radius: 3px;
            flex: 1;
            outline: none;
        }
        button {
            background-color: transparent;
            width: 56px;
            border: 0 none;
            font-size: 14px;
            font-weight: bold;
            color: #333;
            outline: none;
        }
    }
    
    .system-pad {
        z-index: 10;
        background-color: #fff;
        display: flex;
        position: fixed;
        top: 40px;
        width: 100%;
        .system-item {
            flex: 1;
            text-align: center;
            border-bottom: 1px #ddd solid;
            border-right: 1px transparent solid;
            border-left: 1px transparent solid;
            span {
                border: 0 none !important;
                background-color: #f0f2f5;
                margin: 6px 5px;
                font-size: 12px;
                font-weight: normal;
                text-align: center;
                border-radius: 4px;
                padding: 6px 0;
                display: block;
                height: 22px;
                line-height: 12px;
            }
        }
        .system-select {
            border-bottom: 1px transparent solid;
            border-right: 1px #ddd solid;
            border-left: 1px #ddd solid;
            span {
                background-color: transparent;
            }
        }
    }
    
    .product-list {
        padding-top: 75px;
        ul {
            background-color: #fff;
            li {
                list-style: none;
                height: 88px;
                padding-left: 108px;
                position: relative;
                img {
                    height: 66px;
                    width: 66px;
                    background-color: #ccc;
                    position: absolute;
                    left: 28px;
                    top: 11px;
                }
                div {
                    overflow: hidden;
                    padding: 10px 0;
                    border-bottom: 1px solid #f0f0f0;
                    padding-bottom: 12px;
                    text-align: left;
                    .p-name {
                        font-size: 13px;
                    }
                    .p-type {
                        font-size: 12px;
                        color: #666;
                        margin-top: 8px;
                    }
                    .p-price {
                        font-size: 13px;
                        color: #f23030;
                        margin-top: 8px;
                    }
                    .p-cart {
                        position: relative;
                        float: right;
                        background-color: red;
                        height: 20px;
                        width: 40px;
                        background-position: center;
                        background-size: 16px;
                        border-radius: 50px;
                        i {
                            position: absolute;
                            right: -20px;
                            font-size: 12px;
                            top: 3px;
                        }
                    }
                }
            }
        }
        .left-menu {
            position: fixed;
            height: calc(100% - 116px);
            left: -106px;
            width: 125px;
            background-color: #fff;
            top: 76px;
            border-radius: 0 18px 0 0;
            border: 1px solid #d7d7d7;
            overflow: hidden;
            transition: 0.5s;
            margin-bottom: 120px;
            .left-switch {
                width: 20px;
                background-color: #fff;
                position: absolute;
                right: 0;
                height: 100%;
                img {
                    position: absolute;
                    top: 42%;
                    left: 2px;
                    width: 20px;
                    transform: rotate(90deg);
                    transition: 0.5s;
                }
            }
            ul {
                position: absolute;
                height: 100%;
                width: 106px;
                background-color: #f0f0f0;
                overflow: auto;
                li {
                    width: 106px;
                    height: 50px;
                    text-align: center;
                    line-height: 50px;
                    border-bottom: 1px solid #d7d7d7;
                    padding: 0;
                    font-size: 12px;
                    color: #333;
                }
                li.left-item-select {
                    background-color: #fff;
                }
            }
        }
        .left-menu-show {
            left: 0;
            .left-switch {
                img {
                    transform: rotate(-90deg);
                }
            }
        }
    }
    
    .right-pad {
        position: fixed;
        /* right: -85%; */
        right: -85%;
        top: 0;
        width: 85%;
        height: 100%;
        background-color: #f7f7f7;
        z-index: 103;
        transition: 580ms;
        z-index: 101;
        overflow: auto;
        ul {
            list-style: none;
            overflow: hidden;
        }
        .list-pad {
            overflow: auto;
            height: 100%;
            padding-bottom: 40px;
            .f-type-list {
                overflow: hidden;
                >li {
                    padding: 10px;
                    background-color: #fff;
                    margin-bottom: 10px;
                    .f-item-list {
                        overflow: hidden;
                        display: flex;
                        flex-wrap: wrap;
                        li {
                            flex-basis: 33.3%;
                            span {
                                display: block;
                                margin-top: 10px;
                                margin-right: 10px;
                                background: #eee;
                                border: 1px solid #eee;
                                padding: 5px 0;
                                text-align: center;
                                border-radius: 6px;
                                font-size: 13px;
                            }
                            .prop-select {
                                border: 1px solid red;
                                background: #fff;
                                color: red;
                            }
                        }
                    }
                    p {
                        font-size: 14px;
                    }
                }
            }
        }
        .right-edit {
            position: absolute;
            bottom: 0;
            left: 0;
            width: 100%;
            button {
                float: left;
                height: 40px;
                width: 50%;
                line-height: 40px;
                text-align: center;
                border: 0px none;
            }
        }
    }
    
    .cover {
        position: fixed;
        z-index: 11;
        height: 100%;
        width: 100%;
        left: 0;
        top: 0;
        background-color: rgba(51, 51, 51, 0.36);
    }
</style>