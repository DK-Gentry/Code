import axios from "./AxiosHepler";

export const getOrderInfo=async(orderNo:string)=>{
    var res = await axios.get("OrderInfo",{params:{orderNo}});
    return res.data;
    }