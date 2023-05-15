import axios from "./AxiosHepler";
import { IProductInputDto,IProductPropInputDto,IShoppingCartDto} from '../Interfaces/ProductList';

// data:IProductInputDto类型约束
export const getProduct =async (data:IProductInputDto) => {
    // Get方法不能像post那样直接传递参数，需要传递一个对象
    var res = await axios.get("Product",{params:data});
    return res.data;
}

export const getBelogType =async () => {
    // Get方法不能像post那样直接传递参数，需要传递一个对象
    var res = await axios.get("Product/BlongType");
    return res.data;
}

export const getType =async (belongTypeName:string) => {
    var res = await axios.get("Product/type?belongTypeName="+belongTypeName);
    return res.data;
}

export const getProp = async (data: IProductPropInputDto) => {
    var res = await axios.get("Product/props", { params: data })
    return res.data;
 }

 export const addCart =async (data:IShoppingCartDto) => {
    var res  = await axios.post("ShoppingCart",data);
    return res.data;
 }



