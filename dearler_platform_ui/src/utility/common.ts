
// 保留两位小数的价格
export const tranPrice=(price: number) => 
{
    if (price == null) return "0.00"
    return price.toFixed(2).toString()
}

export const tranTime=(time: string|undefined,reservedTime:boolean = true) => 
{
    if (reservedTime){
        return time?.replace("T"," ");
    }
    else{
        return time?.substring(0,time?.indexOf(""));
    }
}