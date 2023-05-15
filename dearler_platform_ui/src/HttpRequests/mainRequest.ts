import axios from "./AxiosHepler";

//data:any数据类型约束
// export 可以理解为构造函数这里外面调用这类传递的是data返回的也是data
export const getCartsNum=async()=>{
    // axios.post的返回值是AxiosRequest类型所以数据是在data中
var res = await axios.get("ShoppingCart/num");
return res.data;
}