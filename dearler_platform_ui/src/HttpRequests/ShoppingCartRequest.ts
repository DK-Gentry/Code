import axios from "./AxiosHepler";

// 这里的customerNo在传递时候需要加{}，因为axios中第二个参数是个对象
export const getCarts = async()=>{
    var res  = await axios.get("ShoppingCart");
    return res.data;
 }

 export const updateCartSelect =async (cartGuids:string[],cartSelected:boolean,productNum:number) => {
    var res  = await axios.post("ShoppingCart/CartSelected",{cartGuids,cartSelected,productNum});
    return res.data;
 }