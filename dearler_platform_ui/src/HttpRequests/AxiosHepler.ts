import axios from "axios";
import { useRouter } from "vue-router";

axios.defaults.baseURL="http://localhost:5284/"

//axios的拦截器分为两类
//1.请求拦截器(向后端发送请求时候)
//2.响应拦截器(后端发生请求接收的时候)

//请求拦截器
axios.interceptors.request.use
(
    config=>
    {
        //判断是否存在token值，如果存在则在每个请求前给header中加上token
        console.log(localStorage["token"]);
        if (localStorage["token"]){
            if (config?.headers!=null)
            {
                //请求头加上token
                config.headers.Authorization ="Bearer " + localStorage["token"];
            }
        }
        return config;
    },
    error=>
    {
        //Promise表示异步的
        return Promise.reject(error);
    }
)

//响应拦截器
axios.interceptors.response.use
(
    response=>{
        return response;
    },
    error=>{
        switch(error.response.status){
            case 401:
                {
                    var route = useRouter();
                    //路由重定向
                }
        }
    }
)

export default axios;