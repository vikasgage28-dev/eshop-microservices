import { clsx, type ClassValue } from "clsx"
import { twMerge } from "tailwind-merge"
import { CURRENCY_LOCALE, CURRENCY_CODE } from "./constants"

export function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs))
}

// ── Currency formatter ────────────────────────────────────────────
// Uses Intl.NumberFormat — native browser API, zero dependencies.
// Change CURRENCY_LOCALE / CURRENCY_CODE in constants.ts to update everywhere.
// Examples:
//   formatCurrency(1999)    → "₹1,999.00"
//   formatCurrency(1999.5)  → "₹1,999.50"
export function formatCurrency(amount: number): string {
  return new Intl.NumberFormat(CURRENCY_LOCALE, {
    style:                 'currency',
    currency:              CURRENCY_CODE,
    minimumFractionDigits: 2,
    maximumFractionDigits: 2,
  }).format(amount)
}
