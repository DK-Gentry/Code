<template>
    <div>
    <div class="login-pad">
            <h2>
               <img src="/img/icons/.net.webp" alt="">
                经销商平台
            </h2>
            <p>
                <input type="text" placeholder="用户账号" v-model="userNo">
            </p>
            <p>
                <input type="password" placeholder="密码" v-model="password">
            </p>
            <button @click="login" >
                 →
            </button>

            <!-- tag是标签转换,这里表示把router-link转换为button -->
            <!-- <router-link to="/Home">
                <button>
                    →
                </button>
            </router-link> -->
            
    </div>
    <div class="login-bottom">
        &copy;DK(全栈暂定)
    </div>
</div>
  </template>

<!-- lang="ts" 表示使用ts代码而不是js -->
<!-- 有什么区别？ts有类的概念 -->
<script lang="ts">
    import {
        reactive,
        toRefs
    } from 'vue'
    import {
        IloginInfo
    } from '../Interfaces/Login'
    import {
        login
    } from '../HttpRequests/LoginRequests'
    import {
        useRouter
    } from 'vue-router'
    export default {
        setup() {
            var router = useRouter();
            const loginInfo: IloginInfo = reactive({
                userNo: '',
                password: '',
                login: async() => {
                    var res = await login({
                        customerNo: loginInfo.userNo,
                        password: loginInfo.password
                    })
                    if (res != null) {
                        localStorage["cno"] = loginInfo.userNo;
                        localStorage["token"] = res;
                        router.push("/layoutMain");
                    }
                }
            })

            // toRefs也是响应式的抛出
            //...是解析数据用的
            return {...toRefs(loginInfo)
            }
        }
    }
</script>

<style lang="scss" scoped>
    .login-pad {
        text-align: center;
        width: 60%;
        margin: auto;
        margin-top: 26%;
        h2 {
            font-weight: normal;
            margin-bottom: 30px;
            img {
                display: inline-block;
                width: 36px;
                height: 36px;
                background: transparent;
                border-radius: 18px;
                vertical-align: -9px;
            }
        }
        p {
            width: 100%;
            margin-top: 20px;
            input {
                width: 100%;
                box-sizing: border-box;
                height: 36px;
                border-radius: 18px;
                border: 0 none;
                background-color: #f0f0f0;
                text-align: center;
            }
        }
        button {
            margin-top: 36px;
            width: 60px;
            height: 60px;
            border-radius: 30px;
            border: 0 none;
            background-color: rgb(79, 137, 245);
            color: #fff;
            font-size: 26px;
            font-weight: bold;
        }
    }
    
    .login-bottom {
        position: absolute;
        bottom: 10px;
        text-align: center;
        width: 100%;
        font-size: 14px;
    }
</style>