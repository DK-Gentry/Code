import { createStore } from 'vuex'

export default createStore({
    //定义属性
    state:{
        cartNum:10
    },
    //getters用于获取数据使用
    getters:{
        
        getCartNum(state){
            return state.cartNum;
        }
    },
    // mutations方法是用来设置属性值
    //必须同步执行，负责操作state中的数据增删改
    //store.commit("方法名",参数)这样来调用这里的参数
    mutations:{
        setCartNum(state,num){ 
            state.cartNum=num;
        }
    },
    //mutations中执行异步操作，
    //  会引起数据失效，提供action在外面包一层
    // 来专门进行异步操作
    //最终调用到的还是mutations
    //store.dispatch("方法名",参数);
    actions:{
        setCartNum(context,num){
            context.commit("setCartNum",num);
        }
    },
    //当项目非常庞大，需要管理的状态非常多的时候Vuex容许我们将store切分成多个模块(module)
    //每一个模块都拥有自己的state、getters、mutations 
    modules:{
        shoppingCart:{
            //区分模块所以要启用命名空间
            //调用时候就是 "shoppingCart/xxx(方法名)"
            namespaced:true,
            state:{  
            },
           
            getters:{
            },
            mutations:{
            },
           
            actions:{
            },
        }
    }
})