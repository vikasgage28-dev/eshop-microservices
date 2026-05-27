export interface Category {
  id: string
  name: string
  description: string
}

export interface Product {
  id: string
  name: string
  description: string
  price: number
  stock: number
  categoryId: string
  categoryName: string
}

export interface Review {
  id: string
  productId: string
  userId: string
  userEmail: string
  rating: number
  comment: string
  verifiedPurchase: boolean
  createdAt: string
}

export interface PagedResult<T> {
  items: T[]
  totalCount: number
  page: number
  pageSize: number
  totalPages: number
}