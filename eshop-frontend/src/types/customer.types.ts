export interface Address {
  id: string
  street: string
  city: string
  state: string
  country: string
  postalCode: string
  isDefault: boolean
}

export interface Customer {
  id: string
  firstName: string
  lastName: string
  fullName: string
  email: string
  phone?: string
  createdAt: string
  updatedAt?: string
  addresses: Address[]
}