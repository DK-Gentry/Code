export interface IProductInputDto {
    searchText:string|null,
    productType: string | null,
    belongTypeName: string,
    productProps:string|null
    sort: string,
    pageIndex: number,
}

export interface IProductPropInputDto {
    belongTypeName: string,
    typeNo: string | null
}

export interface IProductInfo {
    searchText:string,
    systemIndex: string,
    propSelect:any,
    products: IProduct[],
    belogTypes: IBlongType[],
    typeSelected: string|null,
    productTypes: IProductType[],
    productProps:any,
    timer:number,
    pageIndex:number,
    // =>void 这个void就是函数体代表无返回值
    // =>number表示返回值为数字类型
    getPruducts: (
        belongTypeName: string,
        productType: string | null,
        searchText:string |null,
        productProps:string|null,
    ) => void;
    getBelongTypes:()=>void;
    getType: (belongTypeName: string) => void;
    getProps:(belongTypeName:string,typeNo:string| null)=>void
}
export interface IProduct {
    id: number;
    sysNo: string;
    productNo: string;
    productName: string;
    typeNo: string;
    typeName: string;
    productPp: string;
    productXh: string;
    productCz: string;
    productHb: string;
    productHd: string;
    productGy: string;
    productHs: string;
    productMc: string;
    productDj: string;
    productCd: string;
    productGg: string;
    productYs: string;
    unitNo: string;
    unitName: string;
    productNote: string;
    productBzgg: string;
    belongTypeNo: string;
    belongTypeName: string;
    productPhoto: IProductPhoto;
    productSale: IProductSale;
}

export interface IProductPhoto {
    id: number;
    sysNo: string;
    productNo: string;
    productPhotoUrl: string;
}

export interface IProductSale {
    id: number;
    sysNo: string;
    productNo: string;
    stockNo: string;
    salePrice: number;
}

export interface IBlongType {
    sysNo: string;
    belongTypeNanme: string;
}

export interface IProductType {
    typeNo: string;
    typeName: string;
}

export interface IShoppingCartDto{
    CustomerNo:string,
    productNo:string,
    productNum:number
}
