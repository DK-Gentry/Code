import { ref } from "vue";

// 响应式
//就是需要更改的数据存在这里拿也通过这个拿，设置也通过这个设置
//这样不同的界面就共享一个数据
export const shoppingCartNum = ref(0);