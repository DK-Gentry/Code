import axios from "./AxiosHepler";


export const getInvoice=async()=>{
var res = await axios.get("Customer/Invoice");
return res.data;
}

export const getOrderConfrmCarts=async()=>{
var res = await axios.get("OrderConfrm");
return res.data;
}

export const addOrder= async(data:any)=>{
    var res = await axios.post("OrderConfrm",data);
    return res.data;
    }