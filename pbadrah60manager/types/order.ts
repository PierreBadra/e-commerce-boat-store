export type Order = {
    orderId: number, 
    customerId: number,
    dateCreated: string,
    dateFulfilled: string,
    customerEmail: string,
    customerName: string,
    total: number,
    taxes: number
}