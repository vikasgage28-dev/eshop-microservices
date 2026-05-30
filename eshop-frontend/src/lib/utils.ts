import { clsx, type ClassValue } from "clsx"
import { twMerge } from "tailwind-merge"
import { CURRENCY_LOCALE, CURRENCY_CODE } from "./constants"
import type { OrderStatus } from "@/types/ordering.types"

export function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs))
}

// ── Currency formatter ────────────────────────────────────────────
// Uses Intl.NumberFormat — native browser API, zero dependencies.
// Change CURRENCY_LOCALE / CURRENCY_CODE in constants.ts to update everywhere.
// Examples:
//   formatCurrency(1999)    → "₹1,999.00"
//   formatCurrency(1999.5)  → "₹1,999.50"
// ── Order status badge colours ────────────────────────────────────
// Shared by OrdersPage, DashboardPage, OrderDetailPage etc.
export const orderStatusColor: Record<OrderStatus, string> = {
  Pending:    'bg-yellow-100 text-yellow-700 dark:bg-yellow-900/30 dark:text-yellow-400',
  Confirmed:  'bg-blue-100 text-blue-700 dark:bg-blue-900/30 dark:text-blue-400',
  Processing: 'bg-purple-100 text-purple-700 dark:bg-purple-900/30 dark:text-purple-400',
  Shipped:    'bg-indigo-100 text-indigo-700 dark:bg-indigo-900/30 dark:text-indigo-400',
  Delivered:  'bg-green-100 text-green-700 dark:bg-green-900/30 dark:text-green-400',
  Cancelled:  'bg-red-100 text-red-700 dark:bg-red-900/30 dark:text-red-400',
}

export function formatCurrency(amount: number): string {
  return new Intl.NumberFormat(CURRENCY_LOCALE, {
    style:                 'currency',
    currency:              CURRENCY_CODE,
    minimumFractionDigits: 2,
    maximumFractionDigits: 2,
  }).format(amount)
}
