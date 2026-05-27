// Backend serializes enum as string thanks to JsonStringEnumConverter
export type OrderStatus = 'Pending' | 'Confirmed' | 'Processing' | 'Shipped' | 'Delivered' | 'Cancelled'

export interface OrderItem {
  id: string
  productId: string
  productName: string
  quantity: number
  unitPrice: number
  totalPrice: number
}

export interface Order {
  id: string
  customerId: string
  customerEmail: string
  status: OrderStatus
  statusName: string       // computed string from backend (always safe to display)
  totalAmount: number
  orderDate: string        // backend field name (camelCase of OrderDate)
  shippingAddress?: string
  notes?: string
  items: OrderItem[]
}